using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Papper.Attributes;

namespace Papper.Types
{
    internal static class PlcObjectFactory
    {
        private static readonly Dictionary<Type, Type> TypeMatch = new Dictionary<Type, Type>
        {
            {typeof (bool), typeof (PlcBool)},
            {typeof (byte), typeof (PlcByte)},
            {typeof (sbyte), typeof (PlcSInt)},
            {typeof (short), typeof (PlcInt)},
            {typeof (int), typeof (PlcDInt)},
            {typeof (long), typeof (PlcLInt)},
            {typeof (ushort), typeof (PlcWord)},
            {typeof (uint), typeof (PlcDWord)},
            {typeof (ulong), typeof (PlcLWord)},
            {typeof (DateTime), typeof (PlcDateTime)},
            {typeof (TimeSpan), typeof (PlcTime)},
            {typeof (string), typeof (PlcString)},
            {typeof (float), typeof (PlcReal)},
            {typeof (char), typeof (PlcChar)},
        };

        private static readonly Dictionary<Type, Type> ReverseTypeMatch = TypeMatch.ToDictionary(x => x.Value, x => x.Key);

        private static readonly Dictionary<string, Type> TypeNameMatch = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase)
        {
            {"S5Time", typeof (PlcS5Time)},
            {"TimeOfDay", typeof (PlcTimeOfDay)},
            {"Bit", typeof (PlcBool)},
            {"Byte", typeof (PlcByte)},
            {"SInt", typeof (PlcSInt)},
            {"USInt", typeof (PlcUDInt)},
            {"Int", typeof (PlcInt)},
            {"DInt", typeof (PlcDInt)},
            {"UDInt", typeof (PlcUDInt)},
            {"LInt", typeof (PlcLInt)},
            {"ULInt", typeof (PlcULInt)},
            {"Word", typeof (PlcWord)},
            {"DWord", typeof (PlcDWord)},
            {"LWord", typeof (PlcLWord)},
            {"DateTime", typeof (PlcDateTime)},
            {"LDateTime", typeof (PlcLDateTime)},
            {"Time", typeof (PlcTime)},
            {"LTime", typeof (PlcLTime)},
            {"String", typeof (PlcString)},
            {"WString", typeof (PlcWString)},
            {"Real", typeof (PlcReal)},
            {"Float", typeof (PlcReal)},
            {"Char", typeof (PlcChar)},
            {"WChar", typeof (PlcWChar)},
            {"S7Counter", typeof (PlcS7Counter)},
        };

        public static PlcObject CreatePlcObject(PropertyInfo pi, int? arrayIndex = null)
        {
            var plcType = GetTypeFromAttribute(pi);
            PlcObject instance;
            PlcObject leafPlcObject = null;
            var name = GetName(pi);

            if (pi.PropertyType.IsArray && arrayIndex == null)
            {
                PlcObject element;
                var elementType = pi.PropertyType.GetElementType();
                var dimensions = 0;


                if (plcType != null || TypeMatch.TryGetValue(elementType, out plcType))
                {
                    element = Activator.CreateInstance(plcType, name) as PlcObject;
                    UpdateSize(pi, element);
                }
                else if (elementType.IsArray)
                {
                    while (elementType.IsArray)
                    {
                        elementType = elementType.GetElementType();
                        dimensions++;
                    }

                    if (TypeMatch.TryGetValue(elementType, out plcType))
                    {
                        element = Activator.CreateInstance(plcType, name) as PlcObject;
                        UpdateSize(pi, element);
                    }
                    else
                        element = Activator.CreateInstance(typeof(PlcStruct), name, pi.PropertyType) as PlcObject;

                    leafPlcObject = element;
                    for (var i = dimensions; i > 0; i--)
                    {
                        element = Activator.CreateInstance(typeof(PlcArray), string.Empty, element, 0, 0) as PlcObject;
                        UpdateSize(pi, element,i);
                    }
                }
                else
                    element = Activator.CreateInstance(typeof (PlcStruct), name, pi.PropertyType) as PlcObject;

                instance = Activator.CreateInstance(typeof(PlcArray), name, element, 0, 0) as PlcObject;
                if (instance != null)
                {
                    if (leafPlcObject != null)
                    {
                        if (instance is PlcArray plcArray)
                            plcArray.LeafElementType = leafPlcObject;
                    }

                    instance.ElemenType = elementType;
                    UpdateSize(pi, instance);
                }
            }
            else
            {
                if (plcType != null || TypeMatch.TryGetValue(arrayIndex == null ? pi.PropertyType : pi.PropertyType.GetElementType(), out plcType))
                {
                    instance = Activator.CreateInstance(plcType, arrayIndex == null ? name : name + string.Format("[{0}]", arrayIndex)) as PlcObject;
                    UpdateSize(pi, instance);
                }
                else
                    instance = Activator.CreateInstance(typeof(PlcStruct), arrayIndex == null ? name : name + string.Format("[{0}]", arrayIndex), pi.PropertyType) as PlcObject;

                if (instance != null)
                    instance.ElemenType = pi.PropertyType;
            }

            UpdateReadOnlyPoperty(pi, instance);
            return instance;
        }

