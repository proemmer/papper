using System.Collections.Generic;
using Papper.Types;

namespace Papper.Internal
{
    internal delegate bool VisitNode(ITreeNode node);

    internal interface ITreeNode
    {
        string Name { get; }
        ITreeNode Root { get; }
        ITreeNode? Parent { get; }
        IEnumerable<ITreeNode> Childs { get; }
        PlcObject? Data { get; set; }
        void AddChild(ITreeNode child);
        void AddChild(ITreePath path, ITreeNode child);
        ITreeNode RemoveChild(string name);
        ITreeNode? Get(ITreePath path);
        ITreeNode? Get(ITreePath path, ref int offset, bool getRef = false);
        ITreePath GetPath();
        void Accept(VisitNode visit);
        void ReverseAccept(VisitNode visit);
    }
}
