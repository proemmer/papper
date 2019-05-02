using Papper.Internal;
using System;
using System.Text;

namespace Papper.Types
{
    internal class PlcWChar : PlcObject
    {
        // Use share size for this datatype, we will never change the size
        private static readonly PlcSize _size = new PlcSize { Bytes = 2 };
        public override Type DotNetType => typeof(string);

        public PlcWChar(string name) : base(name)
         => Size = _size;

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding, Span<byte> data)
         => data.IsEmpty ? default : Encoding.BigEndianUnicode.GetString(data.Slice(plcObjectBinding.Offset, 2).ToArray());


        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            Span<byte> s = Encoding.BigEndianUnicode.GetBytes(value as string);
            s.Slice(0, 2).CopyTo(data.Slice(plcObjectBinding.Offset, 2));
        }
    }
}
