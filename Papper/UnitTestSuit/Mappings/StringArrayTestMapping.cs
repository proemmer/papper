using Papper.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTestSuit.Mappings
{
    [Mapping("STRING_ARRAY_TEST_MAPPING", "DB30", 0)]
    public class StringArrayTestMapping
    {
        [ArrayBounds(1, 10, 0)]
        [StringLength(35)]
        public string[] TEXT { get; set; }  //Texts Rows 1 - 10 Length 35

        [StringLength(10)]
        public string TEST { get; set; }
    }
}
