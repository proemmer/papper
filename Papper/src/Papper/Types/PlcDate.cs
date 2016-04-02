using System;
using System.Linq;
using Papper.Helper;

namespace Papper.Types
{
    internal class PlcDate : PlcObject
    {
        public PlcDate(string name) :
            base(name)
        {
            Size = new PlcSize { Bytes = 2 };
        }

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding)
        {
            var date = new DateTime(1990, 1, 1);
            if (plcObjectBinding.Data == null || !plcObjectBinding.Data.Any())
            {
                return date;
            }
            //var subset = plcObjectBinding.Data.Skip(plcObjectBinding.Offset).Take(Size.Bytes).ToArray();
            //return date.AddDays(subset.GetSwap<ushort>()) ;

            return date.AddDays(plcObjectBinding.Data.GetSwap<ushort>(plcObjectBinding.Offset));
        }

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding)
        {
            var dateVal = (DateTime)value;
            var date = new DateTime(1990, 1, 1);
            var subset = Convert.ToUInt16(dateVal.Subtract(date).Days).SetSwap();
            for (var i = 0; i < subset.Length; i++)
                plcObjectBinding.Data[plcObjectBinding.Offset + i] = subset[i];
        }
    }
}
