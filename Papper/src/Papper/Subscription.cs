using Papper.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Papper
{
    public class Subscription : IDisposable
    {
        private readonly TaskCompletionSource<object> _watchingTcs = new TaskCompletionSource<object>();
        private readonly PlcDataMapper _mapper;
        private readonly LruCache _lruCache = new LruCache();
        private readonly List<PlcReadReference> _variables = new List<PlcReadReference>();
        private readonly ReaderWriterLockSlim _lock;

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
        public Subscription(PlcDataMapper mapper, PlcReadReference[] vars = null)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _lock = new ReaderWriterLockSlim();
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
        public void AddItems(params PlcReadReference[] vars)
        {
            using (new WriterGuard(_lock))
            {
                _variables.AddRange(vars);
                _modified = true;
            }
        }

        /// <summary>
        /// Remove items from the watch list. This items will be removed before the next what cycle.
        /// The internal 
        /// </summary>
        /// <param name="vars"></param>
        public void RemoveItems(params PlcReadReference[] vars)
        {
            using (new WriterGuard(_lock))
            {
                _modified = vars.Select(item => _variables.Remove(item)).Any() | _modified;
            }
        }

        /// <summary>
        /// Starts a detection an returns the changes if any occurred
        /// </summary>
        /// <returns></returns>
        public async Task<ChangeResult> DetectChangesAsync()
        {
            _cts = new CancellationTokenSource();
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
                        _executions = _mapper.DetermineExecutions(_variables);
                        _modified = false;
                    }
                }

                // determine outdated
                var needUpdate = _mapper.UpdateableItems(_executions);
                    
                // read outdated
                await _mapper.ReadFromPlc(needUpdate); // Update the read cache;

                var detect = DateTime.Now;
                var readRes = _mapper.CreatePlcReadResults(needUpdate.Keys, needUpdate, _lastRun, (x) => FilterChanged(detect, x));
                if (readRes.Length > 0)
                {
                    _lastRun = detect;
                    return new ChangeResult(readRes, Watching.IsCanceled, Watching.IsCompleted);
                }

                if (_cts.IsCancellationRequested)
                {
                    return new ChangeResult(null, true, Watching.IsCompleted);
                }

                await Task.Delay(Interval, _cts.Token);

                if (_cts.IsCancellationRequested)
                {
                    return new ChangeResult(null, true, Watching.IsCompleted);
                }
            }
            return new ChangeResult(null, Watching.IsCanceled, Watching.IsCompleted);
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
                        ? objBinding.Data[objBinding.Offset].GetBit(objBinding.MetaData.Offset.Bits) != saved.Data[0].GetBit(objBinding.MetaData.Offset.Bits)
                        : !objBinding.Data.SequenceEqual(objBinding.Offset, saved.Data, 0, size)))
                {
                    result.Add(binding);
                    var data = objBinding.Data.AsSpan().Slice(objBinding.Offset, size);
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


    }
}