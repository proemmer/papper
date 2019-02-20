using System;
using System.Buffers.Binary;
using Papper.Internal;

namespace Papper.Types
{
    internal class PlcLTime : PlcObject
    {
        public PlcLTime(string name) :
            base(name)
         => Size = new PlcSize { Bytes = 8 };

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding, Span<byte> data)
               => data.IsEmpty ? TimeSpan.MinValue : new TimeSpan(BinaryPrimitives.ReadInt64BigEndian(data.Slice(plcObjectBinding.Offset)) / 100); 

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding, Span<byte> data)
         =>  BinaryPrimitives.WriteInt64BigEndian(data.Slice(plcObjectBinding.Offset), Convert.ToInt64(((TimeSpan)value).Ticks * 100));

    }
}
