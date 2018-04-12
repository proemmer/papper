using System.Collections.Generic;

namespace Papper.Internal
{
    internal interface IPlcObject
    {
        string Name { get; }
        string Selector { get; }
        int ByteOffset { get; }
        int BitOffset { get; }
        int ByteSize { get; }
        int BitSize { get; }
        bool IsReadOnly { get; }

        IEnumerable<IPlcObject> ChildVars { get; } 
    }
}
