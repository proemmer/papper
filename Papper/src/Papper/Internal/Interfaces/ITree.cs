namespace Papper.Internal
{
    internal interface ITree
    {
        ITreeNode Root { get; }
        ITreeNode Get(ITreePath path);
        ITreeNode Get(ITreePath path, ref int offset, bool getRef = false);
        bool TryGet(ITreePath path, out ITreeNode node);
        bool TryGet(ITreePath path, ref int offset, out ITreeNode node, bool getRef = false);
    }
}
