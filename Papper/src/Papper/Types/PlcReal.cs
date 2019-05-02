using Papper.Internal;
using System;

namespace Papper.Types
{
    internal class PlcReal : PlcObject
    {
        // Use share size for this datatype, we will never change the size
        private static readonly PlcSize _size = new PlcSize { Bytes = 4 };
        public override Type DotNetType => typeof(float);

        public PlcReal(string name) : base(name)
            => Size = _size;

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding, Span<byte> data)
         => data.IsEmpty ? default : Converter.ReadSingleBigEndian(data.Slice(plcObjectBinding.Offset));


        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding, Span<byte> data)
         => Converter.WriteSingleBigEndian(data.Slice(plcObjectBinding.Offset), Convert.ToSingle(value));
    }
}
