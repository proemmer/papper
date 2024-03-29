﻿using Papper.Internal;
using System;
using System.Buffers.Binary;

namespace Papper.Types
{
    internal class PlcLTimeOfDay : PlcObject
    {
        // Use share size for this data type, we will never change the size
        private static readonly PlcSize _size = new() { Bytes = 8 };
        public override Type DotNetType => typeof(TimeSpan);

        public PlcLTimeOfDay(string name) : base(name)
         => Size = _size;

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding, Span<byte> data)
               => data.IsEmpty ? TimeSpan.MinValue : new TimeSpan(BinaryPrimitives.ReadInt64BigEndian(data.Slice(plcObjectBinding.Offset)) / 100);

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding, Span<byte> data)
         => BinaryPrimitives.WriteInt64BigEndian(data.Slice(plcObjectBinding.Offset), Convert.ToInt64(((TimeSpan)value).Ticks * 100));
    }
}
