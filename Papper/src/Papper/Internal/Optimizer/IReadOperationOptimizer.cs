using Papper.Types;
using System.Collections.Generic;

namespace Papper.Internal
{

    internal class OperationItem
    {
        public OperationItem(int offset, string symbolicPath, PlcObject plcObject)
        {
            Offset = offset;
            SymbolicPath = symbolicPath;
            PlcObject = plcObject;
        }

        public int Offset { get; private set; }
        public string SymbolicPath { get; private set; }
        public PlcObject PlcObject { get; private set; }
    }


    internal interface IReadOperationOptimizer
    {
        bool SymbolicAccess { get; }
        IEnumerable<PlcRawData> CreateRawReadOperations(string name, string selector, IEnumerable<KeyValuePair<string, OperationItem>> objects, int readDataBlockSize);
    }
}