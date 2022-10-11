﻿using Papper.Internal;
using System;
using System.Buffers.Binary;

namespace Papper.Types
{
    internal class PlcLTime : PlcObject
    {
        // Use share size for this data type, we will never change the size
        private static readonly PlcSize _size = new() { Bytes = 8 };
        public override Type DotNetType => typeof(TimeSpan);
        public PlcLTime(string name) :
            base(name)
         => Size = _size;

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding, Span<byte> data)
               => data.IsEmpty ? TimeSpan.MinValue : new TimeSpan(BinaryPrimitives.ReadInt64BigEndian(data[plcObjectBinding.Offset..]) / 100);

        public override void ConvertToRaw(object? value, PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            if (value is TimeSpan ts)
            {
                BinaryPrimitives.WriteInt64BigEndian(data[plcObjectBinding.Offset..],
                                                     Convert.ToInt64(ts.Ticks * 100));
            }
            else
            {
                BinaryPrimitives.WriteInt64BigEndian(data[plcObjectBinding.Offset..],
                                                    Convert.ToInt64(TimeSpan.MinValue.Ticks * 100));
            }
        }
    }
}
