using Papper.Types;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Papper.Internal
{
    internal abstract partial class Entry : IEntry, IDisposable
    {
        private readonly ConcurrentDictionary<string, PlcObjectBinding> _bindings = new();
        private readonly object _bindingLock = new();
        private readonly PlcDataMapper _mapper;


        public string Name => PlcObject.Name;
        public int ReadDataBlockSize => _mapper.ReadDataBlockSize;
        public int ValidationTimeMs { get; set; }
        public PlcObject PlcObject { get; private set; }
        public IDictionary<string, OperationItem> Variables { get; private set; }

        public Entry(PlcDataMapper mapper, PlcObject? plcObject, int validationTimeInMs)
        {
            _mapper = mapper ?? ExceptionThrowHelper.ThrowArgumentNullException<PlcDataMapper>(nameof(mapper));
            PlcObject = plcObject ?? ExceptionThrowHelper.ThrowArgumentNullException<PlcObject>(nameof(plcObject));
            ValidationTimeMs = validationTimeInMs;
            Variables = new ConcurrentDictionary<string, OperationItem>();
        }

        public IEnumerable<Execution> GetOperations(IEnumerable<string> vars)
        {
            UpdateInternalState(vars);
            return CreateExecutions(vars);
        }
        protected abstract bool AddObject(ITreeNode plcObj, IDictionary<string, OperationItem> plcObjects, IEnumerable<string> values);

        internal void UpdateInternalState(IEnumerable<string> vars)
        {
            if (vars.Any(v => !_bindings.ContainsKey(v)))
            {
                List<KeyValuePair<string, OperationItem>> currentVars;
                lock (_bindingLock)
                {
                    AddObject(PlcObject, Variables, vars);
                    currentVars = _mapper.Optimizer is not BlockBasedReadOperationOptimizer ?  Variables.Where(x => vars.Contains(x.Key)).ToList() : Variables.ToList() ;
                }

                foreach (var rawDataBlock in _mapper.Optimizer.CreateRawReadOperations(PlcObject.Name ?? string.Empty, PlcObject.Selector ?? string.Empty, currentVars, ReadDataBlockSize))
                {
                    if (rawDataBlock.References.Any())
                    {
                        foreach (var reference in rawDataBlock.References)
                        {
                            if (!_bindings.ContainsKey(reference.Key))
                            {
                                _bindings.TryAdd(reference.Key, new PlcObjectBinding(rawDataBlock, reference.Value.Item2, reference.Value.Item1, ValidationTimeMs));
                            }
                        }
                    }
                }
            }
        }

        protected IEnumerable<Execution> CreateExecutions(IEnumerable<string> vars)
        {
            var result = new Dictionary<PlcRawData, Dictionary<string, PlcObjectBinding>>();
            foreach (var binding in _bindings.Where(binding => (vars == null || vars.Contains(binding.Key))).ToList())
            {
                if (!result.TryGetValue(binding.Value.RawData, out var entry))
                {
                    entry = new Dictionary<string, PlcObjectBinding>();
                    result.Add(binding.Value.RawData, entry);
                }
                entry.Add($"{Name}.{binding.Key}", binding.Value);
            }
            return result.Select(res => new Execution(res.Key, res.Value, ValidationTimeMs, _mapper.Optimizer.SymbolicAccess));
        }

        #region IDisposable Support
        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {

                }
                _disposedValue = true;
            }
        }


        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }

}
