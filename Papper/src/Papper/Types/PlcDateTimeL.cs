using Papper.Internal;
using System;
using System.Buffers.Binary;

namespace Papper.Types
{
    internal class PlcDateTimeL : PlcObject
    {
        // Use share size for this data type, we will never change the size
        private static readonly PlcSize _size = new() { Bytes = 12 };
        private static readonly DateTime _epochTime = new(1970, 01, 01, 00, 00, 00);
        public override Type DotNetType => typeof(DateTime);

        public PlcDateTimeL(string name) : base(name)
            => Size = _size;

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            if (data.IsEmpty)
            {
                return _epochTime;
            }
            var source = data.Slice(plcObjectBinding.Offset);
            var nanoseconds = (int)(BinaryPrimitives.ReadUInt32BigEndian(source.Slice(8)));
            return AddNanoseconds(new DateTime(source[0] * 256 + source[1],
                                           source[2],
                                           source[3],
                                           source[5],
                                           source[6],
                                           source[7]),
                                           nanoseconds);
        }


        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            var dt = (DateTime)value;
            var destination = data.Slice(plcObjectBinding.Offset);
            destination[0] = (byte)(dt.Year / 256);
            destination[1] = (byte)(dt.Year % 256);
            destination[2] = (byte)dt.Month;
            destination[3] = (byte)dt.Day;
            destination[4] = (byte)(dt.DayOfWeek + 1); // 1(Sunday) to 7(Saturday)
            destination[5] = (byte)dt.Hour;
            destination[6] = (byte)dt.Minute;
            destination[7] = (byte)dt.Second;
            BinaryPrimitives.WriteUInt32BigEndian(destination.Slice(8), (uint)((dt.Millisecond * 1000000) + GetNanoseconds(dt)));
        }



        private static int GetNanoseconds(DateTime self)
        {
            const int nanosecondsPerTick = 100;
            return (int)(self.Ticks % TimeSpan.TicksPerMillisecond) * nanosecondsPerTick;
        }

        private static DateTime AddNanoseconds(DateTime self, int nanoseconds)
        {
            const int nanosecondsPerTick = 100;
            return self.AddTicks((int)Math.Round(nanoseconds / (double)nanosecondsPerTick));
        }

    }
}
