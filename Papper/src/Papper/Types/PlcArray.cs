using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Papper.Helper;
using Papper.Interfaces;
using Papper.Common;
using System.Text;

namespace Papper.Types
{
    internal class PlcArray : PlcObject
    {
        private readonly Dictionary<int, ITreeNode> _indexCache = new Dictionary<int, ITreeNode>(); 
        private readonly PlcSize _size = new PlcSize();
        private PlcObject _arrayType;
        private int _from;
        private int _to;
        public int Dimension { get; set; }

        public int From
        {
            get { return _from; }
            set
            {
                if (_from != value)
                {
                    _from = value;
                    if (_arrayType != null)
                    {
                        CalculateSize();
                    }
                }
            }
        }

        public int To
        {
            get { return _to; }
            set
            {
                if (_to != value)
                {
                    _to = value;
                    if (_arrayType != null)
                    {
                        CalculateSize();
                    }
                }
            }
        }

        public int ArrayLength
        {
            get { return To - From + 1; }
        }

        public PlcObject ArrayType
        {
            get { return _arrayType; }
            set
            {
                if (_arrayType != value)
                {
                    _arrayType = value;
                    CalculateSize();
                }
            }
        }

        public PlcObject LeafElementType { get; set; }

        public override PlcSize Size
        {
            get { return _size; }
        }

