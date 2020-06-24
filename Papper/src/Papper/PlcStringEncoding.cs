using System;
using System.Collections.Generic;
using System.Text;

namespace Papper
{
    public static class PlcSettings
    {
        public static Encoding StringEncoding { get; set; } = Encoding.GetEncoding(1250);
    }
}
