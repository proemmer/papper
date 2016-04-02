using Papper.Attributes;
using Papper.Common;
using Papper.Helper;
using Papper.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Papper
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class PlcDataMapper
    {
        public delegate bool ReadOperation(string selector, int offset, int length, byte[] data, int targetOffset = 0);
        public delegate bool WriteOperation(string selector, int offset, int length, byte[] data);
        public delegate bool WriteBitsOperation(string selector, int offset, byte[] data, byte mask = 0);

        private const int PduSizeDefault = 480;
        private const int ReadDataHeaderLength = 18;
        private readonly PlcMetaDataTree _tree = new PlcMetaDataTree();
        private readonly IDictionary<string, MappingEntry> _mappings = new Dictionary<string, MappingEntry>();

        private class MappingEntry
        {
            private readonly HashSet<PlcRawData> _data = new HashSet<PlcRawData>();
            private readonly IDictionary<string, PlcObjectBinding> _binding = new Dictionary<string, PlcObjectBinding>();

            public int ReadDataBlockSize { get; private set; }
            public MappingAttribute Mapping { get; private set; }
            public Type Type { get; private set; }
            public PlcObject PlcObject { get; private set; }
            public Dictionary<string, Tuple<int, PlcObject>> Variables { get; private set; }
            public MappingEntry(MappingAttribute mapping, Type type, PlcMetaDataTree tree, int readDataBlockSize)
            {
                if (mapping == null)
                    throw new ArgumentNullException("mapping");
                if (type == null)
                    throw new ArgumentNullException("type");
                if (tree == null)
                    throw new ArgumentNullException("tree");

                Mapping = mapping;
                ReadDataBlockSize = readDataBlockSize;
                Variables = new Dictionary<string, Tuple<int, PlcObject>>();
                PlcObject = PlcObjectResolver.GetMapping(mapping.Name, tree, type);
            }

            public IEnumerable<KeyValuePair<string,PlcObjectBinding>> GetOperations(string[] vars)
            {
                if(PlcObjectResolver.AddPlcObjects(PlcObject, Variables, vars))
                {
                    foreach (var rawDataBlock in PlcObjectResolver.CreateRawReadOperations(PlcObject.Selector, Variables, ReadDataBlockSize))
                    {
                        rawDataBlock.Data = new byte[CalcRawDataSize(rawDataBlock.Size > 0 ? rawDataBlock.Size : 1)];
                        _data.Add(rawDataBlock);
                        foreach (var reference in rawDataBlock.References)
                            _binding.Add(reference.Key, new PlcObjectBinding(rawDataBlock, reference.Value.Item2, reference.Value.Item1, Mapping.ObservationRate));
                    }
                }
                return _binding.Where(binding => vars.Contains(binding.Key));
            }

            private int CalcRawDataSize(int size)
            {
                size = size > 0 ? size : 2;
                if (size % 2 != 0)
                    size++;
                return size;
            }
        }

        public int ReadDataBlockSize { get; private set; }

        public event ReadOperation OnRead;
        public event WriteOperation OnWrite;
        public event WriteBitsOperation OnWriteBits;


        public PlcDataMapper(int pduSize = PduSizeDefault)
        {
            ReadDataBlockSize = pduSize - ReadDataHeaderLength;
            PlcMetaDataTreePath.CreateAbsolutePath(PlcObjectResolver.RootNodeName);
        }

        public bool AddMapping(Type type)
        {
            if (type == null)
                throw new ArgumentException("type");

            foreach (var mapping in type.GetTypeInfo().GetCustomAttributes<MappingAttribute>())
            {
                if (string.IsNullOrWhiteSpace(mapping.Name))
                    return false;

                MappingEntry existingMapping;
                if (_mappings.TryGetValue(mapping.Name, out existingMapping))
                    return existingMapping.Mapping == mapping && existingMapping.Type == type;
                _mappings.Add(mapping.Name, new MappingEntry(mapping, type, _tree, ReadDataBlockSize));
            }
            return true;
        }

        public Dictionary<string, object> Read(string mapping, params string[] vars)
        {
            MappingEntry entry;
            var result = new Dictionary<string, object>();
            if(_mappings.TryGetValue(mapping, out entry))
            {
                foreach (var bindingData in entry.GetOperations(vars))
                {
                    lock (bindingData.Value.RawData)
                    {
                        if (TryRead(bindingData.Value))
                            result.Add(bindingData.Key, bindingData.Value.ConvertFromRaw());
                    }
                }
            }
            return result;
        }

        public bool Write(string mapping, Dictionary<string,object> values)
        {
            MappingEntry entry;
            var error = false;
            if (_mappings.TryGetValue(mapping, out entry))
            {
                var operations = entry.GetOperations(values.Keys.ToArray());
                foreach (var kvp in values)
                {
                    var binding = operations.FirstOrDefault(b => b.Key == kvp.Key);
                    if (binding.Key != null)
                    {
                        if (!binding.Value.MetaData.IsReadOnly)
                        {
                            lock (binding.Value.RawData)
                            {
                                binding.Value.ConvertToRaw(kvp.Value);
                                if (!Write(binding.Value))
                                    error = true;
                            }
                        }
                    }
                }
            }
            return error;
        }

        #region external communication
        private bool TryRead(PlcObjectBinding binding)
        {
            var partiton = binding.RawData.GetPartitonsByOffset(binding.Offset, binding.Size);
            if (partiton != null)
            {
                if (partiton.Min(x => x.LastUpdate).AddMilliseconds(binding.ValidationTimeInMs) < DateTime.Now)
                    return ReadPartitions(binding, partiton);
            }
            else if (binding.LastUpdate.AddMilliseconds(binding.ValidationTimeInMs) < DateTime.Now)
                return Read(binding);
            return false;
        }

        private bool Read(PlcObjectBinding binding)
        {
            lock (binding.RawData)
            {
                var size = binding.RawData.Size > 0 ? binding.RawData.Size : 1;
                if (OnRead(binding.RawData.Selector, binding.RawData.Offset, size, binding.RawData.Data))
                {
                    binding.RawData.LastUpdate = DateTime.Now;
                    return true;
                }
                return false;
            }
        }

        private bool ReadPartitions(PlcObjectBinding binding, IList<Partiton> partitons)
        {
            lock (binding.RawData)
            {
                var startPartition = partitons.First();
                var offset = binding.RawData.Offset + startPartition.Offset;
                var readSize = partitons.Sum(x => x.Size);
                var targetOffset = startPartition.Offset;

                if (OnRead(binding.RawData.Selector, offset, readSize, binding.RawData.Data, targetOffset))
                {
                    foreach (var partiton in partitons)
                        partiton.LastUpdate = DateTime.Now;
                    return true;
                }
                return false;
            }
        }

        private bool Write(PlcObjectBinding binding)
        {
            lock (binding.RawData)
            {
                var rawData = binding.RawData;
                if (binding.Size != 0)
                {
                    var offset = rawData.Offset + binding.Offset;
                    var size = binding.Size;
                    var data = rawData.Data.Skip(binding.Offset).Take(binding.Size).ToArray();
                    return OnWrite(rawData.Selector, offset, size, data);
                }

                var startOffset = ((rawData.Offset + binding.Offset) * 8);
                var bitData = rawData.Data.Skip(binding.Offset).Take(1).ToArray();
                return OnWriteBits(rawData.Selector, startOffset, bitData, Converter.SetBit(0, binding.MetaData.Offset.Bits, true));
            }
        }
        #endregion

    }
}
