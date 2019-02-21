using System;
using System.Buffers.Binary;
using Papper.Internal;

namespace Papper.Types
{
    internal class PlcInt : PlcObject
    {
        public override Type DotNetType => typeof(short);

        public PlcInt(string name) :  base(name )
            => Size = new PlcSize { Bytes = 2 };
        

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding, Span<byte> data)
            => data.IsEmpty ? default : BinaryPrimitives.ReadInt16BigEndian(data.Slice(plcObjectBinding.Offset));

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding, Span<byte> data)
            => BinaryPrimitives.WriteInt16BigEndian(data.Slice(plcObjectBinding.Offset), Convert.ToInt16(value));
    }
}
