using Papper.Types;
using System;
using System.Collections.Generic;

namespace Papper.Internal
{
    internal interface IReadOperationOptimizer
    {
        IEnumerable<PlcRawData> CreateRawReadOperations(string selector, IEnumerable<KeyValuePair<string, Tuple<int, PlcObject>>> objects, int readDataBlockSize);
    }
}