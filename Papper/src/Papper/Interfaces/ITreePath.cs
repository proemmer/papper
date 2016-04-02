using System.Collections.Generic;

namespace Papper.Interfaces
{
    internal interface ITreePath
    {
        string Path { get; }
        IEnumerable<string> Nodes { get; }
        bool IsPathToRoot { get; }
        bool IsPathToCurrent { get; }
        bool IsAbsolute { get; }
        bool IsRelative { get; }
        bool IsPathIndexed { get; }
        ITreePath StepDown();
        ITreePath Extend(string child);
        string ArrayName { get; }
        int[] ArrayIndizes { get; }
    }
}
