using Papper.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace PapperTests.Mappings
{
    [Mapping("SampleData", "DB909", 0)]
    public class SampleData
    {
        [ReadOnly(true)]
        public UInt16 UInt16 { get; set; }
        public Int16 Int16 { get; set; }
        public UInt32 UInt32 { get; set; }
        public Int32 Int32 { get; set; }
        public Single Single { get; set; }
        public char Char { get; set; }

        public bool Bit1 { get; set; }

        [ReadOnly(true)]
        public bool Bit2 { get; set; }
        public bool Bit3 { get; set; }
        public bool Bit4 { get; set; }
        public bool Bit5 { get; set; }
        public bool Bit6 { get; set; }
        public bool Bit7 { get; set; }
        public bool Bit8 { get; set; }
    }


    [Mapping("SampleDataSubstruct", "DB989", 0)]
    public class SampleDataSubstruct
    {
        [ReadOnly(true)]
        public UInt16 UInt16 { get; set; }
        public SampleDataSubstruct1 SubStruct { get; set; }
        public SampleDataSubstruct2 SubStruct2 { get; set; }
    }

    public class SampleDataSubstruct1
    {
        [ReadOnly(true)]
        public UInt16 UInt16 { get; set; }
        public Int16 Int16 { get; set; }
    }

    public class SampleDataSubstruct2
    {
        public UInt16 UInt16 { get; set; }
        public Int16 Int16 { get; set; }
    }
}
