using Papper.Internal;
using System;
using System.Buffers.Binary;

namespace Papper.Types
{
    internal class PlcTime : PlcObject
    {
        // Use share size for this data type, we will never change the size
        private static readonly PlcSize _size = new() { Bytes = 4 };
        public override Type DotNetType => typeof(TimeSpan);

        public PlcTime(string name) : base(name)
         => Size = _size;

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding, Span<byte> data)
         => data.IsEmpty ? TimeSpan.MinValue : TimeSpan.FromMilliseconds(BinaryPrimitives.ReadInt32BigEndian(data[plcObjectBinding.Offset..]));

        public override void ConvertToRaw(object? value, PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            if (value is TimeSpan ts)
            {
                BinaryPrimitives.WriteInt32BigEndian(data[plcObjectBinding.Offset..], 
                                                     Convert.ToInt32(ts.TotalMilliseconds));
            }
            else
            {
                BinaryPrimitives.WriteInt32BigEndian(data[plcObjectBinding.Offset..],
                                     Convert.ToInt32(TimeSpan.MinValue.TotalMilliseconds));
            }
        }
    }
}
