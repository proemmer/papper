using System;
using System.Buffers.Binary;
using System.Linq;
using Papper.Internal;

namespace Papper.Types
{
    internal class PlcDWord : PlcObject
    {
        public PlcDWord(string name) :
            base(name)
        {
            Size = new PlcSize { Bytes = 4 };
        }

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            if (data.IsEmpty)
                return default;
            return BinaryPrimitives.ReadUInt32BigEndian(data.Slice(plcObjectBinding.Offset));
        }

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            BinaryPrimitives.WriteUInt32BigEndian(data.Slice(plcObjectBinding.Offset), Convert.ToUInt32(value));
            //var subset = Convert.ToUInt32(value).SetSwap();
            //for (var i = 0; i < subset.Length; i++)
            //    data[plcObjectBinding.Offset + i] = subset[i];
        }
    }
}
