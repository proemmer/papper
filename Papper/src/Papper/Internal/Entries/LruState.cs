using System;
using System.Buffers;

namespace Papper.Internal
{
    internal class LruState : IDisposable
    {
        public DateTime LastUsage { get; set; }
        public byte[] Data { get; private set; }

        public LruState(int size)
        {
            Data = ArrayPool<byte>.Shared.Rent(size); ;
        }

        public void Dispose()
        {
            ArrayPool<byte>.Shared.Return(Data);
        }
    }
}
