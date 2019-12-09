using Papper;
using Papper.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Papper.Extensions.Notification
{
    /// <summary>
    /// This representates a subscription to a plc value change detection.
    /// </summary>
    public sealed class Subscription : IDisposable
    {
        private readonly TaskCompletionSource<object?> _watchingTcs = new TaskCompletionSource<object?>();
        private readonly PlcDataMapper _mapper;
        private readonly LruCache _lruCache = new LruCache();
        private readonly Dictionary<string, PlcWatchReference> _variables = new Dictionary<string, PlcWatchReference>();
        private readonly ReaderWriterLockSlim _lock;
        private readonly ChangeDetectionStrategy _changeDetectionStrategy;
        private readonly int _defaultInterval;
        private readonly AsyncAutoResetEvent<IEnumerable<DataPack>>? _changeEvent;

        private CancellationTokenSource? _cts;
        private List<Execution>? _executions;
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
        public int Interval { get; set; }

        /// <summary>
        /// Returns the number of subscribed
        /// </summary>
        public int Count => _variables.Count;


        /// <summary>
        /// Create an instance of a subscription to detect plc data changes
        /// </summary>
        /// <param name="mapper">The reference to the plcDatamapper.</param>
        /// <param name="vars">The variables we should watch.</param>
        /// <param name="defaultInterval">setup the default interval, if none was given by the <see cref="PlcWatchReference"/></param>
        public Subscription(PlcDataMapper mapper, ChangeDetectionStrategy changeDetectionStrategy = ChangeDetectionStrategy.Polling , IEnumerable<PlcWatchReference>? vars = null, int defaultInterval = 1000)
        {
            _mapper = mapper ?? ExceptionThrowHelper.ThrowArgumentNullException<PlcDataMapper>(nameof(mapper));
            _changeDetectionStrategy = changeDetectionStrategy;
            _defaultInterval = defaultInterval;
            _lock = new ReaderWriterLockSlim();
            UpdateWatchCycle(vars);

            if (changeDetectionStrategy == ChangeDetectionStrategy.Event)
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
        public void AddItems(params PlcWatchReference[] vars) => AddItems(vars as IEnumerable<PlcWatchReference>);

        /// <summary>
        /// Add items to the subscription. This items are active in the next watch cycle, so you will get a data change if the update was activated.
        /// </summary>
        /// <param name="vars">Vars to activate </param>
        public bool TryAddItems(params PlcWatchReference[] vars) => TryAddItems(vars as IEnumerable<PlcWatchReference>);

        /// <summary>
        /// Add items to the subscription. This items are active in the next watch cycle, so you will get a data change if the update was activated.
        /// </summary>
        /// <param name="vars">Vars to activate </param>
        public void AddItems(IEnumerable<PlcWatchReference> vars) => InternalAddItems(vars, true);

        /// <summary>
        /// Add items to the subscription. This items are active in the next watch cycle, so you will get a data change if the update was activated.
        /// </summary>
        /// <param name="vars">Vars to activate </param>
        public bool TryAddItems(IEnumerable<PlcWatchReference> vars) => InternalAddItems(vars, false);



        /// <summary>
        /// Remove items from the watch list. This items will be removed before the next what cycle.
        /// The internal 
        /// </summary>
        /// <param name="vars"></param>
        public bool RemoveItems(params PlcWatchReference[] vars) => RemoveItems(vars as IEnumerable<PlcWatchReference>);

        /// <summary>
        /// Remove items from the watch list. This items will be removed before the next what cycle.
        /// The internal 
        /// </summary>
        /// <param name="vars"></param>
        public bool RemoveItems(IEnumerable<PlcWatchReference> vars)
        {
            using (new WriterGuard(_lock))
            {
                var result = vars.Any(item => _variables.Remove(item.Address)) | _modified;
                if(result)  UpdateWatchCycle(_variables.Values);
                _modified = result;
                return result;
            }
        }

        /// <summary>
        /// Returns the plc read result form the subscriptions cache
        /// </summary>
        /// <param name="vars"></param>
        /// <returns></returns>
        public PlcReadResult[]? ReadResultsFromCache(IEnumerable<PlcWatchReference> vars)
        {
            if (_executions!.IsNullOrEmpty() || !vars.Any()) return null;
            var variables = vars.Select(x => x.Address).ToList();
            using (new ReaderGuard(_lock))
            {
                if (_executions!.IsNullOrEmpty()) return null;
                return _executions.GroupBy(exec => exec.ExecutionResult) // Group by execution result
                                                     .SelectMany(group => group.SelectMany(g => g.Bindings)
                                                                               .Where(b => variables.Contains(b.Key))
                                                                               .Select(b => new PlcReadResult(b.Key,
                                                                                                              b.Value?.ConvertFromRaw(b.Value.RawData.ReadDataCache.Span),
                                                                                                              group.Key)
                                                                               )).ToArray();
            }
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
                ExceptionThrowHelper.ThrowMultipleDetectionsAreNotSupportedException();
            }

            try
            {
                Dictionary<string, int>? cycles = null;
                var interval = Interval;
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
                                cycles = _variables.Values.ToDictionary(x => x.Address, x => x.WatchCycle);
                                interval = Interval;
                                _executions = _mapper.DetermineExecutions(_variables.Values);
                                if (_changeDetectionStrategy == ChangeDetectionStrategy.Event)
                                {
                                    var needUpdate = PlcDataMapper.UpdateableItems(_executions, false);
                                    await _mapper.UpdateMonitoringItemsAsync(needUpdate.Values).ConfigureAwait(false);
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

                    PlcReadResult[]? readRes = null;
                    var detect = DateTime.Now;
                    if (_changeDetectionStrategy == ChangeDetectionStrategy.Polling || _changeEvent == null)
                    {
                        // determine outdated
                        var needUpdate = PlcDataMapper.UpdateableItems(_executions, true, DetermineChanges(cycles, interval));

                        // read outdated
                        await _mapper.ReadFromPlcAsync(needUpdate).ConfigureAwait(false); // Update the read cache;

                        // filter to get only changed items
                        readRes = PlcDataMapper.CreatePlcReadResults(needUpdate.Keys, needUpdate, _lastRun, (x) => FilterChanged(detect, x));
                    }
                    else
                    {
                        var packs = await _changeEvent.WaitAsync().ConfigureAwait(false);
                        if (packs != null && packs.Any())
                        {
                            readRes = PlcDataMapper.CreatePlcReadResults(_executions, packs);
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
                        await Task.Delay(interval, _cts.Token).ConfigureAwait(false);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cycles">the cycle interval of the current variable</param>
        /// <param name="cycleInterval">the minimum of all watches</param>
        /// <returns></returns>
        private static Func<IEnumerable<string>, DateTime, bool> DetermineChanges(Dictionary<string, int>? cycles, int cycleInterval)
            =>  (variables, lastChange) => cycles != null && 
                                           !cycles.Any() && 
                                           !string.IsNullOrWhiteSpace(variables.FirstOrDefault(x => cycles.TryGetValue(x, out var interval) && 
                                                                                                    lastChange.AddMilliseconds(interval < cycleInterval * 2 ? cycleInterval : interval) < DateTime.Now));

        internal void OnDataChanged(IEnumerable<DataPack> changed)
        {
            if (_changeEvent != null)
            {
                _changeEvent.Set(changed);
            }
            else
            {
                ExceptionThrowHelper.ThrowOperationNotAllowedForCurrentChangeDetectionStrategy();
            }
        }

        private bool InternalAddItems(IEnumerable<PlcWatchReference>? vars, bool throwExceptions)
        {
            if (vars == null || !vars.Any()) return false;
            using (new WriterGuard(_lock))
            {
                var variables = new Dictionary<string, PlcWatchReference>();
                foreach (var variable in vars)
                {
                    if (!_mapper.IsValidReference(variable))
                    {
                        if (throwExceptions)
                        {
                            ExceptionThrowHelper.ThrowInvalidVariableException(variable.Address);
                        }
                        return false;
                    }
                    variables.Add(variable.Address, variable);
                }

                if (variables.Any())
                {
                    // After we validate all items, we add all
                    // if one is not valid non of them will be added, this is because of consistence
                    foreach (var variable in variables.Values)
                    {
                        if (_variables.TryGetValue(variable.Address, out var current))
                        {
                            _variables[variable.Address] = variable;
                        }
                        else
                        {
                            _variables.Add(variable.Address, variable);
                        }
                    }


                    UpdateWatchCycle(_variables.Values);
                    return _modified = true;
                }
                return false;
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

                // only work on valid items
                if (!objBinding.Data.IsEmpty)
                {
                    var size = objBinding.Size == 0 ? 1 : objBinding.Size;
                    if (!_lruCache.TryGetValue(binding.Key, out var saved) ||
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
            }

            _lruCache.RemoveUnused(detect);

            return result;
        }


        private void UpdateWatchCycle(IEnumerable<PlcWatchReference>? vars)
        {
            if (vars != null)
            {
                Interval = vars.Select(x => x.WatchCycle).Min();
            }
            if (Interval <= 0)
            {
                Interval = _defaultInterval;
            }
        }

    }
}