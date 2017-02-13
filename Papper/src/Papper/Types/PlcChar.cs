using System;
using System.Linq;
using Papper.Helper;

namespace Papper.Types
{
    internal class PlcChar : PlcObject
    {
        public PlcChar(string name) :
            base(name)
        {
            Size = new PlcSize { Bytes = 1 };
            AllowOddByteOffsetInArray = true;
        }

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding)
        {
            if (plcObjectBinding.Data == null || !plcObjectBinding.Data.Any())
                return default(char);

            return Convert.ToChar(plcObjectBinding.Data[plcObjectBinding.Offset]);
        }

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding)
        {
            var s = value as string;
            plcObjectBinding.Data[plcObjectBinding.Offset] = s != null ? Convert.ToByte(s.FirstOrDefault()) : Convert.ToByte(value);
        }
    }
}
