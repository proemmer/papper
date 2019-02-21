using System;
using System.Buffers.Binary;
using Papper.Internal;

namespace Papper.Types
{ 
    internal class PlcDInt : PlcObject
    {
        public override Type DotNetType => typeof(int);

        public PlcDInt(string name) : base(name)
         => Size = new PlcSize { Bytes = 4 };

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding, Span<byte> data)
            => data.IsEmpty ? default : BinaryPrimitives.ReadInt32BigEndian(data.Slice(plcObjectBinding.Offset));

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding, Span<byte> data)
            => BinaryPrimitives.WriteInt32BigEndian(data.Slice(plcObjectBinding.Offset), Convert.ToInt32(value));
    }
}
