using System;
using System.Linq;
using Papper.Internal;

namespace Papper.Types
{
    internal class PlcS7Counter : PlcObject
    {
        public PlcS7Counter(string name) : 
            base(name )
        {
            Size = new PlcSize { Bytes = 2 };
        }

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            if (data == null || data.IsEmpty)
                return default;

            return data.GetBcdWord(plcObjectBinding.Offset);
        }

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            var subset = Convert.ToInt32(value).SetBcdWord();
            for (var i = 0; i < subset.Length; i++)
                data[plcObjectBinding.Offset + i] = subset[i];
        }
    }
}
