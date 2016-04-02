using System;
using System.Collections.Generic;
using System.Linq;
using Papper.Interfaces;
using Papper.Types;

namespace Papper.Common
{
    internal class PlcMetaDataTreePath : ITreePath
    {
        public const string Separator = ".";
        private static readonly string[] SplitSeparator = { Separator };
        private readonly List<string> _nodes = new List<string>();

        static public PlcMetaDataTreePath CreateAbsolutePath(params string[] nodeNames)
        {
            return CreatePath((new List<string> { Separator }).Concat(nodeNames).ToArray());
        }

        static public PlcMetaDataTreePath CreatePath(params string[] nodeNames)
        {
            if (nodeNames.Skip(1).Any(node => node.IndexOf(Separator, StringComparison.Ordinal) >= 0)
                || (nodeNames[0].Length > 1 && nodeNames[0].IndexOf(Separator, StringComparison.Ordinal) > 0))
                throw new ArgumentException("Path: Node collection must not contain a separator!");
            return new PlcMetaDataTreePath(nodeNames);
        }

        static public PlcMetaDataTreePath CreateNodePath(ITreePath path, PlcObject plcObject)
        {
            return path.Extend(plcObject.Name) as PlcMetaDataTreePath;
        }

        public PlcMetaDataTreePath(string path)
        {
            path = Normalize(path);

            if (!string.IsNullOrWhiteSpace(path) && path.StartsWith(Separator))
            {
                path = path.Substring(1);
                _nodes.Add(Separator);
            }

            if (!string.IsNullOrWhiteSpace(path))
                _nodes.AddRange(path.Split(SplitSeparator, StringSplitOptions.None));
        }

        private PlcMetaDataTreePath(IEnumerable<string> nodes)
        {
            _nodes.AddRange(nodes);
        }

        private static string Normalize(string path)
        {
            path = path.Trim();
            if (path.Length > 0 && path.EndsWith(Separator))
            {
                path = path.Substring(path.Length - 1);
            }
            return path;
        }

        public string Path
        {
            get
            {
                return IsRelative
                    ? string.Join(Separator, _nodes)
                    : _nodes[0] + string.Join(Separator, _nodes.Skip(1).ToArray());
            }
        }

        public IEnumerable<string> Nodes
        {
            get { return _nodes; }
        }

        public bool IsPathToRoot
        {
            get { return _nodes.Count() == 1 && _nodes[0] == Separator; }
        }

        public bool IsPathToCurrent
        {
            get { return !_nodes.Any(); }
        }

        public bool IsPathIndexed
        {
            get
            {
                var node = _nodes.FirstOrDefault();
                return !string.IsNullOrEmpty(node) && node.EndsWith("]");
            }
        }

        public string ArrayName
        {
            get
            {
                var node = _nodes.FirstOrDefault();
                if (node != null)
                {
                    return node.Replace(']', '[').Split(new string[] { "[" }, StringSplitOptions.RemoveEmptyEntries).First();
                }
                return string.Empty;
            }
        }

        public int[] ArrayIndizes
        {
            get
            {
                var node = _nodes.FirstOrDefault();
                if (node != null)
                {
                    var v = node.Replace(']', '[').Split(new string[] { "[" }, StringSplitOptions.RemoveEmptyEntries);

                    if (!node.StartsWith("["))
                        return v.Skip(1).ToArray().Select(int.Parse).ToArray();
                    return v.ToArray().Select(int.Parse).ToArray();
                    
                }
                return new int[0];
            }
        }

        public bool IsAbsolute
        {
            get { return !IsRelative; }
        }

        public bool IsRelative
        {
            get { return !_nodes.Any() || _nodes[0] != Separator; }
        }

        public ITreePath StepDown()
        {
            if (!_nodes.Any())
                throw new Exception("Path: Cannot step down in empty path!");
            return new PlcMetaDataTreePath(_nodes.Skip(1));
        }

        public ITreePath Extend(string child)
        {
            var extendedNodes = new List<string>(_nodes) { child };
            return new PlcMetaDataTreePath(extendedNodes);
        }
    }
}
