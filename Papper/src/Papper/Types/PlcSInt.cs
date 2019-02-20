using System;
using Papper.Internal;

namespace Papper.Types
{
    internal class PlcSInt : PlcObject
    {
        public PlcSInt(string name) : 
            base(name )
        {
            Size = new PlcSize { Bytes = 1 };
        }

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            if (data.IsEmpty)
                return default;
            return unchecked((sbyte)data[plcObjectBinding.Offset]);
        }

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            
            data[plcObjectBinding.Offset] = (byte)unchecked((sbyte)value);
        }
    }
}
