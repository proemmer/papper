using Papper.Types;
using System;

namespace Papper.Internal
{
    internal class PlcObjectBinding
    {
        public PlcObjectBinding(PlcRawData rawData, PlcObject metaData, int offset, int validationTimeMs, bool fullType = false)
        {
            RawData = rawData;
            MetaData = metaData;
            Offset = offset;
            ValidationTimeInMs = validationTimeMs;
            FullType = fullType;
        }

        public bool IsActive { get; set; }
        public int ValidationTimeInMs { get; set; }

        public bool FullType { get; private set; }

        public object? Data => RawData.ReadDataCache;

        public PlcRawData RawData { get; }

        public PlcObject MetaData { get; }

        public int Offset { get; private set; }
        public int Size => MetaData.Size == null ? 0 : MetaData.Size.Bytes;

        public object ConvertFromRaw(Span<byte> data) => MetaData.ConvertFromRaw(this, data);

        public void ConvertToRaw(object obj, Span<byte> data) => MetaData.ConvertToRaw(obj, this, data);

        public Type GetMetaType() => MetaData.GetType();
    }
}
