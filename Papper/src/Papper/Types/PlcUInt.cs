using System;
using System.Buffers.Binary;
using System.Linq;
using Papper.Internal;

namespace Papper.Types
{
    internal class PlcUInt : PlcObject
    {
        public PlcUInt(string name) : 
            base(name)
        {
            Size = new PlcSize { Bytes = 2 };
        }

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            if (data.IsEmpty)
                return default;
            return BinaryPrimitives.ReadUInt16BigEndian(data.Slice(plcObjectBinding.Offset));
        }

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            BinaryPrimitives.WriteUInt16BigEndian(data.Slice(plcObjectBinding.Offset), Convert.ToUInt16(value));
        }
    }
}
