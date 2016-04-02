using System;
using System.Linq;
using Papper.Helper;

namespace Papper.Types
{
    internal class PlcDWord : PlcObject
    {
        public PlcDWord(string name) :
            base(name)
        {
            Size = new PlcSize { Bytes = 4 };
        }

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding)
        {
            if (plcObjectBinding.Data == null || !plcObjectBinding.Data.Any())
            {
                return default(UInt32);
            }

            return plcObjectBinding.Data.GetSwap<UInt32>(plcObjectBinding.Offset);
        }

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding)
        {
            var subset = Convert.ToUInt32(value).SetSwap();
            for (var i = 0; i < subset.Length; i++)
                plcObjectBinding.Data[plcObjectBinding.Offset + i] = subset[i];
        }
    }
}
