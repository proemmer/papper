using System;

namespace Papper.Internal
{
    internal class LruState
    {
        public DateTime LastUsage { get; set; }
        public byte[] Data { get; set; }

        public LruState(int size)
        {
            Data = new byte[size];
        }
    }
}
