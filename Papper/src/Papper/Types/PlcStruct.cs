using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Papper.Internal;
using System;

namespace Papper.Types
{
    internal class PlcStruct : PlcObject
    {
        public const int AlignmentInBytes = 2;
        private readonly Type _structType;

        public override Type DotNetType => _structType;

        public override PlcSize Size
        {
            get
            {
                int byteOffset = 0;
                if (Childs.Any())
                {
                    var first = Childs.OfType<PlcObject>().FirstOrDefault();
                    var last = Childs.OfType<PlcObject>().LastOrDefault();

                    if (first != last)
                        byteOffset = (last.Offset.Bytes + (last.Size.Bytes == 0 ? 1 : last.Size.Bytes)) - first.Offset.Bytes;
                    else
                        byteOffset = (first.Size.Bytes == 0 ? 1 : first.Size.Bytes);
                }

                return new PlcSize
                {
                    Bytes = ((byteOffset + AlignmentInBytes - 1) / AlignmentInBytes) * AlignmentInBytes
                };
            }
        }


        public PlcStruct(string name, Type structType) 
            : base(name)
        {
            _structType = structType ?? ExceptionThrowHelper.ThrowArgumentNullException<Type>(nameof(structType));
        }

        public override object ConvertFromRaw(PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            if (!plcObjectBinding.FullType || _structType == null)
            {
                var obj = new ExpandoObject();
                foreach (var child in plcObjectBinding.MetaData.Childs.OfType<PlcObject>().AsParallel())
                {
                    var binding = new PlcObjectBinding(plcObjectBinding.RawData, child, plcObjectBinding.Offset + child.Offset.Bytes, plcObjectBinding.ValidationTimeInMs);
                    AddProperty(obj, child.Name, child.ConvertFromRaw(binding, data));
                }
                return obj;
            }
            else
            {
                var obj = Activator.CreateInstance(_structType);
                foreach (var child in plcObjectBinding.MetaData.Childs.OfType<PlcObject>().AsParallel())
                {
                    var prop = _structType.GetProperty(child.Name);
                    var binding = new PlcObjectBinding(plcObjectBinding.RawData, child, plcObjectBinding.Offset + child.Offset.Bytes, plcObjectBinding.ValidationTimeInMs, true);
                    prop.SetValue(obj, child.ConvertFromRaw(binding, data));
                }
                return obj;
            }
        }

        public override void ConvertToRaw(object value, PlcObjectBinding plcObjectBinding, Span<byte> data)
        {
            if (value != null)
            {
                var properties = GetKeyValuePairs(value);
                foreach (var child in plcObjectBinding.MetaData.Childs.OfType<PlcObject>())
                {
                    var binding = new PlcObjectBinding(plcObjectBinding.RawData, child, plcObjectBinding.Offset + child.Offset.Bytes, plcObjectBinding.ValidationTimeInMs);
                    if (properties.TryGetValue(child.Name, out object prop))
                        child.ConvertToRaw(prop, binding, data);
                }
            }
        }

        private static void AddProperty(dynamic parent, string name, object value)
        {
            var list = (parent as List<dynamic>);
            if (list != null)
            {
                list.Add(value);
            }
            else
            {
                var dictionary = parent as IDictionary<string, object>;
                if (dictionary != null)
                    dictionary[name] = value;
            }
        }

        private static IDictionary<string, object> GetKeyValuePairs(object value)
        {
            //var dyn = value as DynamicPlcObject;
            //if (dyn != null)
            //    return dyn.ToDictionary();
            var dictionary = value as IDictionary<string, object>;
            return dictionary ?? value.GetType().GetProperties().ToDictionary(x => x.Name, x => x.GetValue(value));
        }
    }
}
