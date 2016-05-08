using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Papper
{
    public class PlcItemAddress
    {
        public string Selector { get; private set; }

        public Type Type { get; private set; }

        public PlcSize Offset { get; private set; }

        public PlcSize Size { get; private set; }

        public PlcItemAddress(string selector, Type type, PlcSize offset, PlcSize size)
        {
            Selector = selector;
            Type = type;
            Offset = offset;
            Size = size;
        }
    }
}
