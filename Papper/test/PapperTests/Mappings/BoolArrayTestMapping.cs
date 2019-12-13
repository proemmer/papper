using Papper.Attributes;
#pragma warning disable CA1819 // Properties should not return arrays
namespace Papper.Tests.Mappings
{
    [Mapping("BOOL_ARRAY_TEST_MAPPING", "DB930", 0)]
    public class BoolArrayTestMapping
    {
        [ArrayBounds(1, 10, 0)]
        public bool[] NotFull { get; set; }  //Texts Rows 1 - 10 Length 35

        public bool Placeholder { get; set; }

        [ArrayBounds(1, 16, 0)]
        public bool[] Full { get; set; }  //Texts Rows 1 - 10 Length 35


    }
}
#pragma warning restore CA1819 // Properties should not return arrays