namespace Papper
{
    public class DataPack
    {
        public string Selector { get; set; }
        public int Offset { get; set; }
        public int Length { get; set; }

        public byte BitMask { get; set; }

        public byte[] Data { get; internal set; }

        public ExecutionResult ExecutionResult { get; set; }


        public void ApplyData(byte[] data)
        {
            Data = data;
        }
    }
}
