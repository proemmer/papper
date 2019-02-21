using System;
using Papper.Internal;

namespace Papper.Types
{
    internal class PlcS7Counter : PlcObject
    {

        public override Type DotNetType => typeof(int);

        public PlcS7Counter(string name) : 
            base(name )
        {
            Size = new PlcSize { Bytes = 2 };
        }

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding, Span<byte> data)
         => (data == null || data.IsEmpty) ? default : data.GetBcdWord(plcObjectBinding.Offset);

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            var subset = Convert.ToInt32(value).SetBcdWord();
            for (var i = 0; i < subset.Length; i++)
                data[plcObjectBinding.Offset + i] = subset[i];
        }
    }
}
