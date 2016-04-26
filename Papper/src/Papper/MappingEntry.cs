using Papper.Attributes;
using Papper.Common;
using Papper.Helper;
using Papper.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Papper
{
    internal class MappingEntry
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
                var bindings = new Dictionary<string, PlcObjectBinding>();
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

                        foreach (var reference in rawDataBlock.References)
                            bindings.Add(reference.Key, new PlcObjectBinding(rawDataBlock, reference.Value.Item2, reference.Value.Item1, Mapping.ObservationRate));
                    }
                }

                if (bindings.Any())
                {
                    _bindingLock.EnterWriteLock();
                    try
                    {
                        //extend bindings with the new created ones
                        _bindings = _bindings != null
                                        ? _bindings.Union(bindings
                                            .Where(kvp => !_bindings.ContainsKey(kvp.Key))
                                            ).ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
                                        : bindings;
                    }
                    finally
                    {
                        _bindingLock.ExitWriteLock();
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
                bindingSnapshot = _bindings.Where(binding => vars.Contains(binding.Key)).ToList();
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
}
