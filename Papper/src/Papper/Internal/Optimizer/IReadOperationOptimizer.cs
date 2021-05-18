﻿using Papper.Types;
using System.Collections.Generic;

namespace Papper.Internal
{

    internal class OperationItem
    {
        public OperationItem(int offset, PlcObject plcObject)
        {
            Offset = offset;
            PlcObject = plcObject;
        }

        public int Offset { get; private set; }
        public PlcObject PlcObject { get; private set; }
    }


    internal interface IReadOperationOptimizer
    {
        bool SymbolicAccess { get; }
        IEnumerable<PlcRawData> CreateRawReadOperations(string name, string selector, IEnumerable<KeyValuePair<string, OperationItem>> objects, int readDataBlockSize);
    }
}