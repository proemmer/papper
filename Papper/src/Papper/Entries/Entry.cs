using Papper.Common;
using Papper.Helper;
using Papper.Interfaces;
using Papper.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Papper.Entries
{
    internal abstract class Entry : IEntry
    {
        private IDictionary<string, PlcObjectBinding> _bindings;
        private readonly ReaderWriterLockSlim _bindingLock = new ReaderWriterLockSlim();
        private readonly object _eventHandlerLock = new object();
        private readonly PlcDataMapper _mapper;
        private event OnChangeEventHandler EventHandler;
        private bool _isWatching;
        private CancellationTokenSource _cs;
        

        public string Name { get; private set; }
        public int ReadDataBlockSize { get; private set; }
        public int ValidationTimeMs { get; set; }
        public int DataChangeWatchCycleTimeMs { get; set; } = 500;
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

        public event OnChangeEventHandler OnChange
        {
            add
            {
                if (value != null)
                {
                    lock (_eventHandlerLock)
                        EventHandler += value;
                }
            }
            remove
            {
                if (value != null)
                {
                    lock (_eventHandlerLock)
                        EventHandler -= value;
                }
            }
        }

        public Entry(PlcDataMapper mapper, PlcObject plcObject, PlcMetaDataTree tree, int readDataBlockSize, int validationTimeInMs)
        {
            if (mapper == null)
                throw new ArgumentNullException("mapper");
            if (plcObject == null)
                throw new ArgumentNullException("plcObject");
            if (tree == null)
                throw new ArgumentNullException("tree");

            _mapper = mapper;
            ReadDataBlockSize = readDataBlockSize;
            ValidationTimeMs = validationTimeInMs;
            Variables = new Dictionary<string, Tuple<int, PlcObject>>();
            PlcObject = plcObject;
            Name = plcObject.Name;
        }

        ~Entry()
        {
            _bindingLock.Dispose();
        }

        public IEnumerable<Execution> GetOperations(string[] vars)
        {
            UpdateInternalState(vars);
            return CreateExecutions(vars);
        }

        public bool SetActiveState(bool enable, string[] vars)
        {
            try
            {
                if (!vars.Any())
                    return false;
                UpdateInternalState(vars);
                List<KeyValuePair<string, PlcObjectBinding>> toUpdate;
                using (var guard = new ReaderGuard(_bindingLock))
                    toUpdate = _bindings.Where(x => vars.Contains(x.Key)).ToList();

                foreach (var item in toUpdate)
                    item.Value.IsActive = enable;

                if(enable)
                    StartChangeDetection();
                else if(!HasActiveVariables)
                    StopChangeDetection();
            }
            catch(Exception)
            {
                return false;
            }
            return true;
        }

        protected void UpdateInternalState(string[] vars)
        {
            if (AddObject(PlcObject, Variables, vars))
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

        protected IEnumerable<Execution> CreateExecutions(string[] vars, bool onlyActive = false)
        {
            var result = new Dictionary<PlcRawData, Dictionary<string, PlcObjectBinding>>();
            IEnumerable<KeyValuePair<string, PlcObjectBinding>> bindingSnapshot = null;
            using (var guard = new ReaderGuard(_bindingLock))
                bindingSnapshot = _bindings.Where(binding => (vars == null || vars.Contains(binding.Key)) && (!onlyActive || binding.Value.IsActive)).ToList();

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

        protected int CalcRawDataSize(int size)
        {
            size = size > 0 ? size : 2;
            if (size % 2 != 0)
                size++;
            return size;
        }

        private void StartChangeDetection()
        {
            if (_isWatching)
                return;

            if (_cs != null)
                _cs.Dispose();

           

            _cs = new CancellationTokenSource();
            Task.Factory.StartNew(() =>
            {
                try
                {
                    Debug.WriteLine($"ChangeDetection for {Name} activated!");
                    _isWatching = true;
                    var forceUpdate = true;
                    var activeBindings = _bindings.Where(x => x.Value.IsActive).ToList();
                    var states = new Dictionary<PlcRawData, byte[]>();
                    var values = new Dictionary<string, byte[]>();

                    while (!_cs.IsCancellationRequested)
                    {
                        try
                        {
                            var force = forceUpdate;
                            _cs.Token.ThrowIfCancellationRequested();
                            var executions = CreateExecutions(null, true);
                            var changed = new Dictionary<string, object>();

                            foreach (var execution in executions)
                            {
                                _cs.Token.ThrowIfCancellationRequested();
                                byte[] raw = null;
                                states.TryGetValue(execution.PlcRawData, out raw);

                                if(_mapper.ReadRawIfDataChanged(execution, ref raw))
                                {
                                    _cs.Token.ThrowIfCancellationRequested();
                                    foreach (var binding in execution.Bindings)
                                    {
                                        byte[] value;
                                        if(!values.TryGetValue(binding.Key, out value) || !binding.Value.Data.SequenceEqual(binding.Value.Offset, value, binding.Value.Offset, binding.Value.Size))
                                        {
                                            changed.Add(binding.Key,binding.Value.ConvertFromRaw());
                                        }
                                        values[binding.Key] = binding.Value.Data;
                                    }
                                }
                            }

                            if (changed.Any())
                            {
                                _cs.Token.ThrowIfCancellationRequested();
                                lock (_eventHandlerLock)
                                {
                                    if (EventHandler != null)
                                        EventHandler(this, new PlcNotificationEventArgs(Name, changed));
                                }
                            }

                        }
                        catch (OperationCanceledException)
                        {
                            throw;
                        }
                        catch(Exception ex)
                        {
                            Debug.WriteLine($"Unknown exception for {Name}. Exception was {ex.Message}!");
                        }


                        Thread.Sleep(DataChangeWatchCycleTimeMs);
                    }
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    //Write debug output!
                    Debug.WriteLine($"Unknown exception for {Name}. Exception was {ex.Message}!");
                }
                finally
                {
                    _isWatching = false;
                    Debug.WriteLine($"ChangeDetection for {Name} deactivated!");
                }
                
            }, _cs.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default).ConfigureAwait(false);
        }

        private void StopChangeDetection()
        {
            if(_cs != null)
            {
                _cs.Cancel();
            }
        }
    }

}
