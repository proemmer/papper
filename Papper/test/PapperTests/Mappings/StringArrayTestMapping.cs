using Papper.Attributes;
using System;
#pragma warning disable CA1819 // Properties should not return arrays
namespace Papper.Tests.Mappings
{
    [Mapping("STRING_ARRAY_TEST_MAPPING", "DB30", 0)]
    [Mapping("STRING_ARRAY_TEST_MAPPING_1", "DB31", 0)]
    public class StringArrayTestMapping
    {
        [ArrayBounds(1, 10, 0)]
        [StringLength(35)]
        public string[] TEXT { get; set; }  //Texts Rows 1 - 10 Length 35

        [StringLength(10)]
        public string TEST { get; set; }

        [PlcType("TimeOfDay")]
        [ArrayBounds(1, 10, 0)]
        public TimeSpan[] Time { get; set; }
    }
}
#pragma warning restore CA1819 // Properties should not return arrays