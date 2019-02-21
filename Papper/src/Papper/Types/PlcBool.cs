using System.Linq;
using Papper.Internal;
using System;

namespace Papper.Types
{
    internal class PlcBool : PlcObject
    {

        public override Type DotNetType => typeof(bool);


        public PlcBool(string name) : 
            base(name)
        {
            Size = new PlcSize { Bits = 1 };
            AllowOddByteOffsetInArray = true;
        }

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            if (data.IsEmpty)
                return default;

            var baseOffset = plcObjectBinding.Offset + (plcObjectBinding.MetaData.Offset.Bits) / 8;
            var bit = plcObjectBinding.Offset + plcObjectBinding.MetaData.Offset.Bits - baseOffset;
            return data[baseOffset].GetBit(bit);
        }

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            var baseOffset = plcObjectBinding.Offset + (plcObjectBinding.MetaData.Offset.Bits) / 8;
            var bit = plcObjectBinding.Offset + plcObjectBinding.MetaData.Offset.Bits - baseOffset;
            data[baseOffset] = data[baseOffset].SetBit(bit, Convert.ToBoolean(value));
        }

        public void AssigneOffsetFrom(int bitoffset)
        {
            var offsetBits = bitoffset;
            var offsetByte = offsetBits / 8;
            offsetBits = offsetBits - offsetByte * 8;

            Offset.Bytes += offsetByte;
            Offset.Bits += offsetBits;
        }

    }
}
