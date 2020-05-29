using Papper.Types;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Papper.Internal
{
    internal abstract partial class Entry : IEntry, IDisposable
    {
        private readonly ConcurrentDictionary<string, PlcObjectBinding> _bindings = new ConcurrentDictionary<string, PlcObjectBinding>();
        private readonly object _bindingLock = new object();
        private readonly PlcDataMapper _mapper;


        public string Name => PlcObject.Name;
        public int ReadDataBlockSize => _mapper.ReadDataBlockSize;
        public int ValidationTimeMs { get; set; }
        public PlcObject PlcObject { get; private set; }
        public IDictionary<string, Tuple<int, PlcObject>> Variables { get; private set; }

        public bool HasActiveVariables
        {
            get
            {
                return _bindings.Any(x => x.Value.IsActive);
            }
        }


        public Entry(PlcDataMapper mapper, PlcObject? plcObject, int validationTimeInMs)
        {
            _mapper = mapper ?? ExceptionThrowHelper.ThrowArgumentNullException<PlcDataMapper>(nameof(mapper));
            PlcObject = plcObject ?? ExceptionThrowHelper.ThrowArgumentNullException<PlcObject>(nameof(plcObject));
            ValidationTimeMs = validationTimeInMs;
            Variables = new ConcurrentDictionary<string, Tuple<int, PlcObject>>();
        }

        public IEnumerable<Execution> GetOperations(IEnumerable<string> vars)
        {
            UpdateInternalState(vars);
            return CreateExecutions(vars);
        }

        internal void UpdateInternalState(IEnumerable<string> vars)
        {
            if (vars.Any(v => !_bindings.ContainsKey(v)))
            {
                List<KeyValuePair<string, Tuple<int, PlcObject>>> currentVars;
                lock(_bindingLock)
                {
                    AddObject(PlcObject, Variables, vars);
                    currentVars = _mapper.Optimizer is ItemBasedReadOperationOptimizer ? Variables.Where(x => vars.Contains(x.Key)).ToList() : Variables.ToList();
                }

                var bindings = new Dictionary<string, PlcObjectBinding>();
                foreach (var rawDataBlock in _mapper.Optimizer.CreateRawReadOperations(PlcObject.Selector ?? string.Empty, currentVars, ReadDataBlockSize))
                {
                    if (rawDataBlock.References.Any())
                    {
                        foreach (var reference in rawDataBlock.References)
                        {
                            _bindings.TryAdd(reference.Key, new PlcObjectBinding(rawDataBlock, reference.Value.Item2, reference.Value.Item1, ValidationTimeMs));
                        }
                    }
                }
            }
        }

        protected abstract bool AddObject(ITreeNode plcObj, IDictionary<string, Tuple<int, PlcObject>> plcObjects, IEnumerable<string> values);

        protected IEnumerable<Execution> CreateExecutions(IEnumerable<string> vars, bool onlyActive = false)
        {
            var result = new Dictionary<PlcRawData, Dictionary<string, PlcObjectBinding>>();
            foreach (var binding in _bindings.Where(binding => (vars == null || vars.Contains(binding.Key)) && (!onlyActive || binding.Value.IsActive)).ToList())
            {
                if (!result.TryGetValue(binding.Value.RawData, out var entry))
                {
                    entry = new Dictionary<string, PlcObjectBinding>();
                    result.Add(binding.Value.RawData, entry);
                }
                entry.Add($"{Name}.{binding.Key}", binding.Value);
            }
            return result.Select(res => new Execution(res.Key, res.Value, ValidationTimeMs));
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                }
                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Entry()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion

    }

}
