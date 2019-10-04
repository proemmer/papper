using Papper.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.TimeZoneInfo;

namespace PapperTests.Mappings
{
    public class TimeTransformationRule
    {
        public short Bias { get; set; } = 0; // Time bias of standard local time to UTC [min]
        public short DaylightBias { get; set; } = 0; //Time bias of local daylight saving time to local standard time [min]
        public byte DaylightStartMonth { get; set; } = 0; //Month of change to daylight saving time
        public byte DaylightStartWeek { get; set; } = 0;//Week of change to daylight saving time: 1= 1st occurrence of the weekday in the month, ..., 5= last occurrence
        public byte DaylightStartWeekday { get; set; } = 0; //Weekday of change to daylight saving time: 1= Sunday
        public byte DaylightStartHour { get; set; } = 0; //	Hour of change to daylight saving time
        public byte DaylightStartMinute { get; set; } = 0; //	Minute of change to daylight saving time
        public byte StandardStartMonth { get; set; } = 0; //	Month of change to standard time
        public byte StandardStartWeek { get; set; } = 0; //	Week of change to standard time: 1= 1st occurrence of the weekday in the month, ..., 5= last occurrence
        public byte StandardStartWeekday { get; set; } = 0; //	Weekday of change to standard time
        public byte StandardStartHour { get; set; } = 0; //	Hour of change to standard time
        public byte StandardStartMinute { get; set; } = 0; //	Minute of change to standard time

        [StringLength(80)]
        public string TimeZoneName { get; set; } //	Name of the used time zone like in Windows XP: "(GMT+01:00) Amsterdam, Berlin, Bern, Rome, Stockholm, Vienna"


        public static TimeTransformationRule FromTimeZoneInfo(TimeZoneInfo timeZoneInfo)
        {
            var result = new TimeTransformationRule();
            AdjustmentRule adjustmentRule = null;
            var adjustmentRules = timeZoneInfo.GetAdjustmentRules();
            if (adjustmentRules.Length > 0)
            {
                // Find the single record that encompasses today's date. If none exists, sets adjustmentRule to null.
                adjustmentRule = adjustmentRules.SingleOrDefault(ar => ar.DateStart <= DateTime.Now && DateTime.Now <= ar.DateEnd);
            }

            result.Bias = Convert.ToInt16(timeZoneInfo.BaseUtcOffset.TotalMinutes);
            var daylightName = timeZoneInfo.DaylightName;
            result.TimeZoneName = timeZoneInfo.StandardName;
            result.DaylightBias = Convert.ToInt16(adjustmentRule == null ? 60 : adjustmentRule.DaylightDelta.TotalMinutes); // Not sure why default is -60, or why this number needs to be negated, but it does.

            if (adjustmentRule != null)
            {
                var daylightTime = adjustmentRule.DaylightTransitionStart;
                var standardTime = adjustmentRule.DaylightTransitionEnd;

                result.DaylightStartMonth = Convert.ToByte(daylightTime.Month);
                result.DaylightStartWeek = Convert.ToByte(daylightTime.Week);
                result.DaylightStartWeekday = Convert.ToByte(daylightTime.DayOfWeek + 1);
                result.DaylightStartHour = Convert.ToByte(daylightTime.TimeOfDay.Hour);
                result.DaylightStartMinute = Convert.ToByte(daylightTime.TimeOfDay.Minute);

                result.StandardStartMonth = Convert.ToByte(standardTime.Month);
                result.StandardStartWeek = Convert.ToByte(standardTime.Week);
                result.StandardStartWeekday = Convert.ToByte(standardTime.DayOfWeek + 1);
                result.StandardStartHour = Convert.ToByte(standardTime.TimeOfDay.Hour);
                result.StandardStartMinute = Convert.ToByte(standardTime.TimeOfDay.Minute);
            }

            return result;
        }
    }
}
