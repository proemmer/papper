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
            {
                return DateTime.MinValue;
            }
            return new DateTime(plcObjectBinding.Data.GetSwap<uint>(plcObjectBinding.Offset) * 10000);  // Is this really correct?
        }

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding)
        {
            var dateVal = (DateTime)value;
            var time = new TimeSpan(0, dateVal.Hour, dateVal.Minute, dateVal.Second, dateVal.Millisecond);
            var subset = Convert.ToUInt32(time.TotalMilliseconds).SetSwap();
            for (var i = 0; i < subset.Length; i++)
                plcObjectBinding.Data[plcObjectBinding.Offset + i] = subset[i];
        }
    }
}
