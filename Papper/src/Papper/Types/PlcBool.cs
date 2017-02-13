using System.Linq;
using Papper.Helper;
using System;

namespace Papper.Types
{
    internal class PlcBool : PlcObject
    {
        public PlcBool(string name) : 
            base(name)
        {
            Size = new PlcSize { Bits = 1 };
            AllowOddByteOffsetInArray = true;
        }

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding)
        {
            if (plcObjectBinding.Data == null || !plcObjectBinding.Data.Any())
                return default(bool);

            var baseOffset = plcObjectBinding.Offset + (plcObjectBinding.MetaData.Offset.Bits) / 8;
            var bit = plcObjectBinding.Offset + plcObjectBinding.MetaData.Offset.Bits - baseOffset;
            return plcObjectBinding.Data[baseOffset].GetBit(bit);
        }

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding)
        {
            var baseOffset = plcObjectBinding.Offset + (plcObjectBinding.MetaData.Offset.Bits) / 8;
            var bit = plcObjectBinding.Offset + plcObjectBinding.MetaData.Offset.Bits - baseOffset;
            plcObjectBinding.Data[baseOffset] = plcObjectBinding.Data[baseOffset].SetBit(bit, Convert.ToBoolean(value));
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
