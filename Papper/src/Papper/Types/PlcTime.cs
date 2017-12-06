using System;
using System.Linq;
using Papper.Internal;

namespace Papper.Types
{
    internal class PlcTime : PlcObject
    {
        public PlcTime(string name) :
            base(name)
        {
            Size = new PlcSize { Bytes = 4 };
        }

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding, byte[] data)
        {
            if (data == null || !data.Any())
            {
                return TimeSpan.MinValue;
            }
            return new TimeSpan(data.GetSwap<int>(plcObjectBinding.Offset) * 10000);  // Is this really correct?
        }

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding, byte[] data)
        {
            var time = (TimeSpan)value;
            var subset = Convert.ToInt32(time.TotalMilliseconds).SetSwap();
            for (var i = 0; i < subset.Length; i++)
                data[plcObjectBinding.Offset + i] = subset[i];
        }
    }
}
