using System;

namespace Papper
{
    public class AbsoluteAdressedDataPack : DataPack
    {
        public string? Selector { get; set; }
        public int Offset { get; set; }
        public int Length { get; set; }
        public byte BitMask => BitMaskBegin;

        public byte BitMaskBegin { get; set; }
        public byte BitMaskEnd { get; set; }

        public bool HasBitMask => BitMaskBegin != 0 || BitMaskEnd != 0;

        public Memory<byte> Data { get; internal set; }

        public override DataPack ApplyData<T>(T data)
        {
            if (data is Memory<byte> mem)
            {
                Data = mem;
                Timestamp = DateTime.Now;
            }
            return this;
        }

        public override string ToString() => $"{Selector}.{Offset}.{Length}#{BitMaskBegin}#{BitMaskEnd}";
    }

}
