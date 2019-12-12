using System;

namespace Papper
{
    /// <summary>
    /// Represent a plc address
    /// </summary>
    public struct PlcItemAddress
    {
        /// <summary>
        /// Selector is e.g.  DB100,  FB,...
        /// </summary>
        public string Selector { get; private set; }

        /// <summary>
        /// .Net type of the item
        /// </summary>
        public Type Type { get; private set; }

        /// <summary>
        /// Byte and bit offset to the item.
        /// </summary>
        public PlcSize Offset { get; private set; }

        /// <summary>
        /// Size of the item
        /// </summary>
        public PlcSize Size { get; private set; }

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="type"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        public PlcItemAddress(string selector, Type type, PlcSize offset, PlcSize size)
        {
            Selector = selector;
            Type = type;
            Offset = offset;
            Size = size;
        }


        public string RawAddress<T>()
        {
            var sign = GetTypeSign<T>();
            if (sign == "X")
            {
                return $"{Selector}.{sign}{Offset.Bytes}.{Offset.Bits},{Size.Bytes}";
            }
            return $"{Selector}.{sign}{Offset.Bytes},{Size.Bytes}";
        }

        private string GetTypeSign<T>()
        {
            var t = default(T)!;
            return t switch
            {
                bool _ => "X",
                byte _ => "B",
                char _ => "C",
                int _ => "DI",
                uint _ => "DW",
                short _ => "I",
                ushort _ => "W",
                float _ => "R",
                string _ => "S",

                _ => "B",
            };
        }
    }
}
