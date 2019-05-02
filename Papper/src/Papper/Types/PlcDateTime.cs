using Papper.Internal;
using System;

namespace Papper.Types
{
    internal class PlcDateTime : PlcObject
    {
        // Use share size for this datatype, we will never change the size
        private static readonly PlcSize _size = new PlcSize { Bytes = 8 };
        private static readonly DateTime _epochTime = new DateTime(1900, 01, 01, 00, 00, 00);
        public override Type DotNetType => typeof(DateTime);


        public PlcDateTime(string name) :
            base(name) => Size = _size;

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            if (data.IsEmpty)
                return _epochTime;

            int bt = data[plcObjectBinding.Offset];
            //BCD Umwandlung
            bt = (((bt >> 4)) * 10) + ((bt & 0x0f));
            var jahr = bt < 90 ? 2000 : 1900;
            jahr += bt;

            //Monat
            bt = data[plcObjectBinding.Offset + 1];
            var monat = (((bt >> 4)) * 10) + ((bt & 0x0f));

            //Tag
            bt = data[plcObjectBinding.Offset + 2];
            var tag = (((bt >> 4)) * 10) + ((bt & 0x0f));

            //Stunde
            bt = data[plcObjectBinding.Offset + 3];
            var stunde = (((bt >> 4)) * 10) + ((bt & 0x0f));

            //Minute
            bt = data[plcObjectBinding.Offset + 4];
            var minute = (((bt >> 4)) * 10) + ((bt & 0x0f));

            //Sekunde
            bt = data[plcObjectBinding.Offset + 5];
            var sekunde = (((bt >> 4)) * 10) + ((bt & 0x0f));

            //Milisekunden
            //Byte 6 BCD + MSB (Byte 7)
            bt = data[plcObjectBinding.Offset + 6];
            int bt1 = data[plcObjectBinding.Offset + 7];
            var mili = (((bt >> 4)) * 10) + ((bt & 0x0f));
            mili = mili * 10 + (bt1 >> 4);

            //Wochentag
            //LSB (Byte 7) 1=Sunday
            //bt = b[pos + 7];
            //wochentag = (bt1 & 0x0f); 
            if (jahr > 0 && monat > 0 && monat <= 12 && tag > 0 && stunde >= 0 && stunde <= 24 && minute >= 0 && minute < 60 && sekunde >= 0 && sekunde < 60)
            {
                try
                {
                    return new DateTime(jahr, monat, tag, stunde, minute, sekunde, mili, DateTimeKind.Local);
                }
                catch (Exception) { }
            }
            return _epochTime;
        }

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            var dateTime = (DateTime)value;

            var tmp = dateTime.Year - ((dateTime.Year / 100) * 100);
            data[plcObjectBinding.Offset] = Convert.ToByte((tmp / 10) << 4 | tmp % 10);

            tmp = dateTime.Month;
            data[plcObjectBinding.Offset + 1] = Convert.ToByte((tmp / 10) << 4 | tmp % 10);

            tmp = dateTime.Day;
            data[plcObjectBinding.Offset + 2] = Convert.ToByte((tmp / 10) << 4 | tmp % 10);

            tmp = dateTime.Hour;
            data[plcObjectBinding.Offset + 3] = Convert.ToByte((tmp / 10) << 4 | tmp % 10);

            tmp = dateTime.Minute;
            data[plcObjectBinding.Offset + 4] = Convert.ToByte((tmp / 10) << 4 | tmp % 10);

            tmp = dateTime.Second;
            data[plcObjectBinding.Offset + 5] = Convert.ToByte((tmp / 10) << 4 | tmp % 10);

            tmp = dateTime.Millisecond / 10;
            var rem = dateTime.Millisecond % 10;
            data[plcObjectBinding.Offset + 6] = Convert.ToByte((tmp / 10) << 4 | tmp % 10);

            tmp = (int)dateTime.DayOfWeek;
            data[plcObjectBinding.Offset + 7] = Convert.ToByte((rem) << 4 | tmp % 10);

        }
    }
}
