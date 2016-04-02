using System.Linq;
using Papper.Interfaces;

namespace Papper.Common
{
    internal class PlcMetaDataTreeRootNode : PlcMetaDataTreeNode
    {
        public ITree Tree { get; private set; }
        public PlcMetaDataTreeRootNode(ITree tree, string name)
            : base(name)
        {
            Tree = tree;
        }

        public void ClearCaches()
        {
            foreach (var c in Childs.OfType<PlcMetaDataTreeNode>())
                c.ClearCache();
        }
    }
}
