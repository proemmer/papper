using System;
using System.Buffers;

namespace Papper.Internal
{
    internal class LruState : IDisposable
    {
        public int ValidationTime { get; private set; }
        public DateTime LastUsage { get; private set; }
        public Memory<byte> Data { get; private set; }


        public LruState(Memory<byte> data, DateTime detected, int validationTime)
        {
            ValidationTime = validationTime;
            Data = ArrayPool<byte>.Shared.Rent(data.Length);
            ApplyChange(data, detected);
        }

        public void ApplyUsage(DateTime detected) => LastUsage = detected;

        public void ApplyChange(Memory<byte> data, DateTime detected)
        {
            data.CopyTo(Data);
            LastUsage = detected;
        }

        public bool IsOutdated(DateTime currentCheckTime) => LastUsage.AddMilliseconds(ValidationTime) < currentCheckTime;

        public void Dispose() => ArrayPool<byte>.Shared.Return(Data.ToArray());

    }
}
