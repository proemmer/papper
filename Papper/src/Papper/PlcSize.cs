namespace Papper
{
    public class PlcSize
    {
        public int Bytes { get; internal set; }
        public int Bits{ get; internal set; }
        public static PlcSize operator +(PlcSize a, PlcSize b)
        {
            var bits = a.Bits + b.Bits;
            var bytes = a.Bytes + b.Bytes;

            return new PlcSize
            {
                Bytes = bytes,
                Bits = bits
            };
        }
    }
}
