using System;

namespace Papper
{
    public class DataPack
    {
        internal int Id { get; set; }
        public string? Selector { get; set; }
        public int Offset { get; set; }
        public int Length { get; set; }


        public byte BitMask => BitMaskBegin;

        public byte BitMaskBegin { get; set; }
        public byte BitMaskEnd { get; set; }

        public bool HasBitMask => BitMaskBegin != 0 || BitMaskEnd != 0;

        public Memory<byte> Data { get; internal set; }

        public DateTime Timestamp { get; private set; }

        public ExecutionResult ExecutionResult { get; set; }


        public DataPack ApplyData(Memory<byte> data)
        {
            Data = data;
            Timestamp = DateTime.Now;
            return this;
        }

        public override string ToString() => $"{Selector}.{Offset}.{Length}#{BitMaskBegin}#{BitMaskEnd}";
    }
}
