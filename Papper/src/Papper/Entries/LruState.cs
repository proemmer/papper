using System;

namespace Papper.Entries
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
