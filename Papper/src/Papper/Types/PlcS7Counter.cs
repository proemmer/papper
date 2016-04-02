using System;
using System.Linq;
using Papper.Helper;

namespace Papper.Types
{
    internal class PlcS7Counter : PlcObject
    {
        public PlcS7Counter(string name) : 
            base(name )
        {
            Size = new PlcSize { Bytes = 2 };
        }

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding)
        {
            if (plcObjectBinding.Data == null || !plcObjectBinding.Data.Any())
                return default(short);

            return plcObjectBinding.Data.GetBcdWord(plcObjectBinding.Offset);
        }

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding)
        {
            var subset = Convert.ToInt32(value).SetBcdWord();
            for (var i = 0; i < subset.Length; i++)
                plcObjectBinding.Data[plcObjectBinding.Offset + i] = subset[i];
        }
    }
}
