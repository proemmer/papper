using Papper.Entries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Papper
{
    public class Subscription : IDisposable
    {
        private readonly TaskCompletionSource<object> _watchingTcs = new TaskCompletionSource<object>();
        private PlcDataMapper _mapper;
        private List<PlcReference> _variables = new List<PlcReference>();
        private List<Execution> _executions;
        private bool _modified = true;
        private Dictionary<string, LruState> _states = new Dictionary<string, LruState>();

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
            _mapper.RemoveSubscription(this);
        }

        public void AddItems(params PlcReference[] vars)
        {
            _variables.AddRange(vars);
            _modified = true;
        }

        public void RemoveItems(params PlcReference[] vars)
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
                while (!Watching.IsCompleted)
                {
                    if(_modified)
                    {
                        _executions = _mapper.DetermineExecutions(_variables);
                        _modified = false;
                    }

                    // determine outdated
                    var needUpdate = _mapper.UpdateableItems(_executions);
                    
                    // read outdated
                    await _mapper.ReadFromPlc(needUpdate);

                    var readRes = _mapper.CreatePlcReadResults(needUpdate.Keys, needUpdate, true);

                    if(readRes.Length > 0)
                    {
                        return new ChangeResult(readRes, Watching.IsCanceled, Watching.IsCompleted);
                    }

                    await Task.Delay(Interval);
                }
            }
            catch(Exception ex)
            {
                
            }
            return new ChangeResult(null, Watching.IsCanceled, Watching.IsCompleted);
        }


    }
}