using System;
using System.Linq;
using Papper.Helper;

namespace Papper.Types
{
    internal class PlcInt : PlcObject
    {
        public PlcInt(string name) : 
            base(name )
        {
            Size = new PlcSize { Bytes = 2 };
        }

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding)
        {
            if (plcObjectBinding.Data == null || !plcObjectBinding.Data.Any())
                return default(short);

            return plcObjectBinding.Data.GetSwap<short>(plcObjectBinding.Offset);
        }

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding)
        {
            var subset = Convert.ToInt16(value).SetSwap();
            for (var i = 0; i < subset.Length; i++)
                plcObjectBinding.Data[plcObjectBinding.Offset + i] = subset[i];
        }
    }
}
