using System;
using System.Buffers.Binary;
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

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            if (data.IsEmpty)
            {
                return TimeSpan.MinValue;
            }
            return new TimeSpan(BinaryPrimitives.ReadInt32BigEndian(data.Slice(plcObjectBinding.Offset)) * 10000);  // Is this really correct?
        }

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            var time = (TimeSpan)value;
            BinaryPrimitives.WriteInt32BigEndian(data.Slice(plcObjectBinding.Offset), Convert.ToInt32(time.TotalMilliseconds));
        }
    }
}
