using System;

namespace Papper
{
    public class DataPackAbsolute : DataPack
    {
        public string? Selector { get; set; }
        public int Offset { get; set; }
        public int Length { get; set; }
        public byte BitMask => BitMaskBegin;

        public byte BitMaskBegin { get; set; }
        public byte BitMaskEnd { get; set; }

        public bool HasBitMask => BitMaskBegin != 0 || BitMaskEnd != 0;

        public Memory<byte> Data { get; internal set; }

        public override DataPack ApplyResult<T>(ExecutionResult result, T value)
        {
            if (value is Memory<byte> mem)
            {
                Data = mem;
                Timestamp = DateTime.Now;
                ExecutionResult = result;
            }
            else if (value is byte[] by)
            {
                Data = by;
                Timestamp = DateTime.Now;
                ExecutionResult = result;
            }
            else
            {
                ExecutionResult = result == ExecutionResult.Ok ?  ExecutionResult.InvalidData : result;
            }
            return this;
        }
        public override string ToString() => $"{Selector}.{Offset}.{Length}#{BitMaskBegin}#{BitMaskEnd}";
    }

}
