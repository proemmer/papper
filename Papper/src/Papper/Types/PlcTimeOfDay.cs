using Papper.Internal;
using System;
using System.Buffers.Binary;

namespace Papper.Types
{
    internal class PlcTimeOfDay : PlcObject
    {
        // Use share size for this data type, we will never change the size
        private static readonly PlcSize _size = new() { Bytes = 4 };
        public override Type DotNetType => typeof(TimeSpan);

        public PlcTimeOfDay(string name) : base(name)
         => Size = _size;

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding, Span<byte> data)
         => data.IsEmpty ? TimeSpan.MinValue : TimeSpan.FromMilliseconds(BinaryPrimitives.ReadUInt32BigEndian(data[plcObjectBinding.Offset..]));


        public override void ConvertToRaw(object? value, PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            if (value is TimeSpan ts)
            {
                BinaryPrimitives.WriteUInt32BigEndian(data[plcObjectBinding.Offset..],
                                                             Convert.ToUInt32(ts.TotalMilliseconds));
            }
            else
            {
                BinaryPrimitives.WriteUInt32BigEndian(data[plcObjectBinding.Offset..],
                                                             Convert.ToUInt32(TimeSpan.MinValue.TotalMilliseconds));
            }
        
        } 
    }
}
