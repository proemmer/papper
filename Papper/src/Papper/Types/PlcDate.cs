using Papper.Internal;
using System;
using System.Buffers.Binary;

namespace Papper.Types
{
    internal class PlcDate : PlcObject
    {
        // Use share size for this data type, we will never change the size
        private static readonly PlcSize _size = new PlcSize { Bytes = 2 };
        private static readonly DateTime _epochTime = new DateTime(1900, 01, 01, 00, 00, 00);
        public override Type DotNetType => typeof(DateTime);


        public PlcDate(string name) : base(name)
            => Size = _size;

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding, Span<byte> data)
            => data.IsEmpty ? _epochTime : _epochTime.AddDays(BinaryPrimitives.ReadInt16BigEndian(data.Slice(plcObjectBinding.Offset)));

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding, Span<byte> data)
            => BinaryPrimitives.WriteUInt16BigEndian(data.Slice(plcObjectBinding.Offset), Convert.ToUInt16(((DateTime)value).Subtract(_epochTime).Days));
    }
}
