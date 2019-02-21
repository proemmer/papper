using System;
using System.Buffers.Binary;
using Papper.Internal;

namespace Papper.Types
{
    internal class PlcULInt : PlcObject
    {
        public override Type DotNetType => typeof(ulong);

        public PlcULInt(string name) : base(name)
            => Size = new PlcSize { Bytes = 8 };

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding, Span<byte> data)
            => data.IsEmpty ? default : BinaryPrimitives.ReadUInt64BigEndian(data.Slice(plcObjectBinding.Offset));

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding, Span<byte> data)
            => BinaryPrimitives.WriteUInt64BigEndian(data.Slice(plcObjectBinding.Offset), Convert.ToUInt64(value));
    }
}
