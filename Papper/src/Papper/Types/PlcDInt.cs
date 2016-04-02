using System;
using System.Linq;
using Papper.Helper;

namespace Papper.Types
{ 
    internal class PlcDInt : PlcObject
    {
        public PlcDInt(string name) :
            base(name)
        {
            Size = new PlcSize { Bytes = 4 };
        }

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding)
        {
            if (plcObjectBinding.Data == null || !plcObjectBinding.Data.Any())
            {
                return default(Int32);
            }
            return plcObjectBinding.Data.GetSwap<Int32>(plcObjectBinding.Offset);
        }

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding)
        {
            var subset = Convert.ToInt32(value).SetSwap();
            for (var i = 0; i < subset.Length; i++)
                plcObjectBinding.Data[plcObjectBinding.Offset + i] = subset[i];
        }
    }
}
