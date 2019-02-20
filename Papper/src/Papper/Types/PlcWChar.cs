using System;
using System.Text;
using Papper.Internal;

namespace Papper.Types
{
    internal class PlcWChar : PlcObject
    {
        public PlcWChar(string name) :  base(name)
         =>  Size = new PlcSize { Bytes = 2 };

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding, Span<byte> data)
         => data.IsEmpty ? default : Encoding.Unicode.GetString(data.Slice(plcObjectBinding.Offset, 2).ToArray());
        

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            Span<byte> s = Encoding.Unicode.GetBytes(value as string);
            s.Slice(0,2).CopyTo(data.Slice(plcObjectBinding.Offset, 2)); 
        }
    }
}
