using Papper.Types;
using System;
using System.Collections.Generic;

namespace Papper.Entries
{
    internal interface IEntry
    {
        int ReadDataBlockSize { get; }
        int ValidationTimeMs { get; set; }
        PlcObject PlcObject { get; }
        Dictionary<string, Tuple<int, PlcObject>> Variables { get; }
        event OnChangeEventHandler OnChange;

        bool SetActiveState(bool enable, string[] vars);
        IEnumerable<Execution> GetOperations(string[] vars);
    }
}
