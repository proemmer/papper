using System;
using System.Buffers.Binary;
using Papper.Internal;

namespace Papper.Types
{
    internal class PlcLDateTime : PlcObject
    {
        private static readonly DateTime _epochTime = new DateTime(1900, 01, 01, 00, 00, 00);
        public override Type DotNetType => typeof(DateTime);

        public PlcLDateTime(string name) : base(name)
            => Size = new PlcSize {Bytes = 8};

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding, Span<byte> data)
            => data.IsEmpty ? _epochTime : _epochTime.AddTicks(BinaryPrimitives.ReadInt64BigEndian(data.Slice(plcObjectBinding.Offset)) / 100);

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding, Span<byte> data)
            => BinaryPrimitives.TryWriteInt64BigEndian(data.Slice(plcObjectBinding.Offset), (((DateTime)value).Ticks - _epochTime.Ticks) * 100);
 
    }
}
