using Papper.Attributes;
using Papper.Common;
using Papper.Helper;
using Papper.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Papper
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class PlcDataMapper
    {
        public delegate byte[] ReadOperation(string selector, int offset, int length);
        public delegate bool WriteOperation(string selector, int offset, int length, byte[] data);
        public delegate bool WriteBitsOperation(string selector, int offset, byte[] data, byte mask = 0);

        private const int PduSizeDefault = 480;
        private const int ReadDataHeaderLength = 18;
        private readonly PlcMetaDataTree _tree = new PlcMetaDataTree();
        private readonly IDictionary<string, MappingEntry> _mappings = new Dictionary<string, MappingEntry>();


        private class Execution
        {
            public PlcRawData PlcRawData { get; private set; }
            public IEnumerable<Partiton> Partitions { get; private set; }
            public Dictionary<string, PlcObjectBinding> Bindings { get; private set; }
            public int ValidationTimeMs { get; private set; }

            public Execution(PlcRawData plcRawData, Dictionary<string, PlcObjectBinding> bindings, int validationTimeMS)
            {
                ValidationTimeMs = validationTimeMS;
                PlcRawData = plcRawData;
                Bindings = bindings;
                Partitions = plcRawData.GetPartitonsByOffset(bindings.Values.Select(x => new Tuple<int, int>(x.Offset, x.Size)));
            }
        }

        private class MappingEntry
        {
            private IDictionary<string, PlcObjectBinding> _bindings;
            private readonly ReaderWriterLockSlim _bindingLock = new ReaderWriterLockSlim();

            public int ReadDataBlockSize { get; private set; }
            public int ValidationTimeMs { get; set; }
            public MappingAttribute Mapping { get; private set; }
            public Type Type { get; private set; }
            public PlcObject PlcObject { get; private set; }
            public Dictionary<string, Tuple<int, PlcObject>> Variables { get; private set; }

            public MappingEntry(MappingAttribute mapping, Type type, PlcMetaDataTree tree, int readDataBlockSize, int validationTimeInMs)
            {
                if (mapping == null)
                    throw new ArgumentNullException("mapping");
                if (type == null)
                    throw new ArgumentNullException("type");
                if (tree == null)
                    throw new ArgumentNullException("tree");

                Mapping = mapping;
                ReadDataBlockSize = readDataBlockSize;
                ValidationTimeMs = validationTimeInMs;
                Variables = new Dictionary<string, Tuple<int, PlcObject>>();
                PlcObject = PlcObjectResolver.GetMapping(mapping.Name, tree, type);
            }

            ~MappingEntry()
            {
                _bindingLock.Dispose();
            }

            public IEnumerable<Execution> GetOperations(string[] vars)
            {
                UpdateInternalState(vars);
                return CreateExecutions(vars);
            }

            private void UpdateInternalState(string[] vars)
            {
                if (PlcObjectResolver.AddPlcObjects(PlcObject, Variables, vars))
                {
                    foreach (var rawDataBlock in PlcObjectResolver.CreateRawReadOperations(PlcObject.Selector, Variables, ReadDataBlockSize))
                    {
                        if (rawDataBlock.References.Any())
                        {
                            if (rawDataBlock.Data == null || rawDataBlock.Size > rawDataBlock.Data.Length)
                            {
                                var current = rawDataBlock.Data != null ? rawDataBlock.Data.Length : 0;
                                Debug.WriteLine($"{rawDataBlock.Data != null}, needed size:{rawDataBlock.Size}, current size:{current}");
                                lock (rawDataBlock)
                                    rawDataBlock.Data = new byte[CalcRawDataSize(rawDataBlock.Size > 0 ? rawDataBlock.Size : 1)];
                            }

                            var bindings = new Dictionary<string, PlcObjectBinding>();
                            foreach (var reference in rawDataBlock.References)
                                bindings.Add(reference.Key, new PlcObjectBinding(rawDataBlock, reference.Value.Item2, reference.Value.Item1, Mapping.ObservationRate));

                            _bindingLock.EnterWriteLock();
                            try
                            {
                                _bindings = bindings;
                            }
                            finally
                            {
                                _bindingLock.ExitWriteLock();
                            }
                        }
                    }
                }
            }

            private IEnumerable<Execution> CreateExecutions(string[] vars)
            {
                var result = new Dictionary<PlcRawData, Dictionary<string, PlcObjectBinding>>();
                IEnumerable<KeyValuePair<string, PlcObjectBinding>> bindingSnapshot = null;
                _bindingLock.EnterReadLock();
                try
                {
                    bindingSnapshot = _bindings.Where(binding => vars.Contains(binding.Key));
                }
                finally
                {
                    _bindingLock.ExitReadLock();
                }

                foreach (var binding in bindingSnapshot)
                {
                    Dictionary<string, PlcObjectBinding> entry;
                    if (!result.TryGetValue(binding.Value.RawData, out entry))
                    {
                        entry = new Dictionary<string, PlcObjectBinding>();
                        result.Add(binding.Value.RawData, entry);
                    }
                    entry.Add(binding.Key, binding.Value);
                }
                return result.Select(res => new Execution(res.Key, res.Value, ValidationTimeMs));
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
                _mappings.Add(mapping.Name, new MappingEntry(mapping, type, _tree, ReadDataBlockSize, mapping.ObservationRate));
            }
            return true;
        }

        public Dictionary<string, object> Read(string mapping, params string[] vars)
        {
            MappingEntry entry;
            var result = new Dictionary<string, object>();
            if(_mappings.TryGetValue(mapping, out entry))
            {
                foreach (var execution in entry.GetOperations(vars))
                {
                    if (ExecuteRead(execution))
                    {
                        foreach (var item in execution.Bindings)
                            result.Add(item.Key, item.Value.ConvertFromRaw());
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
                foreach (var execution in entry.GetOperations(values.Keys.ToArray()))
                {
                    foreach (var binding in execution.Bindings)
                    {
                        if (!binding.Value.MetaData.IsReadOnly)
                        {
                            lock (binding.Value.RawData)
                            {
                                binding.Value.ConvertToRaw(values[binding.Key]);
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
        private bool ExecuteRead(Execution exec)
        {
            if (exec.Partitions != null)
            {
                if (exec.Partitions.Min(x => x.LastUpdate).AddMilliseconds(exec.ValidationTimeMs) < DateTime.Now)
                    return ReadPartitions(exec.PlcRawData, exec.Partitions);
            }
            else if (exec.PlcRawData.LastUpdate.AddMilliseconds(exec.ValidationTimeMs) < DateTime.Now)
                return Read(exec.PlcRawData);
            return false;
        }

        private bool Read(PlcRawData rawData)
        {
            lock (rawData)
            {
                var size = rawData.Size > 0 ? rawData.Size : 1;
                if (Read(rawData.Selector, rawData.Offset, size, rawData.Data))
                {
                    rawData.LastUpdate = DateTime.Now;
                    return true;
                }
                return false;
            }
        }

        private bool ReadPartitions(PlcRawData rawData, IEnumerable<Partiton> partitons)
        {
            lock (rawData)
            {
                //var startPartition = partitons.First();
                //var offset = rawData.Offset + startPartition.Offset;
                //var readSize = partitons.Sum(x => x.Size);
                //var targetOffset = startPartition.Offset;

                //if (Read(rawData.Selector, offset, readSize, rawData.Data, targetOffset))
                //{
                //    foreach (var partiton in partitons)
                //        partiton.LastUpdate = DateTime.Now;
                //    return true;
                //}
                try
                {
                    foreach (var partition in partitons)
                    {
                        var offset = rawData.Offset + partition.Offset;
                        if (Read(rawData.Selector, offset, partition.Size, rawData.Data, partition.Offset))
                        {
                            foreach (var partiton in partitons)
                                partiton.LastUpdate = DateTime.Now;
                        }
                    }
                    return true;
                }
                catch(Exception)
                {

                }
                return true;
            }
        }

        private bool Read(string selector, int offset, int length, byte[] data, int targetOffset = 0)
        {
            try
            {
                byte[] red = OnRead(selector,  offset, length);
                if (red != null)
                    red.CopyTo(data, targetOffset);
                else
                    return false;
            }
            catch(Exception)
            {
                return false;
            }
            return true;
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
                    var data = rawData.Data.SubArray(binding.Offset, size);
                    return OnWrite(rawData.Selector, offset, size, data);
                }

                var startOffset = ((rawData.Offset + binding.Offset) * 8);
                var bitData = rawData.Data.SubArray(binding.Offset,1);
                return OnWriteBits(rawData.Selector, startOffset, bitData, Converter.SetBit(0, binding.MetaData.Offset.Bits, true));
            }
        }


        #endregion

    }
}
