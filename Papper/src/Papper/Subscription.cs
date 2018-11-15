using Papper.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Papper
{
    /// <summary>
    /// This representates a subscription to a plc value change detection.
    /// </summary>
    public class Subscription : IDisposable
    {
        private readonly TaskCompletionSource<object> _watchingTcs = new TaskCompletionSource<object>();
        private readonly PlcDataMapper _mapper;
        private readonly LruCache _lruCache = new LruCache();
        private readonly List<PlcReadReference> _variables = new List<PlcReadReference>();
        private readonly ReaderWriterLockSlim _lock;
        private readonly ChangeDetectionStrategy _changeDetectionStrategy;
        private AsyncAutoResetEvent<IEnumerable<DataPack>> _changeEvent;

        private CancellationTokenSource _cts;
        private List<Execution> _executions;
        private bool _modified = true;
        private DateTime _lastRun = DateTime.MinValue;


        /// <summary>
        /// Provides read access to the watch task.
        /// </summary>
        public Task Watching => _watchingTcs.Task;

        /// <summary>
        /// Unique Id of the subscription
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Detection interval (pause between detections)
        /// </summary>
        public int Interval { get; set; } = 1000;

        /// <summary>
        /// Returns the number of subscribed
        /// </summary>
        public int Count => _variables.Count;


        /// <summary>
        /// Create an instance of a subscription to detect plc data changes
        /// </summary>
        /// <param name="mapper">The reference to the plcDatamapper.</param>
        /// <param name="vars">The variables we should watch.</param>
        public Subscription(PlcDataMapper mapper, ChangeDetectionStrategy changeDetectionStrategy = ChangeDetectionStrategy.Polling , IEnumerable<PlcReadReference> vars = null)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _changeDetectionStrategy = changeDetectionStrategy;
            _lock = new ReaderWriterLockSlim();
            if(changeDetectionStrategy == ChangeDetectionStrategy.Event)
            {
                _changeEvent = new AsyncAutoResetEvent<IEnumerable<DataPack>>();
            }
            if (vars != null) AddItems(vars);

        }

        /// <summary>
        /// Disposing all allocations and unregister from mapper
        /// </summary>
        public void Dispose()
        {
            _watchingTcs.SetResult(null);
            _mapper.RemoveSubscription(this);
            _variables.Clear();
            _lruCache.Dispose();
            _lock.Dispose();
        }

        /// <summary>
        /// This method cancels the current call of DetectChangesAsync. After calling the DetectChangesAsync again,
        /// this cancellation is automatically reseted and ready for a newly cancellation.
        /// </summary>
        public void CancelCurrentDetection()
        {
            _cts?.Cancel();
        }

        /// <summary>
        /// Add items to the subscription. This items are active in the next watch cycle, so you will get a data change if the update was activated.
        /// </summary>
        /// <param name="vars">Vars to activate </param>
        public bool AddItems(params PlcReadReference[] vars) => AddItems(vars as IEnumerable<PlcReadReference>);

        /// <summary>
        /// Add items to the subscription. This items are active in the next watch cycle, so you will get a data change if the update was activated.
        /// </summary>
        /// <param name="vars">Vars to activate </param>
        public bool AddItems(IEnumerable<PlcReadReference> vars)
        {
            using (new WriterGuard(_lock))
            {
                _variables.AddRange(vars);
                return _modified = true;
            }
        }


        /// <summary>
        /// Remove items from the watch list. This items will be removed before the next what cycle.
        /// The internal 
        /// </summary>
        /// <param name="vars"></param>
        public bool RemoveItems(params PlcReadReference[] vars) => RemoveItems(vars as IEnumerable<PlcReadReference>);

        /// <summary>
        /// Remove items from the watch list. This items will be removed before the next what cycle.
        /// The internal 
        /// </summary>
        /// <param name="vars"></param>
        public bool RemoveItems(IEnumerable<PlcReadReference> vars)
        {
            using (new WriterGuard(_lock))
            {
                return _modified = vars.Any(item => _variables.Remove(item)) | _modified;
            }
        }

        /// <summary>
        /// Returns the plcreadresult form the subscriptions cache
        /// </summary>
        /// <param name="vars"></param>
        /// <returns></returns>
        public PlcReadResult[] ReadResultsFromCache(IEnumerable<PlcReadReference> vars)
        {
            var variables = vars.Select(x => x.Address).ToList();
            return _executions.GroupBy(exec => exec.ExecutionResult) // Group by execution result
                                                     .SelectMany(group => group.SelectMany(g => g.Bindings)
                                                                               .Where(b => variables.Contains(b.Key))
                                                                               .Select(b => new PlcReadResult(b.Key,
                                                                                                              b.Value?.ConvertFromRaw(b.Value.RawData.ReadDataCache.Span),
                                                                                                              group.Key)
                                                                               )).ToArray();
        }

        /// <summary>
        /// Starts a detection an returns the changes if any occurred
        /// </summary>
        /// <returns></returns>
        public async Task<ChangeResult> DetectChangesAsync()
        {
            var cts = new CancellationTokenSource();
            if (Interlocked.CompareExchange(ref _cts, cts, null) != null)
            {
                cts.Dispose();
                throw new InvalidOperationException($"More than one detection run at the same time is not supported!");
            }

            try
            {
                while (!Watching.IsCompleted)
                {
                    if (_cts.IsCancellationRequested)
                    {
                        return new ChangeResult(null, true, Watching.IsCompleted);
                    }

                    if (_modified)
                    {
                        using (new WriterGuard(_lock))
                        {
                            if (_modified)
                            {
                                _executions = _mapper.DetermineExecutions(_variables);
                                if (_changeDetectionStrategy == ChangeDetectionStrategy.Event)
                                {
                                    var needUpdate = _mapper.UpdateableItems(_executions, false);
                                    await _mapper.UpdateMonitoringItemsAsync(needUpdate.Values);
                                }
                                _modified = false;
                            }
                        }
                    }

                    if (_executions == null)
                    {
                        _modified = true;
                        continue;
                    }

                    PlcReadResult[] readRes = null;
                    var detect = DateTime.Now;
                    if (_changeDetectionStrategy == ChangeDetectionStrategy.Polling)
                    {
                        // determine outdated
                        var needUpdate = _mapper.UpdateableItems(_executions);

                        // read outdated
                        await _mapper.ReadFromPlcAsync(needUpdate); // Update the read cache;

                        // filter to get only changed items
                        readRes = _mapper.CreatePlcReadResults(needUpdate.Keys, needUpdate, _lastRun, (x) => FilterChanged(detect, x));
                    }
                    else
                    {
                        var packs = await _changeEvent.WaitAsync();
                        if (packs != null && packs.Any())
                        {
                            readRes = _mapper.CreatePlcReadResults(_executions, packs);
                        }
                    }

                    if (readRes != null && readRes.Any())
                    {
                        _lastRun = detect;
                        return new ChangeResult(readRes, Watching.IsCanceled, Watching.IsCompleted);
                    }

                    if (_cts.IsCancellationRequested)
                    {
                        return new ChangeResult(null, true, Watching.IsCompleted);
                    }

                    try
                    {
                        await Task.Delay(Interval, _cts.Token);
                    }
                    catch(TaskCanceledException){}

                    if (_cts.IsCancellationRequested)
                    {
                        return new ChangeResult(null, true, Watching.IsCompleted);
                    }
                }
                return new ChangeResult(null, Watching.IsCanceled, Watching.IsCompleted);
            }
            finally
            {
                _cts.Dispose();
                _cts = null;
            }
        }



        internal void OnDataChanged(IEnumerable<DataPack> changed)
        {
            if (_changeEvent != null)
            {
                _changeEvent.Set(changed);
            }
            else
            {
                throw new InvalidOperationException("This operation is not allowed for this change detection strategy");
            }
        }

        /// <summary>
        /// This filter detects which items are changed during the sleep  phase of the subscription and returns only the changed ones.
        /// </summary>
        /// <param name="detect">The timestamps of the start of the current detection run. Because we need a single time for filtering not DateTime.Now</param>
        /// <param name="all">All the items updated from the plc.</param>
        /// <returns>A all items where we detect a data change.</returns>
        private IEnumerable<KeyValuePair<string, PlcObjectBinding>> FilterChanged(DateTime detect, IEnumerable<KeyValuePair<string, PlcObjectBinding>> all)
        {
            var result = new List<KeyValuePair<string, PlcObjectBinding>>();
            foreach (var binding in all)
            {
                var objBinding = binding.Value;
                var size = objBinding.Size == 0 ? 1 : objBinding.Size;
                if (!_lruCache.TryGetValue(binding.Key, out LruState saved) ||
                    (objBinding.Size == 0
                        ? objBinding.Data.Span[objBinding.Offset].GetBit(objBinding.MetaData.Offset.Bits) != saved.Data.Span[0].GetBit(objBinding.MetaData.Offset.Bits)
                        : !objBinding.Data.Slice(objBinding.Offset, size).Span.SequenceEqual(saved.Data.Slice(0, size).Span)))
                {
                    result.Add(binding);
                    var data = objBinding.Data.Slice(objBinding.Offset, size);
                    if (saved == null)
                    {
                        saved = _lruCache.Create(binding.Key, data, detect, objBinding.ValidationTimeInMs);
                    }
                    else
                    {
                        _lruCache.Update(saved, data, detect);
                    }
                }
                else
                {
                    _lruCache.Update(saved, detect);
                }
                
            }

            _lruCache.RemoveUnused(detect);

            return result;
        }


        private void MapDataPacks()
        {

        }
    }
}