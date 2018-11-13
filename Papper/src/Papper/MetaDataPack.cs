using System;
using System.Collections.Generic;
using System.Text;

namespace Papper
{
    public class MetaDataPack
    {

        public string MappingName { get; set; }
        public string AbsoluteName { get; set; }
        public MetaData MetaData { get; set; }


        public ExecutionResult ExecutionResult { get; set; }
    }
}
