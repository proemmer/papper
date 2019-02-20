using System;
using System.Buffers.Binary;
using Papper.Internal;

namespace Papper.Types
{
    internal class PlcLWord : PlcObject
    {
        public PlcLWord(string name) :
            base(name)
        {
            Size = new PlcSize { Bytes = 8 };
        }

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            if (data.IsEmpty)
                return default;
            return BinaryPrimitives.ReadUInt64BigEndian(data.Slice(plcObjectBinding.Offset));
        }

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            BinaryPrimitives.WriteUInt64BigEndian(data.Slice(plcObjectBinding.Offset), Convert.ToUInt64(value));
        }
    }
}
