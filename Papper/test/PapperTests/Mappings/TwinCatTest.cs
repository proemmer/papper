
using Papper.Attributes;
using System;

namespace Papper.Tests.Mappings
{

    public class STSampel
    {
        public bool bVar { get; set; }
        [StringLength(81)]
        public string SVar { get; set; }
    }


    [Mapping("MAIN:fbSample1", "MAIN.fbSample1", 0)]
    public class FBSample
    {
        public long FbPointer { get; set; }
        public bool bEnable { get; set; }
        public bool bEnable2 { get; set; }
        public short XX { get; set; }
        public short X1 { get; set; }
        public double fParameter { get; set; }
        public short XY { get; set; }

        [StringLength(81)]
        public string sOutput { get; set; }
        public short nCycleCounter { get; set; }
        public double fDivision { get; set; }

        public STSampel stSample { get; set; }
    }
    
    
    //[Mapping("MAIN", "DB250", 0)]
    //public class BeckoffMain
    //{
    //    public FBSample fbSample1 { get; set; }
    //    public bool bEnable1 { get; set; }
        

    //}

}

