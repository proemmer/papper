using System;
using System.Buffers.Binary;
using Papper.Internal;

namespace Papper.Types
{
    internal class PlcTimeOfDay : PlcObject
    {
        public override Type DotNetType => typeof(TimeSpan);

        public PlcTimeOfDay(string name) : base(name)
         => Size = new PlcSize { Bytes = 4 };

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding, Span<byte> data)
         => data.IsEmpty ?  TimeSpan.MinValue : TimeSpan.FromMilliseconds(BinaryPrimitives.ReadUInt32BigEndian(data.Slice(plcObjectBinding.Offset)));
        

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding, Span<byte> data)
         => BinaryPrimitives.WriteUInt32BigEndian(data.Slice(plcObjectBinding.Offset), Convert.ToUInt32(((TimeSpan)value).TotalMilliseconds));
    }
}
