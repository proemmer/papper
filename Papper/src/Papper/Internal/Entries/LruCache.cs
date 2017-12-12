using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Papper.Internal
{
    internal class LruCache
    {
        private Dictionary<string, LruState> _states = new Dictionary<string, LruState>();



        public bool TryGetValue(string key, out LruState state)
        {
            return _states.TryGetValue(key, out state);
        }

        public LruState Create(string key, int size)
        {
            var state = new LruState(size);
            _states.Add(key, state);
            return state;
        }

        public void Update(LruState state, DateTime detect)
        {
            if(state != null) state.LastUsage = detect;
        }

        public void RemoveUnused(DateTime detect)
        {
            //Remove unused states
            foreach (var state in _states.Where(x => x.Value.LastUsage < detect).ToList())
                _states.Remove(state.Key);
        }
    }
}
