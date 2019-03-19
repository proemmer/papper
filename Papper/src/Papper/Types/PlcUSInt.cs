using Papper.Internal;
using System;
using System.Linq;

namespace Papper.Types
{
    internal class PlcUSInt : PlcObject
    {
        // Use share size for this datatype, we will never change the size
        private static readonly PlcSize _size = new PlcSize { Bytes = 1 };
        public override Type DotNetType => typeof(byte);


        public PlcUSInt(string name) : base(name)
        {
            Size = _size;
            AllowOddByteOffsetInArray = true;
        }

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding, Span<byte> data)
         => data.IsEmpty ? default : data[plcObjectBinding.Offset];

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            var s = value as string;
            data[plcObjectBinding.Offset] = s != null ? Convert.ToByte(s.FirstOrDefault()) : Convert.ToByte(value);
        }
    }
}
