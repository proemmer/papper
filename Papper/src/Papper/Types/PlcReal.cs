using System;
using Papper.Internal;

namespace Papper.Types
{
    internal class PlcReal : PlcObject
    {

        public PlcReal(string name) :
            base(name)
        {
            Size = new PlcSize { Bytes = 4 };
        }

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            if (data.IsEmpty)
                return default;

            return Converter.ReadSingleBigEndian(data.Slice(plcObjectBinding.Offset));
        }

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            Converter.WriteSingleBigEndian(data.Slice(plcObjectBinding.Offset), Convert.ToSingle(value));
        }
    }
}
