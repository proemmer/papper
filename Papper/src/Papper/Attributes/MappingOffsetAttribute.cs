using System;

namespace Papper.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MappingOffsetAttribute : Attribute
    {
        public MappingOffsetAttribute()
        {
            ByteOffset = 0;
        }

        public MappingOffsetAttribute(int byteOffset, int bitOffset = -1)
        {
            ByteOffset = byteOffset;
            BitOffset = bitOffset;
        }

        public int ByteOffset
        {
            get;
            private set;
        }

        public int BitOffset
        {
            get;
            private set;
        }

        public override int GetHashCode()
        {
            return string.Format("{0}.{1}", ByteOffset, BitOffset).GetHashCode();
        }
    }
}