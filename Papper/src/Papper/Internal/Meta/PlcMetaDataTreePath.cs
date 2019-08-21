using Papper.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Papper.Internal
{
    internal class PlcMetaDataTreePath : ITreePath
    {
        public const string Separator = ".";
        private static readonly string[] _splitSeparator = { Separator };
        private readonly List<string> _nodes = new List<string>();
        private int[] _arrayIndizes;

        public static PlcMetaDataTreePath CreateAbsolutePath(params string[] nodeNames) => CreatePath((new List<string> { Separator }).Concat(nodeNames));

        public static PlcMetaDataTreePath CreateAbsolutePath(IEnumerable<string> nodeNames) => CreatePath((new List<string> { Separator }).Concat(nodeNames));

        public static PlcMetaDataTreePath CreatePath(params string[] nodeNames)
            => CreatePath(nodeNames as IEnumerable<string>);

        public static PlcMetaDataTreePath CreatePath(IEnumerable<string> nodeNames)
        {
            var first = nodeNames.FirstOrDefault();
            if (first == null ||
                nodeNames.Skip(1).Any(node => node.IndexOf(Separator, StringComparison.Ordinal) >= 0) || 
                (first.Length > 1 && first.IndexOf(Separator, StringComparison.Ordinal) > 0))
            {
                ExceptionThrowHelper.ThrowInvalidNodePathCollectionException();
            }

            return new PlcMetaDataTreePath(nodeNames);
        }

        public static PlcMetaDataTreePath CreateNodePath(ITreePath path, PlcObject plcObject) => path.Extend(plcObject.Name) as PlcMetaDataTreePath;

        public PlcMetaDataTreePath(string path)
        {
            path = Normalize(path);

            if (!string.IsNullOrWhiteSpace(path) && path.StartsWith(Separator))
            {
                path = path.Substring(1);
                _nodes.Add(Separator);
            }

            if (!string.IsNullOrWhiteSpace(path))
                _nodes.AddRange(path.Split(_splitSeparator, StringSplitOptions.None));
        }

        private PlcMetaDataTreePath(IEnumerable<string> nodes) => _nodes.AddRange(nodes);

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

        public IEnumerable<string> Nodes => _nodes;

        public bool IsPathToRoot => _nodes.Count == 1 && _nodes[0] == Separator;

        public bool IsPathToCurrent => !_nodes.Any();

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
                return node != null ? node.Split(new[] { "[", "]" }, StringSplitOptions.RemoveEmptyEntries)[0] : string.Empty;
            }
        }

        public int[] ArrayIndizes
        {
            get
            {
                if (_arrayIndizes != null)
                    return _arrayIndizes;

                var node = _nodes.FirstOrDefault();
                if (node != null)
                {
                    var v = node.Split(new[] { "[", "]" }, StringSplitOptions.RemoveEmptyEntries);
                    _arrayIndizes = !node.StartsWith("[") ? v.Skip(1).Select(int.Parse).ToArray() : v.Select(int.Parse).ToArray();
                }
                else
                {
                    _arrayIndizes = new int[0];
                }

                return _arrayIndizes;
            }
        }

        public bool IsAbsolute => !IsRelative;

        public bool IsRelative => !_nodes.Any() || _nodes[0] != Separator;

        public ITreePath StepDown()
        {
            if (!_nodes.Any()) ExceptionThrowHelper.ThrowEmptyNodePathCollectionException();
            return new PlcMetaDataTreePath(_nodes.Skip(1));
        }

        public ITreePath Extend(string child)
        {
            var extendedNodes = new List<string>(_nodes) { child };
            return new PlcMetaDataTreePath(extendedNodes);
        }
    }
}
