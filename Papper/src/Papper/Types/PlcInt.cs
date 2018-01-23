using System;
using System.Buffers.Binary;
using System.Linq;
using Papper.Internal;

namespace Papper.Types
{
    internal class PlcInt : PlcObject
    {
        public PlcInt(string name) : 
            base(name )
        {
            Size = new PlcSize { Bytes = 2 };
        }

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            if (data.IsEmpty)
                return default;

            return BinaryPrimitives.ReadInt16BigEndian(data.Slice(plcObjectBinding.Offset));
        }

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            BinaryPrimitives.WriteInt16BigEndian(data.Slice(plcObjectBinding.Offset), Convert.ToInt16(value));
        }
    }
}
