using System.Text;

namespace Papper.Internal
{
    internal interface ITree
    {
        ITreeNode Root { get; }
        ITreeNode? Get(ITreePath path);
        ITreeNode? Get(ITreePath path, ref int offset, ref StringBuilder symbolicPath, bool getRef = false);
        bool TryGet(ITreePath path, out ITreeNode node);
        bool TryGet(ITreePath path, ref int offset, ref StringBuilder symbolicPath, out ITreeNode node, bool getRef = false);
    }
}