        public PlcArray(string name, PlcObject arrayType, int from = 0, int to = 0) 
            : base(name)
        {
            ArrayType = arrayType;
            From = from;
            To = to;
        }

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding)
        {
            if (ArrayType is PlcByte)
                return InternalConvert(plcObjectBinding, new byte());
            if (ArrayType is PlcString)
                return InternalConvert(plcObjectBinding, string.Empty);
            if (ArrayType is PlcInt)
                return InternalConvert(plcObjectBinding, new Int16());
            if (ArrayType is PlcBool)
                return InternalConvert(plcObjectBinding, new bool());
            if (ArrayType is PlcDInt)
                return InternalConvert(plcObjectBinding, new Int32());
            if (ArrayType is PlcWord)
                return InternalConvert(plcObjectBinding, new UInt16());
            if (ArrayType is PlcDWord)
                return InternalConvert(plcObjectBinding, new UInt32());
            if (ArrayType is PlcDateTime || ArrayType is PlcTimeOfDay)
                return InternalConvert(plcObjectBinding, new DateTime());
            if (ArrayType is PlcS5Time || ArrayType is PlcTime)
                return InternalConvert(plcObjectBinding, new TimeSpan());
            if (ArrayType is PlcReal)
                return InternalConvert(plcObjectBinding, new Single());
            if (ArrayType is PlcChar)
                return InternalConvert(plcObjectBinding, new char());
            if (ArrayType is PlcArray)
                return ArrayType.ConvertFromRaw(plcObjectBinding);
            return InternalConvert(plcObjectBinding, new ExpandoObject());
        }

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding)
        {
            var list = value as IEnumerable;
            var convert = list as string;

            //handle byte array for Json --- TODO:  not so beautiful here
            if (convert != null && (ArrayType is PlcByte || ArrayType.ElemenType == typeof(byte)))
                value = list = Convert.FromBase64String(convert);

            if (list != null)
            {
                //Special handling for byte and char, because of performance (specially with big data)
                if (ArrayType is PlcByte &&  value is byte[])
                {
                    var byteArray = value as byte[];
                    byteArray.CopyTo(plcObjectBinding.RawData.Data, plcObjectBinding.Offset);
                }
                else if (ArrayType is PlcChar && value is char[])
                {
                    var charArray = value as char[];
                    Encoding.ASCII.GetBytes(charArray).CopyTo(plcObjectBinding.RawData.Data, plcObjectBinding.Offset);
                }
                else
                {
                    var enumerator = list.GetEnumerator();
                    for (var i = 0; i < ArrayLength; i++)
                    {
                        if (enumerator.MoveNext())
                        {
                            var child = Childs.OfType<PlcObject>().Skip(i).FirstOrDefault();
                            if (child == null)
                                throw new Exception("Array error");
                            var binding = new PlcObjectBinding(plcObjectBinding.RawData, child, plcObjectBinding.Offset + child.Offset.Bytes + (child.Size.Bytes * i), plcObjectBinding.ValidationTimeInMs);
                            ArrayType.ConvertToRaw(enumerator.Current, binding);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This method converts the given binding data to the correct representation data type
        /// </summary>
        /// <typeparam name="T">Target type</typeparam>
        /// <param name="plcObjectBinding"></param>
        /// <param name="type">instance of target type</param>
        /// <returns></returns>
        private object InternalConvert<T>(PlcObjectBinding plcObjectBinding, T type)
        {
            
            if (plcObjectBinding.Data != null && plcObjectBinding.Data.Any())
            {
                //special handling of byte and char, because of performance (specially with big data)
                if (type is byte)
                {
                    return plcObjectBinding.RawData.Data.SubArray(plcObjectBinding.Offset, ArrayLength);
                }
                else if(type is char)
                {
                    return Encoding.ASCII.GetChars(plcObjectBinding.RawData.Data.SubArray(plcObjectBinding.Offset, ArrayLength));
                }
                else
                {
                    var list = new T[ArrayLength];
                    for (var i = 0; i < ArrayLength; i++)
                    {
                        var child = Childs.OfType<PlcObject>().Skip(i).FirstOrDefault();
                        if (child == null)
                            throw new Exception("Array error");
                        var binding = new PlcObjectBinding(plcObjectBinding.RawData, child, plcObjectBinding.Offset + child.Offset.Bytes + (child.Size.Bytes * i), plcObjectBinding.ValidationTimeInMs);
                        list[i] = ((T)ArrayType.ConvertFromRaw(binding));
                    }
                    return list;
                }
            }
            return new T[ArrayLength]; ;
        }


        public override bool AreDataEqual(object obj1, object obj2)
        {
            var list1 = obj1 as IEnumerable;
            var list2 = obj2 as IEnumerable;

            if (list1 != null && list2 != null)
            {
                var enumerator1 = list1.GetEnumerator();
                var enumerator2 = list2.GetEnumerator();
                for (var i = 0; i < ArrayLength; i++)
                {
                    if (enumerator1.MoveNext() && enumerator2.MoveNext())
                    {
                        if (!base.AreDataEqual(enumerator1.Current, enumerator2.Current))
                            return false;
                    }
                    else
                        break;
                }
            }
            else
                return base.AreDataEqual(obj1, obj2);
            return true;
        }

        public override IEnumerable<ITreeNode> Childs
        {
            get
            {
                for (var i = From; i <= To; i++)
                {
                    yield return GetIndex(i);
                }
            }
        }

        public override ITreeNode GetChildByName(string name)
        {
            return base.GetChildByName(name);
        }

        public override ITreeNode Get(ITreePath path, ref int offset, bool getRef = false)
        {
            if (path.IsPathToCurrent && !path.IsPathIndexed)
                return this;
            var idx = path.ArrayIndizes.First();
            if (idx >= From && idx <= To)
            {
                offset += Offset.Bytes + ((idx - From)*(LeafElementType ?? ArrayType).Size.Bytes);
                ITreeNode ret;
                return (_indexCache.TryGetValue(idx, out ret) ? 
                    ret :
                    GetIndex(idx)).Get(CreateSubPath(path), ref offset);
            }
            return null;
        }

        private static ITreePath CreateSubPath(ITreePath path)
        {
            var indizes = path.ArrayIndizes.Length;
            if (indizes > 1)
            {
                var nodes = new List<string>();
                var first = path.Nodes.First();
                nodes.Add(first.Substring(first.IndexOf(']') + 1));
                nodes.AddRange(path.Nodes.Skip(1));
                return new PlcMetaDataTreePath(nodes.Aggregate((a, b) => a + PlcMetaDataTreePath.Separator + b));
            }
            else
                return path.StepDown();
        }

        public override void Accept(VisitNode visit)
        {
            base.Accept(visit);
        }

        private void CalculateSize()
        {
            Size.Bits = _arrayType is PlcBool ? ArrayLength * _arrayType.Size.Bits : 0;
            Size.Bytes = _arrayType is PlcBool ? 0 : ArrayLength * _arrayType.Size.Bytes;

            if (_arrayType is PlcBool)
            {
                Size.Bytes = Size.Bits / 8;
                Size.Bits = Size.Bits - Size.Bytes * 8;
            }
        }

        private PlcObject GetIndex(int idx)
        {
            lock (_indexCache)
            {
                ITreeNode ret;
                if (_indexCache.TryGetValue(idx, out ret))
                    return ret as PlcObject;
                ret = ArrayType is PlcStruct
                    ? new PlcObjectRef(string.Format("[{0}]", idx), ArrayType)
                    : PlcObjectFactory.CreatePlcObjectForArrayIndex(ArrayType, idx, From);
                _indexCache.Add(idx, ret);
                return ret as PlcObject;
            }
        }

        public override object StringToObject(string value)
        {
            try
            {
                var type = PlcObjectFactory.GetTypeForPlcObject(ArrayType.GetType());
                if (type == typeof(char))
                {
                    return value.ToCharArray();
                }
                if (type == typeof(byte))
                {
                    return value.ToArray();
                }
                var parts = value.Split(';');
                return parts.Select(part => Convert.ChangeType(value, type)).ToList();
            }
            catch
            { }
            return null;
        }
    }
}
