using System;
using System.Buffers.Binary;
using Papper.Internal;

namespace Papper.Types
{
    internal class PlcWord : PlcObject
    {
        public PlcWord(string name) :   base(name)
         => Size = new PlcSize { Bytes = 2 };

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding, Span<byte> data)
            => data.IsEmpty ? default : BinaryPrimitives.ReadUInt16BigEndian(data.Slice(plcObjectBinding.Offset));

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding, Span<byte> data)
            => BinaryPrimitives.WriteUInt16BigEndian(data.Slice(plcObjectBinding.Offset), Convert.ToUInt16(value));
    }
}
