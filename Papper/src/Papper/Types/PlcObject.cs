using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Papper.Internal;

namespace Papper.Types
{
    internal abstract class PlcObject : PlcMetaDataTreeNode, IPlcObject
    {
        private const string RootNodeName = "Plc";
        private readonly PlcSize _offset = new PlcSize();
        public string Selector { get; set; }
        public Type ElemenType { get; set; }
        public bool AllowOddByteOffsetInArray { get; set; }

        public PlcSize Offset
        {
            get
            {
                return _offset;
            }
        }

        public bool IsReadOnly { get; internal set; }

        public virtual int ByteOffset { get { return Offset.Bytes; } }
        public virtual int BitOffset { get { return Offset.Bits; } }
        public virtual int ByteSize { get { return Size.Bytes; } }
        public virtual int BitSize { get { return Size.Bits; } }

        public IEnumerable<IPlcObject> ChildVars { get { return Childs.OfType<IPlcObject>(); } }

        public virtual PlcSize Size { get; protected set; }

        public static PlcObject AddPlcObjectToTree(PlcObject obj, ITree tree, ITreePath path)
        {
            var offset = obj.Offset.Bytes;
            var node = PlcMetaDataTreePath.CreateNodePath(path, obj);
            var metaDataNode = tree.Get(node) as PlcObject;
            if (metaDataNode == null)
            {
                lock (tree.Root)
                {
                    metaDataNode = tree.Get(node) as PlcObject;
                    if (metaDataNode == null)
                        tree.Root.AddChild(path, obj);
                    else
                        return metaDataNode;
                }
                return obj;
            }
            return metaDataNode;
        }

        protected PlcObject(string name)
            : base(name)
        {
        }

        public override string ToString()
        {
            return GetType().ToString();
        }


        public abstract object ConvertFromRaw(PlcObjectBinding plcObjectBinding, byte[] data);
        public abstract void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding, byte[] data);

        public virtual object StringToObject(string value)
        {
            try
            {
                var type = PlcObjectFactory.GetTypeForPlcObject(GetType());
                return Convert.ChangeType(value, type);
            }
            catch
            { }
            return null;
        }

        public virtual bool AreDataEqual(object obj1, object obj2)
        {
            var t1 = obj1.GetType();
            var t2 = obj2.GetType();

            if (t1 == t2){ return t1 != typeof (ExpandoObject) ? ElementEqual(obj1, obj2) : DynamicObjectCompare(obj1, obj2); }
            try{ return ElementEqual(obj1, Convert.ChangeType(obj2, t1));} catch{ }
            return false;
        }

        private bool ElementEqual(object obj1, object obj2)
        {
            var list1 = obj1 as IEnumerable;
            var list2 = obj2 as IEnumerable;

            if (list1 != null && list2 != null)
            {
                var enumerator1 = list1.GetEnumerator();
                var enumerator2 = list2.GetEnumerator();
                while (true)
                {
                    var e1 = enumerator1.MoveNext();
                    var e2 = enumerator2.MoveNext();
                    if (e1 && e2)
                    {
                        if (!AreDataEqual(enumerator1.Current, enumerator2.Current))
                            return false;
                    }
                    else
                        return e1 == e2; //Length not the same?
                }
            }
            return obj1.Equals(obj2);
        }

        private bool DynamicObjectCompare(object obj1, object obj2)
        {
            var dictionary1 = obj1 as IDictionary<string, object>;
            var dictionary2 = obj2 as IDictionary<string, object>;

            if (dictionary1 != null && dictionary2 != null)
            {
                foreach (var o1 in dictionary1)
                {
                    if (!dictionary2.TryGetValue(o1.Key, out object o2) || !AreDataEqual(o1.Value, o2))
                        return false;

                }
            }
            return true;
        }

    }
}
