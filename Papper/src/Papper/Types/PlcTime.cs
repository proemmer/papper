using System;
using System.Buffers.Binary;
using Papper.Internal;

namespace Papper.Types
{
    internal class PlcTime : PlcObject
    {
        public override Type DotNetType => typeof(TimeSpan);

        public PlcTime(string name) : base(name)
         => Size = new PlcSize { Bytes = 4 };

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding, Span<byte> data)
         => data.IsEmpty ? TimeSpan.MinValue : new TimeSpan(BinaryPrimitives.ReadInt32BigEndian(data.Slice(plcObjectBinding.Offset)) * 10000);  // Is this really correct?

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding, Span<byte> data)
         => BinaryPrimitives.WriteInt32BigEndian(data.Slice(plcObjectBinding.Offset), Convert.ToInt32(((TimeSpan)value).TotalMilliseconds));
    }
}
