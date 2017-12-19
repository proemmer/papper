using System;
using System.Collections.Generic;
using System.Linq;

namespace Papper.Internal
{
    internal class LruCache : IDisposable
    {
        private Dictionary<string, LruState> _states = new Dictionary<string, LruState>();

        public bool TryGetValue(string key, out LruState state)
        {
            return _states.TryGetValue(key, out state);
        }

        public LruState Create(string key, Span<byte> data, DateTime detect, int validationTime)
        {
            var state = new LruState(data, detect, validationTime);
            _states.Add(key, state);
            return state;
        }

        public void Update(LruState state, DateTime detect)
        {
            state?.ApplyUsage(detect);
        }

        public void Update(LruState state, Span<byte> data, DateTime detect)
        {
            state?.ApplyChange(data, detect);
        }

        public void RemoveUnused(DateTime detect)
        {
            _states.Where(x => x.Value.IsOutdated(detect)).ToList()
                   .Where(state => _states.Remove(state.Key)).ToList()
                   .ForEach(state => state.Value.Dispose());
        }

        public void Dispose()
        {
            _states.ToList()
                   .Where(state => _states.Remove(state.Key)).ToList()
                   .ForEach(state => state.Value.Dispose());
        }
    }
}
