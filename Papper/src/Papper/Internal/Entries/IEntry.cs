using Papper.Types;
using System;
using System.Collections.Generic;

namespace Papper.Internal
{
    internal interface IEntry
    {
        int ReadDataBlockSize { get; }
        int ValidationTimeMs { get; set; }
        PlcObject PlcObject { get; }
        IDictionary<string, OperationItem> Variables { get; }
        IEnumerable<Execution> GetOperations(IEnumerable<string> vars);
    }
}
