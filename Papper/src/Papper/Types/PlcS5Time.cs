using System;
using Papper.Internal;

namespace Papper.Types
{
    internal class PlcS5Time : PlcObject
    {

        public override Type DotNetType => typeof(TimeSpan);
        public PlcS5Time(string name) :
            base(name)
        {
            Size = new PlcSize { Bytes = 2 };
        }

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            if (data.IsEmpty)
            {
                return TimeSpan.MinValue;
            }
            var w1 = data[plcObjectBinding.Offset + 1].GetBcdByte();
            var idx0Value = data[plcObjectBinding.Offset];
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

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding, Span<byte> data)
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

            data[plcObjectBinding.Offset] = Convert.ToByte(valueBase << 4 | p3);
            data[plcObjectBinding.Offset + 1] = Convert.ToByte((p2 << 4 | p1));
        }
    }
}
