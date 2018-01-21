namespace Papper
{
    /// <summary>
    /// Define a Size of a Variable
    /// </summary>
    public class PlcSize
    {
        /// <summary>
        /// Get the offset in bytes
        /// </summary>
        public int Bytes { get; internal set; }

        /// <summary>
        /// Get the bit offset (0-7)
        /// </summary>
        public int Bits{ get; internal set; }

        /// <summary>
        /// operator to add plcsizes
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
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
