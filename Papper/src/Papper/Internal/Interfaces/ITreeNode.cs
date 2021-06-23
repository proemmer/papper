using Papper.Types;
using System.Collections.Generic;
using System.Text;

namespace Papper.Internal
{
    internal delegate bool VisitNode(ITreeNode node);

    internal interface ITreeNode
    {
        string Name { get; }

        string SymbolicAccessName { get; }
        ITreeNode Root { get; }
        ITreeNode? Parent { get; }
        IEnumerable<ITreeNode> Childs { get; }
        PlcObject? Data { get; set; }
        void AddChild(ITreeNode child);
        void AddChild(ITreePath path, ITreeNode child);
        ITreeNode RemoveChild(string name);
        ITreeNode? Get(ITreePath path);
        ITreeNode? Get(ITreePath path, ref int offset, ref StringBuilder symbolicPath, bool getRef = false);
        ITreePath GetPath();
        void Accept(VisitNode visit);
        void ReverseAccept(VisitNode visit);
    }
}
