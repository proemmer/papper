using System;
using System.Buffers.Binary;
using Papper.Internal;

namespace Papper.Types
{
    internal class PlcUDInt : PlcObject
    {
        public PlcUDInt(string name) :
            base(name)
        {
            Size = new PlcSize { Bytes = 4 };
        }

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            if (data.IsEmpty)
                return default;
            return BinaryPrimitives.ReadUInt32BigEndian(data.Slice(plcObjectBinding.Offset));
        }

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            BinaryPrimitives.WriteUInt32BigEndian(data.Slice(plcObjectBinding.Offset), Convert.ToUInt32(value));
        }
    }
}
