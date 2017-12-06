using Papper.Internal;

namespace Papper.Internal
{
    internal class PlcMetaDataTree : ITree
    {
        public ITreeNode Root { get; private set; }

        public PlcMetaDataTree()
        {
            Root = new PlcMetaDataTreeRootNode(this, PlcMetaDataTreePath.Separator);
        }

        public ITreeNode Get(ITreePath path)
        {
            return Root.Get(path);
        }

        public ITreeNode Get(ITreePath path, ref int offset, bool getRef = false)
        {
            return Root.Get(path, ref offset, getRef);
        }

        public bool TryGet(ITreePath path,out ITreeNode node)
        {
            node = Root.Get(path);
            return node != null;
        }

        public bool TryGet(ITreePath path, ref int offset, out ITreeNode node, bool getRef = false)
        {
            node = Root.Get(path, ref offset, getRef);
            return node != null;
        }
    }
}
