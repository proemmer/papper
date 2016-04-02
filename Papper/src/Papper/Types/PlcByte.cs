using System;
using System.Linq;
using Papper.Helper;

namespace Papper.Types
{
    internal class PlcByte : PlcObject
    {
        public PlcByte(string name) : 
            base(name)
        {
            Size = new PlcSize {Bytes = 1};
        }

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding)
        {
            if (plcObjectBinding.Data == null || !plcObjectBinding.Data.Any())
                return default(byte);

            return plcObjectBinding.Data[plcObjectBinding.Offset];
        }

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding)
        {
            var s = value as string;
            plcObjectBinding.Data[plcObjectBinding.Offset] = s != null ? Convert.ToByte(s.FirstOrDefault()) : Convert.ToByte(value);
        }
    }
}
