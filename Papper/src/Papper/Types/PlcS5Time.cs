using System;
using System.Linq;
using Papper.Helper;

namespace Papper.Types
{
    internal class PlcS5Time : PlcObject
    {
        public PlcS5Time(string name) :
            base(name)
        {
            Size = new PlcSize { Bytes = 2 };
        }

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding)
        {
            if (plcObjectBinding.Data == null || !plcObjectBinding.Data.Any())
            {
                return TimeSpan.MinValue;
            }
            var subset = plcObjectBinding.Data.Skip(plcObjectBinding.Offset).Take(Size.Bytes).ToArray();

            var w1 = plcObjectBinding.Data[plcObjectBinding.Offset + 1].GetBcdByte();
            var idx0Value = plcObjectBinding.Data[plcObjectBinding.Offset];
            var w2 = ((idx0Value & 0x0f));
            long number = w2 * 100 + w1;
            switch ((idx0Value >> 4) & 0x03)
            {
                case 0:
                    number *= 100000;
                    break;
                case 1:
                    number *= 1000000;
                    break;
                case 2:
                    number *= 10000000;
                    break;
                case 3:
                    number *= 100000000;
                    break;

            }

            return new TimeSpan(number);  // Is this really correct?
        }

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding)
        {
            var time = (TimeSpan)value;
            byte valueBase;
            int val;

            if (time.TotalMilliseconds <= 999 * 10)
            {
                valueBase = 0;
                val = Convert.ToInt32(time.TotalMilliseconds) / 10;
            }
            else if (time.TotalMilliseconds <= 999 * 100)
            {
                valueBase = 1;
                val = Convert.ToInt32(time.TotalMilliseconds) / 100;
            }
            else if (time.TotalMilliseconds <= 999 * 1000)
            {
                valueBase = 2;
                val = Convert.ToInt32(time.TotalMilliseconds) / 1000;
            }
            else if (time.TotalMilliseconds <= 999 * 10000)
            {
                valueBase = 3;
                val = Convert.ToInt32(time.TotalMilliseconds) / 10000;
            }
            else
            {
                valueBase = 3;
                val = 999;
            }

            var p3 = (val / 100);
            var p2 = ((val - p3 * 100) / 10);
            var p1 = (val - p3 * 100 - p2 * 10);

            plcObjectBinding.Data[plcObjectBinding.Offset] = Convert.ToByte(valueBase << 4 | p3);
            plcObjectBinding.Data[plcObjectBinding.Offset] = Convert.ToByte((p2 << 4 | p1));
        }
    }
}
