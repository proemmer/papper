using Papper.Internal;
using System;
using System.Text;

namespace Papper.Types
{
    internal class PlcString : PlcObject, ISupportStringLengthAttribute
    {
        private const int _defaultStringLength = 255;
        private const char _defaultFillChar = '\0';
        private readonly byte _defaultFillByte;
        private readonly PlcSize _size = new PlcSize();

        public override Type DotNetType => typeof(string);

        public int StringLength
        {
            get => _size.Bytes;
            set => _size.Bytes = value + 2;
        }

        public override PlcSize? Size
        {
            get => _size;
            protected set {; }
        }


        public PlcString(string name)
            : base(name)
        {
            _defaultFillByte = Convert.ToByte(_defaultFillChar);
            StringLength = _defaultStringLength;
        }

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            if (data.IsEmpty)
            {
                return string.Empty;
            }

            var maxLength = data[plcObjectBinding.Offset];
            var curLength = data[plcObjectBinding.Offset + 1];
            var take = Math.Min(Math.Min(maxLength, curLength), Size!.Bytes - 2);
            return Encoding.UTF7.GetString(data.ToArray(), plcObjectBinding.Offset + 2, take);
        }

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            var maxLength = Convert.ToByte(Size!.Bytes - 2);
            var i = plcObjectBinding.Offset;
            data[i++] = maxLength;
            var fill = string.Empty;
            if (value != null)
            {
                var str = value.ToString();
                var curLength = Convert.ToByte(str.Length);
                var take = Math.Min(maxLength, curLength);
                fill = str.Substring(0, take);

                data[i++] = take;
                foreach (var c in fill)
                {
                    data[i++] = Convert.ToByte(c);
                }
            }
            else
            {
                data[i++] = 0;  //currentLength
            }

            for (var j = 0; j < (maxLength - fill.Length); j++)
            {
                data[i + j] = _defaultFillByte;
            }
        }


        public void AssigneLengthFrom(ISupportStringLengthAttribute s)
        {
            if (s != null)
            {
                _size.Bytes = s.StringLength;
            }
        }

    }
}
