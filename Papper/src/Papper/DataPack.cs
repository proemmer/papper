using System;

namespace Papper
{
    public class DataPack
    {
        public string Selector { get; set; }
        public int Offset { get; set; }
        public int Length { get; set; }

        public byte BitMask { get; set; }

        public Memory<byte> Data { get; internal set; }

        public ExecutionResult ExecutionResult { get; set; }


        public DataPack ApplyData(Memory<byte> data)
        {
            Data = data;
            return this;
        }

        public override string ToString()
        {
            return $"{Selector}.{Offset}.{Length}#{BitMask}";
        }
    }
}
