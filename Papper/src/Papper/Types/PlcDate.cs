using System;
using System.Linq;
using Papper.Internal;

namespace Papper.Types
{
    internal class PlcDate : PlcObject
    {
        public PlcDate(string name) :
            base(name)
        {
            Size = new PlcSize { Bytes = 2 };
        }

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding, byte[] data)
        {
            var date = new DateTime(1990, 1, 1);
            if (data == null || !data.Any())
            {
                return date;
            }
            //var subset = data.Skip(plcObjectBinding.Offset).Take(Size.Bytes).ToArray();
            //return date.AddDays(subset.GetSwap<ushort>()) ;

            return date.AddDays(data.GetSwap<ushort>(plcObjectBinding.Offset));
        }

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding, byte[] data)
        {
            var dateVal = (DateTime)value;
            var date = new DateTime(1990, 1, 1);
            var subset = Convert.ToUInt16(dateVal.Subtract(date).Days).SetSwap();
            for (var i = 0; i < subset.Length; i++)
                data[plcObjectBinding.Offset + i] = subset[i];
        }
    }
}
