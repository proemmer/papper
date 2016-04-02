using System;
using System.Linq;
using Papper.Helper;

namespace Papper.Types
{
    internal class PlcWord : PlcObject
    {
        public PlcWord(string name) : 
            base(name)
        {
            Size = new PlcSize { Bytes = 2 };
        }

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding)
        {
            if (plcObjectBinding.Data == null || !plcObjectBinding.Data.Any())
                return default(ushort);

            return plcObjectBinding.Data.GetSwap<ushort>(plcObjectBinding.Offset);
        }

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding)
        {
            var subset = Convert.ToUInt16(value).SetSwap();
            for (var i = 0; i < subset.Length; i++)
                plcObjectBinding.Data[plcObjectBinding.Offset + i] = subset[i];
        }
    }
}
