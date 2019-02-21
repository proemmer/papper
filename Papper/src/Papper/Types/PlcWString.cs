using System;
using System.Buffers.Binary;
using System.Text;
using Papper.Internal;

namespace Papper.Types
{
    internal class PlcWString : PlcObject
    {
        private const int DefaultStringLength = 254;
        private const char DefaultFillChar = '\0';
        private readonly byte _defaultFillByte;
        private readonly PlcSize _size = new PlcSize();

        public override Type DotNetType => typeof(string);


        public int StringLength
        {
            get { return _size.Bytes; }
            set { _size.Bytes = (value * 2) + 4; }
        }

        public override PlcSize Size
        {
            get { return _size; }
            protected set { ; } 
        }


        public PlcWString(string name) 
            : base(name)
        {
            _defaultFillByte = Convert.ToByte(DefaultFillChar);
            StringLength = DefaultStringLength;
        }

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            if (data.IsEmpty)
                return string.Empty;

            var maxLength = BinaryPrimitives.ReadInt16BigEndian(data.Slice(plcObjectBinding.Offset));
            var curLength = BinaryPrimitives.ReadInt16BigEndian(data.Slice(plcObjectBinding.Offset + 2));
            var take = Math.Min(Math.Min(maxLength, curLength), Size.Bytes - 4) * 2;
            return Encoding.Unicode.GetString(data.Slice(plcObjectBinding.Offset + 4, take).ToArray());
        }

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            var maxLength = Convert.ToInt16((Size.Bytes / 2) - 2);
            var i = plcObjectBinding.Offset;
            BinaryPrimitives.TryWriteInt16BigEndian(data.Slice(i, 2), maxLength);
            i += 2;

            var fill = string.Empty;
            if (value != null)
            {
                var str = value.ToString();
                var curLength = Convert.ToInt16(str.Length);
                var take = (short)(Math.Min(maxLength, curLength));
                fill = str.Substring(0, take);

                BinaryPrimitives.TryWriteInt16BigEndian(data.Slice(i, 2), take);
                i += 2;


                foreach (var c in Encoding.Unicode.GetBytes(fill))
                    data[i++] = c;
            }
            else
            {
                BinaryPrimitives.TryWriteInt16BigEndian(data.Slice(i, 2), 0);  //currentLength
            }

            for (var j = 0; j < (maxLength - fill.Length) * 2; j++)
            {
                data[i + j] = _defaultFillByte;
            }

        }

    }
}
