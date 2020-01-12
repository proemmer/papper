﻿using Papper.Attributes;

#pragma warning disable CA1819 // Properties should not return arrays
namespace Papper.Tests.Mappings
{
    [Mapping("ARRAY_TEST_MAPPING_1", "DB21", 0)]
    [Mapping("ARRAY_TEST_MAPPING_2", "DB22", 0)]
    [Mapping("ARRAY_TEST_MAPPING_3", "DB23", 0)]
    [Mapping("ARRAY_TEST_MAPPING_4", "DB24", 0)]
    [Mapping("ARRAY_TEST_MAPPING_5", "DB25", 0)]
    public class ArrayTestMapping
    {
        [ArrayBounds(1, 50000, 0)]
        public byte[] BigByteArray { get; set; }

        [ArrayBounds(1, 50000, 0)]
        public char[] BigCharArray { get; set; }

        [ArrayBounds(1, 5000, 0)]
        public int[] BigIntArray { get; set; }

        [ArrayBounds(1, 10, 0)]
        public byte[] ByteElements { get; set; }

        [ArrayBounds(1, 10, 0)]
        public char[] CharElements { get; set; }

        [ArrayBounds(1, 10, 0)]
        public int[] IntElements { get; set; }
    }
}
#pragma warning restore CA1819 // Properties should not return arrays