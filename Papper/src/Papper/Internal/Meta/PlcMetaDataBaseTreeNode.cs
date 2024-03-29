﻿using Papper.Types;
using System.Collections.Generic;
using System.Text;

namespace Papper.Internal
{
    internal abstract class PlcMetaDataBaseTreeNode : ITreeNode
    {
        private readonly string _name;
        private string? _symbolicAccessName;
        protected string? _originName;
        private ITreePath? _savedPath;
        private ITreeNode? _parent;
        public PlcObject? Data { get; set; }

        protected PlcMetaDataBaseTreeNode(string name) => _name = name;

        public string Name => _name;

        public string SymbolicAccessName 
        { 
            get => _symbolicAccessName ?? Name; 
            set
            {
                if (value != _name)
                {
                    _symbolicAccessName = value;
                }
            }
        }

        public string OriginName
        {
            get => _originName ?? Name;
            set
            {
                if (value != _name)
                {
                    _originName = value;
                }
            }
        }

        public ITreeNode Root => (_parent == null) ? this : _parent.Root;

        public ITreeNode? Parent
        {
            get => _parent;
            internal set
            {
                if (_parent != null && value != null)
                {
                    ExceptionThrowHelper.ThrowAttemptToAssignNewParentException();
                }

                _parent = value;
                if (_parent == null)
                {
                    _savedPath = null;
                }
            }
        }

        public abstract IEnumerable<ITreeNode> Childs { get; }
        public abstract void AddChild(ITreeNode child);
        public abstract void AddChild(ITreePath path, ITreeNode child);
        public abstract ITreeNode RemoveChild(string name);
        public abstract ITreeNode? Get(ITreePath path);
        public abstract ITreeNode? Get(ITreePath path, ref int offset, ref StringBuilder symbolicPath, bool getRef = false);
        public abstract void Accept(VisitNode visit);
        public abstract void ReverseAccept(VisitNode visit);

        public override string ToString() => $"Name={Name}";

        public ITreePath GetPath()
        {
            if (_savedPath == null)
            {
                SetupPath();
            }
            return _savedPath!;
        }

        private void SetupPath()
        {
            var nodeNames = new List<string>();
            ITreeNode? upper = this;
            do
            {
                nodeNames.Insert(0, upper.Name);
                upper = upper.Parent;
            } while (upper != null);
            _savedPath = PlcMetaDataTreePath.CreatePath(nodeNames);
        }
    }
}
