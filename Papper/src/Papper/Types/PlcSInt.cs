using System;
using Papper.Internal;

namespace Papper.Types
{
    internal class PlcSInt : PlcObject
    {
        public override Type DotNetType => typeof(sbyte);

        public PlcSInt(string name) : 
            base(name )
        {
            Size = new PlcSize { Bytes = 1 };
            AllowOddByteOffsetInArray = true;
        }

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding, Span<byte> data)
            => data.IsEmpty ? default : unchecked((sbyte)data[plcObjectBinding.Offset]);


        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding, Span<byte> data)
         => data[plcObjectBinding.Offset] = (byte)unchecked((sbyte)value);

    }
}
