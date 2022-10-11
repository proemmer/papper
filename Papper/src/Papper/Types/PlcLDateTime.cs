using Papper.Internal;
using System;
using System.Buffers.Binary;

namespace Papper.Types
{
    internal class PlcLDateTime : PlcObject
    {
        // Use share size for this data type, we will never change the size
        private static readonly PlcSize _size = new() { Bytes = 8 };
        private static readonly DateTime _epochTime = new(1970, 01, 01, 00, 00, 00);
        public override Type DotNetType => typeof(DateTime);

        public PlcLDateTime(string name) : base(name)
            => Size = _size;

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding, Span<byte> data)
            => data.IsEmpty ? _epochTime : _epochTime.AddTicks(BinaryPrimitives.ReadInt64BigEndian(data[plcObjectBinding.Offset..]) / 100);

        public override void ConvertToRaw(object? value, PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            if (value is DateTime dt)
            {
                BinaryPrimitives.TryWriteInt64BigEndian(data[plcObjectBinding.Offset..], (dt.Ticks - _epochTime.Ticks) * 100);
            }
            else
            {
                BinaryPrimitives.TryWriteInt64BigEndian(data[plcObjectBinding.Offset..], (_epochTime.Ticks) * 100);
            }
        }
    }
}
