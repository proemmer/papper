using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;

namespace Papper.Internal
{
    internal class PlcMetaDataTreeNode : PlcMetaDataBaseTreeNode
    {
        private static readonly List<ITreeNode> _empty = new();
        private List<ITreeNode>? _childs;
        private ConcurrentDictionary<string, ITreeNode>? _childByNameCache;


        public PlcMetaDataTreeNode(string name)
            : base(name)
        {
        }

        public override IEnumerable<ITreeNode> Childs => _childs ?? _empty;

        public override void AddChild(ITreeNode child)
        {
            if (GetChildByName(child.Name) != null)
            {
                ExceptionThrowHelper.ThrowChildNodeException(child.Name, true);
            }

            if (child is PlcMetaDataBaseTreeNode baseTreeNode)
            {
                baseTreeNode.Parent = this;
            }

            if (_childs == null)
            {
                _childs = new List<ITreeNode>();
            }

            _childs.Add(child);
        }

        public override ITreeNode RemoveChild(string name)
        {
            var child = GetChildByName(name);
            if (child == null)
            {
                ExceptionThrowHelper.ThrowChildNodeException(name, false);
            }

            _childs?.Remove(child!);
            if (child is PlcMetaDataBaseTreeNode baseTreeNode)
            {
                baseTreeNode.Parent = null;
            }

            return child!;
        }

        public virtual ITreeNode? GetChildByName(string name)
        {
            ITreeNode? tn;
            if (_childByNameCache == null)
            {
                _childByNameCache = new ConcurrentDictionary<string, ITreeNode>();
            }
            else if (_childByNameCache.TryGetValue(name, out tn))
            {
                return tn;
            }

            tn = _childs?.Find(node => node.Name == name);

            if (tn != null)
            {
                _childByNameCache[name] = tn;
            }

            return tn;
        }

        public override void Accept(VisitNode visit)
        {
            if (visit(this))
            {
                if (_childs != null)
                {
                    foreach (var childNode in _childs)
                    {
                        childNode.Accept(visit);
                    }
                }
            }
        }

        public override void ReverseAccept(VisitNode visit)
        {
            if (visit(this) && Parent != null)
            {
                Parent.ReverseAccept(visit);
            }
        }

        public override ITreeNode? Get(ITreePath path)
        {
            var dummy = 0;
            var sb = new StringBuilder();
            return Get(path, ref dummy, ref sb);
        }

        public override ITreeNode? Get(ITreePath path, ref int offset, ref StringBuilder symbolicPath, bool getRef = false)
        {
            if (path.IsPathToCurrent)
            {
                return this;
            }

            if (path.IsAbsolute)
            {
                offset = 0;
                return Root.Get(path.StepDown(), ref offset, ref symbolicPath, getRef);
            }
            if (path.IsPathIndexed)
            {
                var arrayChild = GetChildByName(path.ArrayName);
                var result = arrayChild?.Get(path, ref offset, ref symbolicPath, getRef);
                return result;
            }
            var child = GetChildByName(path.Nodes.First());
            if (child != null)
            {
                symbolicPath.Append(".");
                symbolicPath.Append(child.SymbolicAccessName);
            }
            return child?.Get(path.StepDown(), ref offset, ref symbolicPath, getRef);
        }

        public override void AddChild(ITreePath path, ITreeNode node)
        {
            if (path.IsPathToCurrent)
            {
                AddChild(node);
            }
            else if (path.IsAbsolute)
            {
                Root.AddChild(path.StepDown(), node);
            }
            else
            {
                var nextNode = path.Nodes.First();
                var child = GetChildByName(nextNode);
                if (child == null)
                {
                    AddChild(new PlcMetaDataTreeNode(nextNode));
                    child = GetChildByName(nextNode);
                }
                child?.AddChild(path.StepDown(), node);
            }
        }

        public virtual void ClearCache()
        {
            foreach (var c in Childs.OfType<PlcMetaDataTreeNode>())
            {
                c.ClearCache();
            }

            if (_childByNameCache != null)
            {
                _childByNameCache.Clear();
            }
        }
    }

}
