using Papper.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
