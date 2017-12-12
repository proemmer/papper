using System;
using System.Linq;
using Papper.Internal;

namespace Papper.Types
{
    internal class PlcTimeOfDay : PlcObject
    {
        public PlcTimeOfDay(string name) :
            base(name)
        {
            Size = new PlcSize { Bytes = 4 };
        }

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            if (data.IsEmpty)
                return TimeSpan.MinValue;
            return TimeSpan.FromMilliseconds(data.GetSwap<uint>(plcObjectBinding.Offset));
        }

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            var time = (TimeSpan)value;
            var subset = Convert.ToUInt32(time.TotalMilliseconds).SetSwap();
            for (var i = 0; i < subset.Length; i++)
                data[plcObjectBinding.Offset + i] = subset[i];
        }
    }
}
