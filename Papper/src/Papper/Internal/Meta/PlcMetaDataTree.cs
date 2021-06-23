using System.Text;

namespace Papper.Internal
{
    internal class PlcMetaDataTree : ITree
    {
        public ITreeNode Root { get; private set; }

        public PlcMetaDataTree() => Root = new PlcMetaDataTreeRootNode(this, PlcMetaDataTreePath.Separator);

        public ITreeNode? Get(ITreePath path) => Root.Get(path);

        public ITreeNode? Get(ITreePath path, ref int offset, ref StringBuilder symbolicPath, bool getRef = false) => Root.Get(path, ref offset, ref symbolicPath, getRef);

        public bool TryGet(ITreePath path, out ITreeNode node)
        {
            node = Root.Get(path)!;
            return node != null;
        }

        public bool TryGet(ITreePath path, ref int offset, ref StringBuilder symbolicPath, out ITreeNode node, bool getRef = false)
        {
            node = Root.Get(path, ref offset, ref symbolicPath, getRef)!;
            return node != null;
        }
    }
}
