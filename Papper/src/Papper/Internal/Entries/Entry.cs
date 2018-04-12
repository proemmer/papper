using Papper.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Papper.Internal
{
    internal abstract partial class Entry : IEntry
    {
        private IDictionary<string, PlcObjectBinding> _bindings = new Dictionary<string, PlcObjectBinding>();
        private readonly ReaderWriterLockSlim _bindingLock = new ReaderWriterLockSlim();
        private readonly PlcDataMapper _mapper;


        public string Name { get; private set; }
        public int ReadDataBlockSize { get; private set; }
        public int ValidationTimeMs { get; set; }
        public PlcObject PlcObject { get; private set; }
        public Dictionary<string, Tuple<int, PlcObject>> Variables { get; private set; }

        public bool HasActiveVariables
        {
            get
            {
                using (var guard = new ReaderGuard(_bindingLock))
                    return _bindings.Any(x => x.Value.IsActive);
            }
        }

  
        public Entry(PlcDataMapper mapper, PlcObject plcObject, int readDataBlockSize, int validationTimeInMs)
        {
            _mapper = mapper ?? throw new ArgumentNullException("mapper");
            ReadDataBlockSize = readDataBlockSize;
            ValidationTimeMs = validationTimeInMs;
            Variables = new Dictionary<string, Tuple<int, PlcObject>>();
            PlcObject = plcObject ?? throw new ArgumentNullException("plcObject");
            Name = plcObject.Name;
        }

        ~Entry()
        {
            _bindingLock.Dispose();
        }

        public IEnumerable<Execution> GetOperations(IEnumerable<string> vars)
        {
            UpdateInternalState(vars);
            return CreateExecutions(vars);
        }

        protected void UpdateInternalState(IEnumerable<string> vars)
        {
            if (AddObject(PlcObject, Variables, vars))
            {
                var bindings = new Dictionary<string, PlcObjectBinding>();
                foreach (var rawDataBlock in _mapper.Optimizer.CreateRawReadOperations(PlcObject.Selector, Variables, ReadDataBlockSize))
                {
                    if (rawDataBlock.References.Any())
                    {
                        //if (rawDataBlock.ReadDataCache == null || rawDataBlock.Size > rawDataBlock.ReadDataCache.Length)
                        //{
                        //    var current = rawDataBlock.ReadDataCache != null ? rawDataBlock.ReadDataCache.Length : 0;
                        //    Debug.WriteLine($"{rawDataBlock.ReadDataCache != null}, needed size:{rawDataBlock.Size}, current size:{current}");
                        //    lock (rawDataBlock)
                        //    {
                        //        if(rawDataBlock.ReadDataCache == null || rawDataBlock.Size > rawDataBlock.ReadDataCache.Length)
                        //            rawDataBlock.ReadDataCache = new byte[CalcRawDataSize(rawDataBlock.MemoryAllocationSize)];
                        //    }
                        //}

                        foreach (var reference in rawDataBlock.References)
                            bindings.Add(reference.Key, new PlcObjectBinding(rawDataBlock, reference.Value.Item2, reference.Value.Item1, ValidationTimeMs));
                    }
                }

                if (bindings.Any())
                {
                    using (var guard = new WriterGuard(_bindingLock))
                    {
                        //extend bindings with the new created ones
                        _bindings = _bindings != null
                                        ? _bindings.Union(bindings
                                            .Where(kvp => !_bindings.ContainsKey(kvp.Key))
                                            ).ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
                                        : bindings;
                    }
                }
            }
        }

        protected abstract bool AddObject(ITreeNode plcObj, Dictionary<string, Tuple<int, PlcObject>> plcObjects, IEnumerable<string> values);

        protected IEnumerable<Execution> CreateExecutions(IEnumerable<string> vars, bool onlyActive = false)
        {
            var result = new Dictionary<PlcRawData, Dictionary<string, PlcObjectBinding>>();
            IEnumerable<KeyValuePair<string, PlcObjectBinding>> bindingSnapshot = null;
            using (var guard = new ReaderGuard(_bindingLock))
            {
                bindingSnapshot = _bindings.Where(binding => (vars == null || vars.Contains(binding.Key)) && (!onlyActive || binding.Value.IsActive)).ToList();
            }

            foreach (var binding in bindingSnapshot)
            {
                if (!result.TryGetValue(binding.Value.RawData, out Dictionary<string, PlcObjectBinding> entry))
                {
                    entry = new Dictionary<string, PlcObjectBinding>();
                    result.Add(binding.Value.RawData, entry);
                }
                entry.Add($"{Name}.{binding.Key}", binding.Value);
            }
            return result.Select(res => new Execution(res.Key, res.Value, ValidationTimeMs));
        }

    }

}
