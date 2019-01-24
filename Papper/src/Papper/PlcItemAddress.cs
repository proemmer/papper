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


        public string RawAddress<T>() => $"{Selector}.{GetTypeSign<T>()}{Offset.Bytes},{Size.Bytes}";

        private string GetTypeSign<T>()
        {

            var t = default(T);
            switch (t)
            {
                case bool b: return "X";
                case byte by: return "B";
                case char c: return "C";
                case int i: return "DI";
                case uint i: return "DW";
                case short i: return "I";
                case ushort i: return "W";
                case Single si: return "R";
                case string s: return "S";
            }

            return "B";
        }
    }
}
