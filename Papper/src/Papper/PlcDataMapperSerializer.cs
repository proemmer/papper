using Papper.Internal;
using Papper.Types;
using System;
using System.Collections.Generic;

namespace Papper
{
    public class PlcDataMapperSerializer
    {
        private readonly PlcMetaDataTree _tree = new PlcMetaDataTree();
        private readonly Dictionary<Type, SerializerMappingEntry> _entries = new Dictionary<Type, SerializerMappingEntry>();


        private class SerializerMappingEntry
        {
            public PlcObject PlcObject { get; private set; }
            public Dictionary<string, Tuple<int, PlcObject>> Variables { get; private set; }
            public Dictionary<string, PlcObjectBinding> Bindings { get; private set; }
            public PlcObjectBinding BaseBinding { get; private set; }

            public SerializerMappingEntry(PlcObject plcObject)
            {
                PlcObject = plcObject ?? throw new ArgumentNullException(nameof(plcObject));
                BaseBinding = new PlcObjectBinding(new PlcRawData(plcObject.ByteSize), plcObject, 0, 0, true);
                Variables = new Dictionary<string, Tuple<int, PlcObject>>();
                Bindings = new Dictionary<string, PlcObjectBinding>();
            }
        }

        /// <summary>
        /// Converts a data type to a plc known format.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] Serialize<T>(T data)
        {
            var binding = GetMappingEntryForType(typeof(T)).BaseBinding;
            var buffer = new byte[binding.RawData.MemoryAllocationSize];  // TODO handle a reusable buffer
            binding.ConvertToRaw(data, buffer);
            return buffer;
        }

        /// <summary>
        /// Converts a plc known format to a datatype
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public T Deserialize<T>(byte[] data)
        {
            var binding = GetMappingEntryForType(typeof(T)).BaseBinding;
            if (data.Length < binding.Size)
                throw new ArgumentOutOfRangeException($"{nameof(data)}");
            return (T)binding.ConvertFromRaw(data);
        }

        /// <summary>
        /// Get a value from the structs binary data
        /// </summary>
        /// <typeparam name="TStruct">The struct of the given byte array</typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="data"></param>
        /// <param name="variable"></param>
        /// <returns></returns>
        public TValue GetValue<TStruct, TValue>(string variable, Span<byte> data)
        {
            var mappingEntry = GetMappingEntryForType(typeof(TStruct));
            UpdateVariables(mappingEntry, variable);
            return mappingEntry.Bindings.TryGetValue(variable, out var binding) ? (TValue)binding.ConvertFromRaw( data) : default;
        }



        /// <summary>
        /// Returns the size in bytes of the given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public int SerializedByteSize<T>()
        {
            var t = typeof(T);
            var plcObject = PlcObjectResolver.GetMapping(t.Name, _tree, t, true);
            return plcObject.ByteSize;
        }


        private SerializerMappingEntry GetMappingEntryForType(Type type)
        {
            if (!_entries.TryGetValue(type, out var mappingEntry))
            {
                mappingEntry = new SerializerMappingEntry(PlcObjectResolver.GetMapping(type.Name, _tree, type, true));
                _entries.Add(type, mappingEntry);
            }
            return mappingEntry;
        }

        private bool UpdateVariables(SerializerMappingEntry mappingEntry, string variable)
        {
            if (PlcObjectResolver.AddPlcObjects(mappingEntry.PlcObject, mappingEntry.Variables, new List<string> { variable }))
            {
                if (mappingEntry.Variables.TryGetValue(variable, out var accessObject))
                {
                    mappingEntry.Bindings.Add(variable, new PlcObjectBinding(mappingEntry.BaseBinding.RawData, accessObject.Item2, accessObject.Item2.ByteOffset, 0));
                    return true;
                }
            }
            return false;
        }
    }
}