        public static PlcObject CreatePlcObjectForArrayIndex(PlcObject obj, int? arrayIndex, int from)
        {
            var plcType = obj.GetType();
            var arrayElement = obj as PlcArray;
            var plcObject = arrayElement != null  ?
                Activator.CreateInstance(plcType, arrayIndex == null ? obj.Name : obj.Name + string.Format("[{0}]", arrayIndex), arrayElement.ArrayType, arrayElement.From, arrayElement.To) as PlcObject :
                Activator.CreateInstance(plcType, arrayIndex == null ? obj.Name : obj.Name + string.Format("[{0}]", arrayIndex)) as PlcObject;

            if (plcObject != null && arrayIndex != null)
            {
                (plcObject as PlcBool)?.AssigneOffsetFrom(((int)arrayIndex - @from) * obj.Size.Bits);
                (plcObject as PlcString)?.AssigneLengthFrom(obj as PlcString);
            }
            return plcObject;
        }

        private static void UpdateReadOnlyPoperty(MemberInfo pi, PlcObject plcObject)
        {
            var readOnlyAttribute = pi.GetCustomAttributes<ReadOnlyAttribute>().FirstOrDefault();
            if (readOnlyAttribute != null)
            {
                plcObject.IsReadOnly = readOnlyAttribute.IsReadOnly;
            }
        }

        private static void UpdateSize(MemberInfo pi, PlcObject plcObject, int dimension = 0)
        {
            if (plcObject is PlcString)
            {
                var stringLength = pi.GetCustomAttributes<StringLengthAttribute>().FirstOrDefault();
                if (stringLength != null)
                    (plcObject as PlcString).StringLength = stringLength.MaximumLength;
            }
            else if (plcObject is PlcArray)
            {
                var bounds = pi.GetCustomAttributes<ArrayBoundsAttribute>().OfType<ArrayBoundsAttribute>().FirstOrDefault(x => x.Dimension == dimension);
                if (bounds != null)
                {
                    var obj = (plcObject as PlcArray);
                    obj.From = bounds.From;
                    obj.To = bounds.To;
                    obj.Dimension = bounds.Dimension;
                }
            }
        }

        private static string GetName(MemberInfo pi)
        {
            var aliasAttribute = pi.GetCustomAttributes<AliasNameAttribute>().FirstOrDefault();
            if (aliasAttribute != null)
            {
                return aliasAttribute.Name;
            }
            return pi.Name;
        }

        public static bool GetOffsetFromAttribute(MemberInfo pi, ref int byteOffset, ref int bitOffset)
        {
            var mappingOffsets = pi.GetCustomAttributes<MappingOffsetAttribute>().FirstOrDefault();
            if (mappingOffsets != null)
            {
                var off =  mappingOffsets.ByteOffset;
                if (byteOffset != off)
                {
                    byteOffset = off;
                    bitOffset = 0;
                }

                off = mappingOffsets.BitOffset;
                if (off != -1)
                    bitOffset = off;
                return true;
            }
            return false;
        }

        private static Type GetTypeFromAttribute(MemberInfo pi)
        {
            var attribute = pi.GetCustomAttributes<PlcTypeAttribute>().FirstOrDefault();
            if (attribute != null)
            {
                if (TypeNameMatch.TryGetValue(attribute.Name, out Type plcType))
                    return plcType;
            }
            return null;
        }

        public static Type GetTypeForPlcObject(Type plcObjectType)
        {
            return ReverseTypeMatch.TryGetValue(plcObjectType, out Type retType) ? retType : typeof(object);
        }
    }
}
