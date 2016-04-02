using System;
using System.Linq;
using Papper.Helper;

namespace Papper.Types
{
    internal class PlcDateTime : PlcObject
    {
        public PlcDateTime(string name) :
            base(name)
        {
            Size = new PlcSize {Bytes = 8};
        }

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding)
        {
            if (plcObjectBinding.Data == null || !plcObjectBinding.Data.Any())
                return new DateTime(1900, 01, 01, 00, 00, 00);
            
            int bt = plcObjectBinding.Data[plcObjectBinding.Offset];
            //BCD Umwandlung
            bt = (((bt >> 4)) * 10) + ((bt & 0x0f));
            var jahr = bt < 90 ? 2000 : 1900;
            jahr += bt;

            //Monat
            bt = plcObjectBinding.Data[plcObjectBinding.Offset+1];
            var monat = (((bt >> 4)) * 10) + ((bt & 0x0f));

            //Tag
            bt = plcObjectBinding.Data[plcObjectBinding.Offset+2];
            var tag = (((bt >> 4)) * 10) + ((bt & 0x0f));

            //Stunde
            bt = plcObjectBinding.Data[plcObjectBinding.Offset+3];
            var stunde = (((bt >> 4)) * 10) + ((bt & 0x0f));

            //Minute
            bt = plcObjectBinding.Data[plcObjectBinding.Offset+4];
            var minute = (((bt >> 4)) * 10) + ((bt & 0x0f));

            //Sekunde
            bt = plcObjectBinding.Data[plcObjectBinding.Offset+5];
            var sekunde = (((bt >> 4)) * 10) + ((bt & 0x0f));

            //Milisekunden
            //Byte 6 BCD + MSB (Byte 7)
            bt = plcObjectBinding.Data[plcObjectBinding.Offset+6];
            int bt1 = plcObjectBinding.Data[plcObjectBinding.Offset+7];
            var mili = (((bt >> 4)) * 10) + ((bt & 0x0f));
            mili = mili * 10 + (bt1 >> 4);

            //Wochentag
            //LSB (Byte 7) 1=Sunday
            //bt = b[pos + 7];
            //wochentag = (bt1 & 0x0f); 
            try
            {
                return new DateTime(jahr, monat, tag, stunde, minute, sekunde, mili);
            }
            catch (Exception)
            {
                return new DateTime(1900, 01, 01, 00, 00, 00);
            }
        }

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding)
        {
            var dateTime = (DateTime) value;

            var tmp = dateTime.Year - ((dateTime.Year / 100) * 100);
            plcObjectBinding.Data[plcObjectBinding.Offset] = Convert.ToByte((tmp / 10) << 4 | tmp % 10);

            tmp = dateTime.Month;
            plcObjectBinding.Data[plcObjectBinding.Offset + 1] = Convert.ToByte((tmp / 10) << 4 | tmp % 10);

            tmp = dateTime.Day;
            plcObjectBinding.Data[plcObjectBinding.Offset + 2] = Convert.ToByte((tmp / 10) << 4 | tmp % 10);

            tmp = dateTime.Hour;
            plcObjectBinding.Data[plcObjectBinding.Offset + 3] = Convert.ToByte((tmp / 10) << 4 | tmp % 10);

            tmp = dateTime.Minute;
            plcObjectBinding.Data[plcObjectBinding.Offset + 4] = Convert.ToByte((tmp / 10) << 4 | tmp % 10);

            tmp = dateTime.Second;
            plcObjectBinding.Data[plcObjectBinding.Offset + 5] = Convert.ToByte((tmp / 10) << 4 | tmp % 10);

            tmp = dateTime.Millisecond / 10;
            var rem = dateTime.Millisecond % 10;
            plcObjectBinding.Data[plcObjectBinding.Offset + 6] = Convert.ToByte((tmp / 10) << 4 | tmp % 10);

            tmp = (int)dateTime.DayOfWeek;
            plcObjectBinding.Data[plcObjectBinding.Offset + 7] = Convert.ToByte((rem) << 4 | tmp % 10);

        }
    }
}
