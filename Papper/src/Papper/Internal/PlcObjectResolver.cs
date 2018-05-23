﻿using Papper.Attributes;
using Papper.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Papper.Internal
{
    internal static class PlcObjectResolver
    {

        public const string RootNodeName = "Plc";
        public const string InstancesNodeName = "Instances";
        public const string MetaDataNodeName = "MetaData";

        /// <summary>
        /// Syntax:
        /// Selector= Area[DbNumber]
        /// 
        /// IB: Input Area
        /// FB: Flag Area
        /// QB: Output Area
        /// TM: Timer Area
        /// CT: Counter Area
        /// DB: DataBlock Area
        /// 
        /// W = Word; X = Bool;  B = Byte; I = Int; S = String;
        /// 
        /// [TYPE][OFFSET][_CountOfData]
        /// 
        /// Bool
        /// [TYPE][OFFSET_Bit][_CountOfData]
        /// 
        /// String
        /// [TYPE][OFFSET_Stringlength][_CountOfData]
        /// </summary>
        /// <param name="plcObj"></param>
        /// <param name="plcObjects"></param>
        /// <param name="values"></param>
        internal static bool AddRawPlcObjects(ITreeNode plcObj, Dictionary<string, Tuple<int, PlcObject>> plcObjects, IEnumerable<string> values)
        {
            var updated = false;
            foreach (var value in values)
            {
                PlcObject plcObject = null;
                var dataType = new StringBuilder();
                foreach (var c in value.TakeWhile(char.IsLetter))
                {
                    dataType.Append(c);
                }

                switch (dataType.ToString())
                {
                    case "X":
                    case "BIT":
                        plcObject = new PlcBool(value);
                        break;
                    case "B":
                    case "BYTE":
                        plcObject = new PlcByte(value);
                        break;
                    case "C":
                    case "CHAR":
                        plcObject = new PlcChar(value);
                        break;
                    case "DATE":
                        plcObject = new PlcDate(value);
                        break;
                    case "DT":
                    case "DATETIME":
                        plcObject = new PlcDateTime(value);
                        break;
                    case "DI":
                    case "DINT":
                        plcObject = new PlcDInt(value);
                        break;
                    case "DW":
                    case "DWORD":
                        plcObject = new PlcDWord(value);
                        break;
                    case "I":
                    case "INT":
                        plcObject = new PlcInt(value);
                        break;
                    case "R":
                    case "REAL":
                        plcObject = new PlcReal(value);
                        break;
                    case "TIMEBCD":
                        plcObject = new PlcS5Time(value);
                        break;
                    case "S":
                    case "STRING":
                        plcObject = new PlcString(value);
                        break;
                    case "T":
                    case "TIME":
                        plcObject = new PlcTime(value);
                        break;
                    case "TOD":
                        plcObject = new PlcTimeOfDay(value);
                        break;
                    case "W":
                    case "WORD":
                        plcObject = new PlcWord(value);
                        break;
                    case "CT":
                    case "COUNT":
                        plcObject = new PlcS7Counter(value);
                        break;
                }

                if (plcObject != null)
                {
                    var parts = value.Substring(dataType.Length).Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
                    plcObject.Selector = plcObj.Name;
                    if (parts.Length >= 2)
                    {
                        if (plcObject is PlcBool)
                            plcObject.Offset.Bits = Int32.Parse(parts[1]);

                        if (plcObject is PlcString)
                            (plcObject as PlcString).StringLength = Int32.Parse(parts[1]);

                        if (!(plcObject is PlcBool) || parts.Length >= 3)
                        {
                            var length = Int32.Parse(parts.Last());
                            if (length > 0)
                                length--;
                            plcObject = new PlcArray(value, plcObject, 0, length);
                        }
                    }
                    plcObject.Offset.Bytes = Int32.Parse(parts[0]);
                    if (!plcObjects.ContainsKey(value))
                    {
                        plcObjects.Add(value, new Tuple<int, PlcObject>(0, plcObject));
                        updated = true;
                    }

                }
            }
            return updated;
        }

        /// <summary>
        /// Adds the meta data of requested variables to the dictionary 
        /// </summary>
        internal static bool AddPlcObjects(ITreeNode plcObj, Dictionary<string, Tuple<int, PlcObject>> plcObjects, IEnumerable<string> values, string prefix = "", int offset = 0)
        {
            var updated = false;
            foreach (var value in values.Where( x => !plcObjects.ContainsKey(x)))
            {
                var baseOffset = offset;
                var item = value == "This" ? plcObj as PlcObject : plcObj.Get(PlcMetaDataTreePath.CreatePath(value.Split('.')), ref baseOffset) as PlcObject;
                if (item == null)
                    continue;

                var key = prefix + value;

                if (!plcObjects.ContainsKey(key))
                {
                    plcObjects.Add(key, new Tuple<int, PlcObject>(baseOffset, item));
                    updated = true;
                }

                if (item is PlcStruct plcStruct)
                    updated = AddPlcObjects(plcStruct, plcObjects, plcStruct.Childs.Select(child => child.Name), key + ".", baseOffset) || updated;
            }
            return updated;
        }

        /// <summary>
        /// Get all leafs from the meta tree, normally this should b only value types.
        /// </summary>
        /// <param name="obj">TreeNode</param>
        /// <param name="path">Path to node</param>
        /// <returns>List of leafs</returns>
        public static IEnumerable<string> GetLeafs(ITreeNode obj, ICollection<string> path)
        {
            var list = new List<string>();
            if (obj != null)
            {
                var idx = obj.Name.IndexOf("[", StringComparison.Ordinal);

                if (idx >= 0)
                {
                    var p = path.Last();
                    path = new List<string>(path.Take(path.Count - 1)) { p + obj.Name.Substring(idx) };
                }
                else
                    path.Add(obj.Name);
                if (obj.Childs.Any())
                {
                    if (obj is PlcArray)
                    {
                        foreach (var child in obj.Childs)
                        {
                            if (child.Childs.Any())
                            {
                                var nunmberOfChilds = child.Childs.Count();
                                var internalPath = new List<string>(path.Take(path.Count - 1)) { obj.Name + child.Name };
                                foreach (var c in child.Childs)
                                    list.AddRange(GetLeafs(c, internalPath.ToList()));
                            }
                            else
                            {
                                var internalPath = new List<string>(path.Take(path.Count - 1)) { child.Name };
                                list.Add(PlcMetaDataTreePath.CreateAbsolutePath(internalPath.Skip(1).ToArray()).Path.Substring(1));
                            }
                        }
                    }
                    else
                    {
                        foreach (var child in obj.Childs)
                            list.AddRange(GetLeafs(child, path.ToList()));
                    }

                }
                else
                    list.Add(PlcMetaDataTreePath.CreateAbsolutePath(path.Skip(1).ToArray()).Path.Substring(1));
            }
            return list;
        }

        /// <summary>
        /// Try to get the mapping from MetaTree. If the data are not in the tree, try to create and add it. 
        /// </summary>
        internal static PlcObject GetMapping(string name, ITree tree, Type t, bool allowAddingWithoutMappingAttribute = false)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;
            var nodePathStack = new Stack<string>();
            nodePathStack.Push(RootNodeName);
            nodePathStack.Push(InstancesNodeName);
            foreach (var part in name.Split('.'))
                nodePathStack.Push(part);

            var path = PlcMetaDataTreePath.CreateAbsolutePath(nodePathStack.Reverse().ToArray());
            var offset = 0;
            if (!tree.TryGet(path, ref offset, out ITreeNode obj, true))
            {
                if (t == null)
                    throw new ArgumentNullException("t");

                var mapping = t.GetTypeInfo().GetCustomAttributes<MappingAttribute>().FirstOrDefault(m => m.Name == name);
                if (mapping != null)
                {
                    nodePathStack.Pop();
                    var plcObj = new PlcObjectRef(name, GetMetaData(tree, t))
                    {
                        Offset = { Bytes = mapping.Offset },
                        Selector = mapping.Selector
                    };
                    PlcObject.AddPlcObjectToTree(plcObj, tree, PlcMetaDataTreePath.CreateAbsolutePath(nodePathStack.Reverse().ToArray()));
                    obj = plcObj;
                }
                else if(allowAddingWithoutMappingAttribute)
                {
                    nodePathStack.Pop();
                    var plcObj = new PlcObjectRef(name, GetMetaData(tree, t));
                    PlcObject.AddPlcObjectToTree(plcObj, tree, PlcMetaDataTreePath.CreateAbsolutePath(nodePathStack.Reverse().ToArray()));
                    obj = plcObj;
                }
                else
                {
                    throw new ArgumentException("Given name is not declared!");
                }
            }
            return obj as PlcObject;
        }


        private static string NormalizeTypeName(string name)
        {
            var result = new StringBuilder();
            foreach (char c in name.Where(c => c != '.'))
            {
                result.Append(c);
            }
            return result.ToString();
        }

        /// <summary>
        /// Try to get Meta data from MetaTree. If the data are not in the tree, try to create and add it. 
        /// All the calculation of the offsets will be made in this method. the tree has two parts, one for MetaData and one for The Mappings.
        /// The mappings have only a Reference of the Meta data, so you shouldn't change the Offsets while using.
        /// </summary>
        private static PlcObject GetMetaData(ITree tree, Type t)
        {
            var nodePathStack = new Stack<string>();
            var name = NormalizeTypeName(t.FullName);
            nodePathStack.Push(RootNodeName);
            nodePathStack.Push(MetaDataNodeName);
            nodePathStack.Push(name);
            var path = PlcMetaDataTreePath.CreateAbsolutePath(nodePathStack.Reverse().ToArray());
            var offset = 0;
            if (!tree.TryGet(path, ref offset, out ITreeNode obj))
            {
                var byteOffset = 0;
                var bitOffset = 0;
                nodePathStack.Pop();
                var parent = new PlcStruct(name, t);
                PlcObject.AddPlcObjectToTree(parent, tree, PlcMetaDataTreePath.CreateAbsolutePath(nodePathStack.Reverse().ToArray()));
                nodePathStack.Push(parent.Name);
                PlcObject pred = null;
                DebugOutPut("{0}{{", name);
                
                foreach (var pi in t.GetTypeInfo().DeclaredProperties)
                {
                    var plcObject = PlcObjectFactory.CreatePlcObject(pi);

                    if (plcObject is PlcArray plcObjectArray && (plcObjectArray.LeafElementType ?? plcObjectArray.ArrayType) is PlcStruct)
                        plcObjectArray.ArrayType = GetMetaData(tree, plcObjectArray.ElemenType);
                    else if (plcObject is PlcStruct)
                        plcObject = new PlcObjectRef(plcObject.Name, GetMetaData(tree, pi.PropertyType));


                    var hasCustomOffset = PlcObjectFactory.GetOffsetFromAttribute(pi, ref byteOffset, ref bitOffset);
                    AddPlcObject(tree, pred, plcObject, nodePathStack, ref byteOffset, ref bitOffset, hasCustomOffset);
                    pred = plcObject;
                }
                DebugOutPut("}} = {0}", parent.Size.Bytes);
                nodePathStack.Pop();
                obj = parent;
            }
            return obj as PlcObject;
        }

        /// <summary>
        /// Calculates the Offset of the new Object, add it to the tree and update the offsets
        /// </summary>
        private static void AddPlcObject(ITree tree, PlcObject pred, PlcObject plcObject, IEnumerable<string> nodePathStack, ref int byteOffset, ref int bitOffset, bool hasCustomOffset)
        {
            if(!hasCustomOffset)
                CalculateOffset(pred, plcObject, ref byteOffset, ref bitOffset);
            plcObject.Offset.Bytes = byteOffset;
            plcObject.Offset.Bits = bitOffset;
            DebugOutPut("{0}: Offset:{1,3}.{2}    Size={3}.{4}", plcObject.Name.PadRight(20), byteOffset, bitOffset, plcObject.Size.Bytes, plcObject.Size.Bits);
            PlcObject.AddPlcObjectToTree(plcObject, tree, PlcMetaDataTreePath.CreateAbsolutePath(nodePathStack.Reverse().ToArray()));
            byteOffset += plcObject.Size.Bytes;
            bitOffset += plcObject.Size.Bits;
        }

        /// <summary>
        /// Calculate the real offsets in the PLC.
        /// </summary>
        internal static void CalculateOffset(PlcObject pred, PlcObject cur, ref int byteOffset, ref int bitOffset)
        {
            if (byteOffset % 2 == 0 && bitOffset == 0)
            {
                //No further checks needed
            }
            else
            {
                var prevIsBool = pred is PlcBool;
                var curIsBool = cur is PlcBool;
                var prevIsByte = pred is PlcByte || pred is PlcChar;
                var curIsByte = cur is PlcByte || cur is PlcChar;
                var prevIsArray = pred is PlcArray;
                var curIsArray = cur is PlcArray;

                if (prevIsBool && (!curIsBool || bitOffset > 7) ||
                    prevIsArray)
                {
                    byteOffset += 1;
                    bitOffset = 0;
                }

                if (byteOffset % 2 != 0)
                {
                    if (curIsArray ||
                        !((prevIsByte && curIsByte) ||
                          (prevIsByte && curIsBool) ||
                          (prevIsBool && curIsByte) ||
                          (prevIsBool && curIsBool)))
                    {
                        byteOffset += 1;
                        bitOffset = 0;
                    }
                }
            }
        }


        [Conditional("DEBUG")]
        private static void DebugOutPut(string format, params object[] attributes)
        {
            Debug.WriteLine(format, attributes);
        }
    }
}