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
        public int Bits { get; internal set; }

        /// <summary>
        /// operator to add plc sizes
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static PlcSize operator +(PlcSize a, PlcSize b) => Add(a, b);

        public static PlcSize Add(PlcSize left, PlcSize right)
        {
            var bits = 0;
            var bytes = 0;
            if (left != null && right != null)
            {
                bits = left.Bits + right.Bits;
                bytes = left.Bytes + right.Bytes;
            }
            else if (left == null && right != null)
            {
                bits = right.Bits;
                bytes = right.Bytes;
            }
            else if (left != null && right == null)
            {
                bits = left.Bits;
                bytes = left.Bytes;
            }

            return new PlcSize
            {
                Bytes = bytes,
                Bits = bits
            };
        }
    }
}
