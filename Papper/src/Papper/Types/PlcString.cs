using System;
using System.Linq;
using System.Text;
using Papper.Helper;

namespace Papper.Types
{
    internal class PlcString : PlcObject
    {
        private const int DefaultStringLength = 255;
        private const char DefaultFillChar = '\0';
        private readonly byte _defaultFillByte;
        private readonly PlcSize _size = new PlcSize();

        public int StringLength
        {
            get { return _size.Bytes; }
            set { _size.Bytes = value + 2; }
        }

        public override PlcSize Size
        {
            get { return _size; }
            protected set { ; } 
        }


        public PlcString(string name) 
            : base(name)
        {
            _defaultFillByte = Convert.ToByte(DefaultFillChar);
            StringLength = DefaultStringLength;
        }

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding)
        {
            if (plcObjectBinding.Data == null || !plcObjectBinding.Data.Any())
                return string.Empty;

            //var subset = plcObjectBinding.Data.Skip(plcObjectBinding.Offset).Take(Size.Bytes).ToArray();
            //var maxLength = subset.Take(1).ToArray().GetSwap<byte>();
            //var curLength = subset.Skip(1).Take(1).ToArray().GetSwap<byte>();
            //var take = Math.Min(maxLength, curLength);
            //return Encoding.Default.GetString(subset.Skip(2).Take(take).ToArray());

            var maxLength = plcObjectBinding.Data.GetSwap<byte>(plcObjectBinding.Offset);
            var curLength = plcObjectBinding.Data.GetSwap<byte>(plcObjectBinding.Offset+1);
            var take = Math.Min(Math.Min(maxLength, curLength), Size.Bytes-2);
            return Encoding.ASCII.GetString(plcObjectBinding.Data, plcObjectBinding.Offset + 2,take);
        }

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding)
        {
            var maxLength = Convert.ToByte(Size.Bytes - 2);
            var i = plcObjectBinding.Offset;
            plcObjectBinding.Data[i++] = maxLength.SetSwap()[0];
            var fill = string.Empty;
            if (value != null)
            {
                var str = value.ToString();
                var curLength = Convert.ToByte(str.Length);
                var take = Math.Min(maxLength, curLength);
                fill = str.Substring(0, take);

                plcObjectBinding.Data[i++] = take.SetSwap()[0];
                foreach (var c in fill)
                    plcObjectBinding.Data[i++] = Convert.ToByte(c);
            }
            else
            {
                plcObjectBinding.Data[i++] = 0;  //currentLength
            }

            for (var j = 0; j < (maxLength - fill.Length); j++)
                plcObjectBinding.Data[i + j] = _defaultFillByte;

        }

    }
}
