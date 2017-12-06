using System;
using System.Collections.Generic;
using Papper.Internal;

namespace Papper.Types
{
    internal class PlcObjectRef : PlcObject
    {
        internal const string RefMarker = "\\";
        private readonly PlcObject _referencedObject;
        //private int _offset = 0;

        public override PlcSize Size
        {
            get { return _referencedObject.Size; }
        }

        public override int ByteOffset { get { return Offset.Bytes; } }
        public override int BitOffset { get { return Offset.Bits; } }
        public override int ByteSize { get { return Size.Bytes; } }
        public override int BitSize { get { return Size.Bits; } }

        public PlcObjectRef(string name, PlcObject reference) :
            base(name)
        {
            _referencedObject = reference;
        }

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding, byte[] data)
        {
            return _referencedObject.ConvertFromRaw(plcObjectBinding, data);
        }

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding, byte[] data)
        {
            _referencedObject.ConvertToRaw(value, plcObjectBinding, data);
        }

        public override IEnumerable<ITreeNode> Childs
        {
            get { return _referencedObject.Childs; }
        }

        public override void AddChild(ITreeNode child)
        {
            throw new NotSupportedException();
        }

        public override ITreeNode RemoveChild(string name)
        {
            throw new NotSupportedException();
        }

        public override void AddChild(ITreePath path, ITreeNode node)
        {
            throw new NotSupportedException();
        }

        public override ITreeNode GetChildByName(string name)
        {
            return _referencedObject.GetChildByName(name);
        }

        public override ITreeNode Get(ITreePath path, ref int offset, bool getRef = false)
        {
            //_offset = offset;
            offset += Offset.Bytes;
            if (path.IsPathToCurrent && getRef)
                return this;
            return _referencedObject.Get(path, ref offset);
        }

        public override void Accept(VisitNode visit)
        {
            _referencedObject.Accept(visit);
        }

        public override void ReverseAccept(VisitNode visit)
        {
            _referencedObject.ReverseAccept(visit);
        }

        public override void ClearCache()
        {
            _referencedObject.ClearCache();
        }

    }
}
