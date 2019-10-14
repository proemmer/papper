using System;

namespace Papper.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class MappingOffsetAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MappingOffsetAttribute()
        {
            ByteOffset = 0;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="byteOffset">The offset in byte calculate from the mapping offset.</param>
        /// <param name="bitOffset">The offset in bit from the current byte offset (only 0-7  or -1 allowed).</param>
        public MappingOffsetAttribute(int byteOffset, int bitOffset = -1)
        {
            ByteOffset = byteOffset;
            BitOffset = bitOffset;
        }

        /// <summary>
        /// The offset in byte calculate from the mapping offset.
        /// </summary>
        public int ByteOffset
        {
            get;
            private set;
        }

        /// <summary>
        /// The offset in bit from the current byte offset (only 0-7  or -1 allowed).
        /// </summary>
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