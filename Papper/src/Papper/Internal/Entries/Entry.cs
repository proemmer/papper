using Papper.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Papper.Internal
{
    internal abstract partial class Entry : IEntry, IDisposable
    {
        private IDictionary<string, PlcObjectBinding> _bindings = new Dictionary<string, PlcObjectBinding>();
        private readonly ReaderWriterLockSlim _bindingLock = new ReaderWriterLockSlim();
        private readonly PlcDataMapper _mapper;


        public string Name => PlcObject.Name;
        public int ReadDataBlockSize => _mapper.ReadDataBlockSize;
        public int ValidationTimeMs { get; set; }
        public PlcObject PlcObject { get; private set; }
        public Dictionary<string, Tuple<int, PlcObject>> Variables { get; private set; }

        public bool HasActiveVariables
        {
            get
            {
                using var guard = new ReaderGuard(_bindingLock);
                return _bindings.Any(x => x.Value.IsActive);
            }
        }


        public Entry(PlcDataMapper mapper, PlcObject? plcObject, int validationTimeInMs)
        {
            _mapper = mapper ?? ExceptionThrowHelper.ThrowArgumentNullException<PlcDataMapper>(nameof(mapper));
            PlcObject = plcObject ?? ExceptionThrowHelper.ThrowArgumentNullException<PlcObject>(nameof(plcObject));
            ValidationTimeMs = validationTimeInMs;
            Variables = new Dictionary<string, Tuple<int, PlcObject>>();
        }

        public IEnumerable<Execution> GetOperations(IEnumerable<string> vars)
        {
            UpdateInternalState(vars);
            return CreateExecutions(vars);
        }

        internal void UpdateInternalState(IEnumerable<string> vars)
        {
            using var guard = new UpgradeableGuard(_bindingLock);
            if (AddObject(PlcObject, Variables, vars))
            {
                var bindings = new Dictionary<string, PlcObjectBinding>();
                foreach (var rawDataBlock in _mapper.Optimizer.CreateRawReadOperations(PlcObject.Selector ?? string.Empty, Variables, ReadDataBlockSize))
                {
                    if (rawDataBlock.References.Any())
                    {
                        foreach (var reference in rawDataBlock.References)
                        {
                            bindings.Add(reference.Key, new PlcObjectBinding(rawDataBlock, reference.Value.Item2, reference.Value.Item1, ValidationTimeMs));
                        }
                    }
                }

                if (bindings.Any())
                {
                    //extend bindings with the new created ones
                    using var writeGuard = guard.UpgradeToWriterLock();
                    _bindings = _bindings != null
                                    ? _bindings.Union(bindings
                                        .Where(kvp => !_bindings.ContainsKey(kvp.Key))
                                        ).ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
                                    : bindings;
                }
            }
        }

        protected abstract bool AddObject(ITreeNode plcObj, Dictionary<string, Tuple<int, PlcObject>> plcObjects, IEnumerable<string> values);

        protected IEnumerable<Execution> CreateExecutions(IEnumerable<string> vars, bool onlyActive = false)
        {
            var result = new Dictionary<PlcRawData, Dictionary<string, PlcObjectBinding>>();
            IEnumerable<KeyValuePair<string, PlcObjectBinding>> bindingSnapshot;
            using (var guard = new ReaderGuard(_bindingLock))
            {
                bindingSnapshot = _bindings.Where(binding => (vars == null || vars.Contains(binding.Key)) && (!onlyActive || binding.Value.IsActive)).ToList();
            }

            foreach (var binding in bindingSnapshot)
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
                    _bindingLock.Dispose();
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
