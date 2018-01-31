﻿using System;
using System.Collections.Generic;
using System.Linq;
using Papper.Interfaces;

namespace Papper.Common
{
    internal class PlcMetaDataTreeNode : PlcMetaDataBaseTreeNode
    {
        private readonly List<ITreeNode> _childs = new List<ITreeNode>();
        private Dictionary<string, ITreeNode> _childByNameCache = new Dictionary<string, ITreeNode>();


        public PlcMetaDataTreeNode(string name)
            : base(name)
        {
        }

        public override IEnumerable<ITreeNode> Childs
        {
            get { return _childs; }
        }

        public override void AddChild(ITreeNode child)
        {
            if (GetChildByName(child.Name) != null)
                throw new ArgumentException(string.Format("TreeNode: A child with name {0} already exists!", child.Name));
            if (child is PlcMetaDataBaseTreeNode baseTreeNode) baseTreeNode.Parent = this;
            _childs.Add(child);
        }

        public override ITreeNode RemoveChild(string name)
        {
            var child = GetChildByName(name);
            if (child == null)
                throw new ArgumentException(string.Format("TreeNode: A child with name {0} does not exist!", name));
            _childs.Remove(child);
            if (child is PlcMetaDataBaseTreeNode baseTreeNode) baseTreeNode.Parent = null;
            return child;
        }

        public virtual ITreeNode GetChildByName(string name)
        {
            ITreeNode tn;
            if (_childByNameCache == null)
                _childByNameCache = new Dictionary<string, ITreeNode>();
            else if (_childByNameCache.TryGetValue(name, out tn))
                return tn;

            tn = _childs.Find(node => node.Name == name);

            if (tn != null)
                _childByNameCache[name] = tn;
            return tn;
        }

        public override void Accept(VisitNode visit)
        {
            if (visit(this))
            {
                foreach (var childNode in _childs)
                    childNode.Accept(visit);
            }
        }

        public override void ReverseAccept(VisitNode visit)
        {
            if (visit(this) && Parent != null)
                Parent.ReverseAccept(visit);
        }

        public override ITreeNode Get(ITreePath path)
        {
            int dummy = 0;
            return Get(path, ref dummy);
        }

        public override ITreeNode Get(ITreePath path, ref int offset, bool getRef = false)
        {
            if (path.IsPathToCurrent)
                return this;

            if (path.IsAbsolute)
            {
                offset = 0;
                return Root.Get(path.StepDown(), ref offset, getRef);
            }
            if (path.IsPathIndexed)
            {
                var arrayChild = GetChildByName(path.ArrayName);
                return arrayChild?.Get(path, ref offset, getRef);
            }
            var child = GetChildByName(path.Nodes.First());
            return child?.Get(path.StepDown(), ref offset, getRef);
        }

        public override void AddChild(ITreePath path, ITreeNode node)
        {
            if (path.IsPathToCurrent)
                AddChild(node);
            else if (path.IsAbsolute)
                Root.AddChild(path.StepDown(), node);
            else
            {
                var nextNode = path.Nodes.First();
                var child = GetChildByName(nextNode);
                if (child == null)
                {
                    AddChild(new PlcMetaDataTreeNode(nextNode));
                    child = GetChildByName(nextNode);
                }
                child.AddChild(path.StepDown(), node);
            }
        }

        public virtual void ClearCache()
        {
            foreach (var c in Childs.OfType<PlcMetaDataTreeNode>())
                c.ClearCache();

            if (_childByNameCache != null)
                _childByNameCache.Clear();
        }
    }

}