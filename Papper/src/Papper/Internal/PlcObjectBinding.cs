using Papper.Types;
using System;

namespace Papper.Internal
{
    internal class PlcObjectBinding
    {
        private readonly PlcRawData _rawData;
        private readonly PlcObject _metaData;

        public PlcObjectBinding(PlcRawData rawData, PlcObject metaData, int offset, int validationTimeMs, bool fullType = false)
        {
            _rawData = rawData;
            _metaData = metaData;
            Offset = offset;
            ValidationTimeInMs = validationTimeMs;
            FullType = fullType;
        }

        public bool IsActive { get; set; }
        public int ValidationTimeInMs { get; set; }

        public bool FullType { get; private set; }

        public byte[] Data
        {
            get { return _rawData.ReadDataCache; }
        }

        public DateTime LastUpdate
        {
            get { return _rawData.LastUpdate; }
        }

        public PlcRawData RawData
        {
            get { return _rawData; }
        }

        public PlcObject MetaData
        {
            get { return _metaData; }
        }

        public int Offset { get; private set; }
        public int Size { get { return _metaData.Size.Bytes; } }

        public object ConvertFromRaw(Span<byte> data)
        {
            return _metaData.ConvertFromRaw(this, data);
        }

        public void ConvertToRaw(object obj, Span<byte> data)
        {
            _metaData.ConvertToRaw(obj, this, data);
        }

        public Type GetMetaType()
        {
            return _metaData.GetType();
        }
    }
}
