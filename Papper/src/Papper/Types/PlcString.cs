using System;
using System.Linq;
using System.Text;
using Papper.Internal;

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

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            if (data.IsEmpty)
                return string.Empty;

            //var subset = data.Skip(plcObjectBinding.Offset).Take(Size.Bytes).ToArray();
            //var maxLength = subset.Take(1).ToArray().GetSwap<byte>();
            //var curLength = subset.Skip(1).Take(1).ToArray().GetSwap<byte>();
            //var take = Math.Min(maxLength, curLength);
            //return Encoding.Default.GetString(subset.Skip(2).Take(take).ToArray());

            var maxLength = data.GetSwap<byte>(plcObjectBinding.Offset);
            var curLength = data.GetSwap<byte>(plcObjectBinding.Offset+1);
            var take = Math.Min(Math.Min(maxLength, curLength), Size.Bytes-2);
            return Encoding.ASCII.GetString(data.ToArray(), plcObjectBinding.Offset + 2,take);
        }

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            var maxLength = Convert.ToByte(Size.Bytes - 2);
            var i = plcObjectBinding.Offset;
            data[i++] = maxLength.SetSwap()[0];
            var fill = string.Empty;
            if (value != null)
            {
                var str = value.ToString();
                var curLength = Convert.ToByte(str.Length);
                var take = Math.Min(maxLength, curLength);
                fill = str.Substring(0, take);

                data[i++] = take.SetSwap()[0];
                foreach (var c in fill)
                    data[i++] = Convert.ToByte(c);
            }
            else
            {
                data[i++] = 0;  //currentLength
            }

            for (var j = 0; j < (maxLength - fill.Length); j++)
                data[i + j] = _defaultFillByte;

        }


        public void AssigneLengthFrom(PlcString s)
        {
            if(s != null) _size.Bytes = s.StringLength;
        }

    }
}
