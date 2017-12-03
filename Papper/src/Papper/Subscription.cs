using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Papper
{
    public class Subscription : IDisposable
    {
        private PlcDataMapper _mapper;
        private List<PlcReference> _variables = new List<PlcReference>();
        private List<Execution> _executions;
        private bool _modified; 


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

        public Task<PlcReadResult[]> DetectChangesAsync()
        {
            return Task.FromResult(new PlcReadResult[0]);
        }



    }
}