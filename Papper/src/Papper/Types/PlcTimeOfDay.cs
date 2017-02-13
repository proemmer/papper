using System;
using System.Linq;
using Papper.Helper;

namespace Papper.Types
{
    internal class PlcTimeOfDay : PlcObject
    {
        public PlcTimeOfDay(string name) :
            base(name)
        {
            Size = new PlcSize { Bytes = 4 };
        }

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding)
        {
            if (plcObjectBinding.Data == null || !plcObjectBinding.Data.Any())
                return TimeSpan.MinValue;
            return TimeSpan.FromMilliseconds(plcObjectBinding.Data.GetSwap<uint>(plcObjectBinding.Offset));
        }

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding)
        {
            var time = (TimeSpan)value;
            var subset = Convert.ToUInt32(time.TotalMilliseconds).SetSwap();
            for (var i = 0; i < subset.Length; i++)
                plcObjectBinding.Data[plcObjectBinding.Offset + i] = subset[i];
        }
    }
}
