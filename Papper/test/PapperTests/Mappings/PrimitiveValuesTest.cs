using Papper.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace PapperTests.Mappings
{
    [Mapping("PrimitiveValuesMapping", "DB99", 0)]
    public class PrimitiveValuesMapping
    {
        public UInt16 UInt16 { get; set; }
        public Int16 Int16 { get; set; }
        public UInt32 UInt32 { get; set; }
        public Int32 Int32 { get; set; }
        public Single Single { get; set; }
        public char Char { get; set; }
    }
}
