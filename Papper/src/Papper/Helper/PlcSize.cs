namespace Papper.Helper
{
    internal class PlcSize
    {
        public int Bytes { get; set; }
        public int Bits{ get; set; }
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
