using System;
using System.Collections.Generic;
using System.Text;

namespace Papper
{
    public static class PlcEncoding
    {
        public static Encoding? Default { get; set; } 


        static PlcEncoding()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Default = Encoding.GetEncoding(1252);
        }
    }
}
