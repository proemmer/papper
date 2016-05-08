using Papper.Attributes;
using System;

namespace UnitTestSuit.Mappings
{
    [Mapping("ARRAY_TEST_MAPPING", "DB16", 0)]
    public class ArrayTestMapping
    {
        [ArrayBounds(1, 50000, 0)]
        public byte[] BigByteArray { get; set; }

        [ArrayBounds(1, 50000, 0)]
        public char[] BigCharArray { get; set; }

        [ArrayBounds(1, 5000, 0)]
        public Int32[] BigIntArray { get; set; }

        [ArrayBounds(1, 10, 0)]
        public byte[] ByteElements { get; set; }

        [ArrayBounds(1, 10, 0)]
        public char[] CharElements { get; set; }

        [ArrayBounds(1, 10, 0)]
        public Int32[] IntElements { get; set; }
    }
}
