using System;
using System.Buffers.Binary;
using System.Linq;
using Papper.Internal;

namespace Papper.Types
{
    internal class PlcDate : PlcObject
    {
        public PlcDate(string name) :
            base(name)
        {
            Size = new PlcSize { Bytes = 2 };
        }

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            var date = new DateTime(1990, 1, 1);
            if (data.IsEmpty)
                return date;
           
            return date.AddDays(BinaryPrimitives.ReadInt16BigEndian(data.Slice(plcObjectBinding.Offset)));
        }

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            var dateVal = (DateTime)value;
            var date = new DateTime(1990, 1, 1);
            BinaryPrimitives.WriteUInt16BigEndian(data.Slice(plcObjectBinding.Offset), Convert.ToUInt16(dateVal.Subtract(date).Days));
        }
    }
}
