using System;
using System.Buffers.Binary;
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
            return TimeSpan.FromMilliseconds(BinaryPrimitives.ReadUInt32BigEndian(data.Slice(plcObjectBinding.Offset)));
        }

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            var time = (TimeSpan)value;
            BinaryPrimitives.WriteUInt32BigEndian(data.Slice(plcObjectBinding.Offset), Convert.ToUInt32(time.TotalMilliseconds));
        }
    }
}
