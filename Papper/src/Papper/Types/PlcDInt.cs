using System;
using System.Buffers.Binary;
using System.Linq;
using Papper.Internal;

namespace Papper.Types
{ 
    internal class PlcDInt : PlcObject
    {
        public PlcDInt(string name) :
            base(name)
        {
            Size = new PlcSize { Bytes = 4 };
        }

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            if (data.IsEmpty)
                return default;

            return BinaryPrimitives.ReadInt32BigEndian(data.Slice(plcObjectBinding.Offset));
        }

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            BinaryPrimitives.WriteInt32BigEndian(data.Slice(plcObjectBinding.Offset), Convert.ToInt32(value));
        }
    }
}
