using System;
using System.Buffers.Binary;
using Papper.Internal;

namespace Papper.Types
{
    internal class PlcLInt : PlcObject
    {
        public override Type DotNetType => typeof(long);

        public PlcLInt(string name) : base(name)
            => Size = new PlcSize { Bytes = 8 };

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding, Span<byte> data)
            => data.IsEmpty ? default : BinaryPrimitives.ReadInt64BigEndian(data.Slice(plcObjectBinding.Offset));

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding, Span<byte> data)
            => BinaryPrimitives.WriteInt64BigEndian(data.Slice(plcObjectBinding.Offset), Convert.ToInt64(value));

    }
}
