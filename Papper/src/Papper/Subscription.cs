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
        private CancellationTokenSource _cts;
        private PlcDataMapper _mapper;
        private List<PlcReadReference> _variables = new List<PlcReadReference>();
        private List<Execution> _executions;
        private bool _modified = true;
        private Dictionary<string, LruState> _states = new Dictionary<string, LruState>();
        private DateTime _lastRun = DateTime.MinValue;

        /// <summary>
        /// Provides read access to the watch task.
        /// </summary>
        public Task Watching => _watchingTcs.Task;

        public Guid Id { get; set; } = Guid.NewGuid();
        public int Interval { get; set; } = 1000;



        public Subscription(PlcDataMapper mapper)
        {
            _mapper = mapper;
        }

        public void Dispose()
        {
            _watchingTcs.SetResult(null);
            _mapper.RemoveSubscription(this);
        }

        public void CancelCurrentDetection()
        {
            _cts?.Cancel();
        }

        public void AddItems(params PlcReadReference[] vars)
        {
            _variables.AddRange(vars);
            _modified = true;
        }

        public void RemoveItems(params PlcReadReference[] vars)
        {
            var modified = false;
            foreach (var item in vars)
            {
                if(_variables.Remove(item))
                    modified = true;
            }
            _modified = modified;
        }

        public async Task<ChangeResult> DetectChangesAsync()
        {
            try
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
                        _executions = _mapper.DetermineExecutions(_variables);
                        _modified = false;
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
                }
            }
            catch(Exception)
            {
                
            }
            return new ChangeResult(null, Watching.IsCanceled, Watching.IsCompleted);
        }

        private IEnumerable<KeyValuePair<string, PlcObjectBinding>> FilterChanged(DateTime detect, IEnumerable<KeyValuePair<string, PlcObjectBinding>> all)
        {
            var result = new List<KeyValuePair<string, PlcObjectBinding>>();
            foreach (var binding in all)
            {
                var size = binding.Value.Size == 0 ? 1 : binding.Value.Size;
                if (!_states.TryGetValue(binding.Key, out LruState saved) ||
                    (binding.Value.Size == 0
                        ? binding.Value.Data[binding.Value.Offset].GetBit(binding.Value.MetaData.Offset.Bits) != saved.Data[0].GetBit(binding.Value.MetaData.Offset.Bits)
                        : !binding.Value.Data.SequenceEqual(binding.Value.Offset, saved.Data, 0, size)))
                {
                    result.Add(binding);
                    if (saved == null)
                    {
                        saved = new LruState(size);
                        _states.Add(binding.Key, saved);
                    }
                }
                if (saved != null)
                    saved.LastUsage = detect;
            }

            //Remove unused states
            foreach (var state in _states.Where(x => x.Value.LastUsage < detect).ToList())
                _states.Remove(state.Key);

            return result;
        }


    }
}