using Papper.Attributes;
using Papper.Internal;
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Papper
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class PlcDataMapper
    {
        #region Delegates
        /// <summary>
        /// This delegate is used to invoke the read operations
        /// </summary>
        /// <returns></returns>
        public delegate Task ReadOperation(IEnumerable<DataPack> reads);

        /// <summary>
        /// his delegate is used to invoke the write operations.
        /// </summary>
        /// <returns></returns>
        public delegate Task WriteOperation(IEnumerable<DataPack> writes);

        /// <summary>
        /// his delegate is used to invoke the write operations.
        /// </summary>
        /// <returns></returns>
        public delegate Task UpdateMonitoring(IEnumerable<DataPack> monitoring, bool add = true);

        #endregion

        #region Fields
        private HashSet<Subscription> _subscriptions = new HashSet<Subscription>();
        private const int PduSizeDefault = 480;
        private const int ReadDataHeaderLength = 18;
        private readonly PlcMetaDataTree _tree = new PlcMetaDataTree();
        private readonly ConcurrentDictionary<string, IEntry> _mappings = new ConcurrentDictionary<string, IEntry>();
        private readonly ReaderWriterLockSlim _mappingsLock = new ReaderWriterLockSlim();
        private ReadOperation _readEventHandler;
        private WriteOperation _writeEventHandler;
        private UpdateMonitoring _updateHandler;
        private readonly IReadOperationOptimizer _optimizer;
        #endregion

        #region Properties


        public int ReadDataBlockSize { get; private set; }
        public int PduSize { get; private set; }

        internal IReadOperationOptimizer Optimizer => _optimizer;
        #endregion

        public PlcDataMapper(int pduSize,
                             ReadOperation readEventHandler,
                             WriteOperation writeEventHandler = null,
                             UpdateMonitoring updateHandler = null,
                             OptimizerType optimizer = OptimizerType.Block)
        {
            PduSize = pduSize;
            _readEventHandler = readEventHandler;
            _writeEventHandler = writeEventHandler;
            _updateHandler = updateHandler;
            _optimizer = OptimizerFactory.CreateOptimizer(optimizer);
            ReadDataBlockSize = pduSize - ReadDataHeaderLength;
            if (ReadDataBlockSize <= 0)
                throw new ArgumentException($"PDU size have to be greater then {ReadDataHeaderLength}", "pduSize");
            PlcMetaDataTreePath.CreateAbsolutePath(PlcObjectResolver.RootNodeName);
        }

        ~PlcDataMapper()
        {
            _readEventHandler = null;
            _writeEventHandler = null;
            _updateHandler = null;
        }

        /// <summary>
        /// Return a list of all registered Mappings
        /// </summary>
        public IEnumerable<string> Mappings { get { return _mappings.Keys; } }

        /// <summary>
        /// Return all variable names of an mapping
        /// </summary>
        /// <param name="mapping"></param>
        /// <returns></returns>
        public IEnumerable<string> GetVariablesOf(string mapping)
        {
            var result = new List<string>();
            if (_mappings.TryGetValue(mapping, out IEntry entry))
                return PlcObjectResolver.GetLeafs(entry.PlcObject, result);
            throw new KeyNotFoundException($"The mapping {mapping} does not exist.");
        }

        /// <summary>
        /// Add a type with an MappingAttribute to register this type as an mapping for read and write operations
        /// </summary>
        /// <param name="type">Has to be a type with at least one MappingAttribute</param>
        /// <returns></returns>
        public bool AddMapping(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            var mappingAttributes = type.GetTypeInfo().GetCustomAttributes<MappingAttribute>().ToList();
            if (!mappingAttributes.Any())
                throw new ArgumentException("The given type has no MappingAttribute", "type");

            foreach (var mapping in mappingAttributes)
            {

                using (var upgradeableGuard = new UpgradeableGuard(_mappingsLock))
                {
                    if (_mappings.TryGetValue(mapping.Name, out IEntry existingMapping))
                    {
                        var mappingEntry = existingMapping as MappingEntry;
                        return mappingEntry.Mapping == mapping && mappingEntry.Type == type;
                    }
                    using (upgradeableGuard.UpgradeToWriterLock())
                        _mappings.TryAdd(mapping.Name, new MappingEntry(this, mapping, type, _tree, ReadDataBlockSize, mapping.ObservationRate));
                }
            }
            return true;
        }

        /// <summary>
        /// Read variables from an given mapping
        /// </summary>
        /// <param name="vars"></param>
        /// <returns>return a dictionary with all variables and the read value</returns>
        public Task<PlcReadResult[]> ReadAsync(params PlcReadReference[] vars) => ReadAsync(vars as IEnumerable<PlcReadReference>);

        /// <summary>
        /// Read variables from an given mapping
        /// </summary>
        /// <param name="vars"></param>
        /// <returns>return a dictionary with all variables and the read value</returns>
        public async Task<PlcReadResult[]> ReadAsync(IEnumerable<PlcReadReference> vars)
        {
            // determine executions
            var executions = DetermineExecutions(vars);

            // determine outdated
            var needUpdate = UpdateableItems(executions);

            // read from plc
            await ReadFromPlc(needUpdate);

            // transform to result
            return CreatePlcReadResults(executions, needUpdate);
        }

        /// <summary>
        /// Write values to variables of an given mapping
        /// </summary>
        /// <param name="mapping">mapping name specified in the MappingAttribute</param>
        /// <param name="values">variable names and values to write</param>
        /// <returns>return true if all operations are succeeded</returns>
        public Task<PlcWriteResult[]> WriteAsync(params PlcWriteReference[] vars) => WriteAsync(vars as IEnumerable<PlcWriteReference>);

        /// <summary>
        /// Write values to variables of an given mapping
        /// </summary>
        /// <param name="mapping">mapping name specified in the MappingAttribute</param>
        /// <param name="values">variable names and values to write</param>
        /// <returns>return true if all operations are succeeded</returns>
        public async Task<PlcWriteResult[]> WriteAsync(IEnumerable<PlcWriteReference> vars)
        {
            
            // because we need the byte arrays only for converting, we can use the ArrayPool
            var memoryBuffer = new Dictionary<PlcRawData, byte[]>();
            try
            {
                KeyValuePair<string, DataPack> UpdatePack(string key, DataPack pack, int dataOffset)
                {
                    pack.Data = pack.Data.SubArray(dataOffset, pack.Length);
                    return new KeyValuePair<string, DataPack>(key, pack);
                }

                byte[] GetOrCreateBufferAndApplyValue(PlcObjectBinding binding, Dictionary<PlcRawData, byte[]> dict, object value)
                {
                    if (!dict.TryGetValue(binding.RawData, out var buffer))
                    {
                        buffer = ArrayPool<byte>.Shared.Rent(binding.RawData.MemoryAllocationSize);
                        dict.Add(binding.RawData, buffer);
                    }
                    binding.ConvertToRaw(value, buffer);
                    return buffer;
                }

                var values = vars.ToDictionary(x => x.Address, x => x.Value);
                // determine executions
                var executions = DetermineExecutions(vars);

                var prepared = executions.SelectMany(execution => execution.Bindings.Where(b => !b.Value.MetaData.IsReadOnly))
                                         .Select(x => UpdatePack(x.Key, new DataPack
                                         {
                                             Selector = x.Value.RawData.Selector,
                                             Offset = x.Value.Offset + x.Value.RawData.Offset,
                                             Length = x.Value.Size > 0 ? x.Value.Size : 1,
                                             BitMask = x.Value.Size <= 0 ? Converter.SetBit(0, x.Value.MetaData.Offset.Bits, true) : (byte)0,
                                             Data = GetOrCreateBufferAndApplyValue( x.Value,
                                                                                    memoryBuffer,
                                                                                    values[x.Key])
                                         }, x.Value.Offset))
                                                .ToDictionary(x => x.Key, x => x.Value);

                await WriteToPlc(prepared.Values);

                executions.ForEach(exec => exec.Invalidate());

                return prepared.Select(x => new PlcWriteResult(x.Key, x.Value.ExecutionResult)).ToArray();
            }
            finally
            {
                memoryBuffer.Values.ToList().ForEach(x => ArrayPool<byte>.Shared.Return(x));
            }
        }

        /// <summary>
        /// Return address data of the given variable
        /// </summary>
        /// <param name="mapping">name of the mapping</param>
        /// <param name="variable">name of the variable</param>
        /// <returns></returns>
        public PlcItemAddress GetAddressOf(string mapping, string variable)
        {
            if (string.IsNullOrWhiteSpace(mapping))
                throw new ArgumentException("The given argument could not be null or whitespace.", "mapping");
            if (string.IsNullOrWhiteSpace(variable))
                throw new ArgumentException("The given argument could not be null or whitespace.", "variable");

            var result = new Dictionary<string, object>();
            if (_mappings.TryGetValue(mapping, out IEntry entry))
            {
                if (entry.Variables.TryGetValue(variable, out Tuple<int, Types.PlcObject> varibleEntry))
                {
                    return new PlcItemAddress(
                        varibleEntry.Item2.Selector,
                        varibleEntry.Item2.ElemenType,
                        varibleEntry.Item2.Offset,
                        varibleEntry.Item2.Size
                        );
                }
            }
            throw new KeyNotFoundException($"There is variable <{variable}> for mapping <{mapping}>");
        }

        /// <summary>
        /// Create a Subscription to watch data changes
        /// </summary>
        /// <returns></returns>
        public Subscription CreateSubscription(ChangeDetectionStrategy changeDetectionStrategy = ChangeDetectionStrategy.Polling)
        {
            var sub = new Subscription(this, changeDetectionStrategy);
            _subscriptions.Add(sub);
            return sub;
        }

        public void OnDataChanges(IEnumerable<DataPack> changed)
        {
            foreach (var item in _subscriptions.ToList())
            {
                item.OnDataChanged(changed);
            }
        }


        #region internal read write operations

        internal bool RemoveSubscription(Subscription sub)
        {
            return _subscriptions.Remove(sub);
        }


        internal async Task ReadFromPlc(Dictionary<Execution, DataPack> needUpdate)
        {
            await _readEventHandler?.Invoke(needUpdate.Values);
        }

        internal async Task WriteToPlc(IEnumerable<DataPack> packs)
        {
            await _writeEventHandler?.Invoke(packs);
        }

        internal async Task UpdateMonitoringItems(IEnumerable<DataPack> monitoring, bool add = true)
        {
            await _updateHandler?.Invoke(monitoring, add);
        }

        internal List<Execution> DetermineExecutions<T>(IEnumerable<T> vars) where T: IPlcReference
        {
            return vars.GroupBy(x => x.Mapping)
                                .Select((execution) => GetOrAddMapping(execution.Key, out IEntry entry)
                                                                ? (execution, entry)
                                                                : (null, null))
                                .Where(x => x.execution != null)
                                .SelectMany(x => x.entry.GetOperations(x.execution.Select(exec => exec.Variable)))
                                .ToList();
        }


        internal PlcReadResult[] CreatePlcReadResults(IEnumerable<Execution> executions, 
                                                      Dictionary<Execution, DataPack> needUpdate,
                                                      DateTime? changedAfter = null,
                                                      Func<IEnumerable<KeyValuePair<string, PlcObjectBinding>>, 
                                                      IEnumerable<KeyValuePair<string, PlcObjectBinding>>> filter = null)
        {
            if (filter == null)
            {
                filter = (x) => x;
            }
            return executions.Select(exec => needUpdate.TryGetValue(exec, out var pack) ? exec.ApplyDataPack(pack) : exec)
                             .Where(exec => changedAfter == null || exec.LastChange > changedAfter) // filter by data area
                             .GroupBy(exec => exec.ExecutionResult) // Group by execution result
                             .SelectMany(group => filter(group.SelectMany(g => g.Bindings))
                                                       .Select(b => new PlcReadResult(b.Key, 
                                                                                      b.Value?.ConvertFromRaw(b.Value.RawData.ReadDataCache), 
                                                                                      group.Key)
                                                       )).ToArray();
        }

        internal PlcReadResult[] CreatePlcReadResults(IEnumerable<Execution> executions, IEnumerable<DataPack> packs)
        {
            return packs.Select(pack => executions.FirstOrDefault(x => pack.Selector == x.PlcRawData.Selector &&
                                                                pack.Offset == x.PlcRawData.Offset &&
                                                                pack.Length == (x.PlcRawData.Size > 0 ? x.PlcRawData.Size : 1))?.ApplyDataPack(pack))
                        .Where(exec => exec != null)
                        .GroupBy(exec => exec.ExecutionResult) // Group by execution result
                        .SelectMany(group => group.SelectMany(g => g.Bindings)
                                                    .Select(b => new PlcReadResult(b.Key,
                                                                    b.Value?.ConvertFromRaw(b.Value.RawData.ReadDataCache),
                                                                    group.Key)
                                                    )).ToArray();
        }

        internal Dictionary<Execution, DataPack> UpdateableItems(List<Execution> executions, bool onlyOutdated = true)
        {
            return executions.Where(exec => !onlyOutdated || exec.ValidationTimeMs <= 0 || exec.PlcRawData.LastUpdate.AddMilliseconds(exec.ValidationTimeMs) < DateTime.Now)
                           .Select(x => new KeyValuePair<Execution, DataPack>(x,
                                                                            new DataPack
                                                                            {
                                                                                Selector = x.PlcRawData.Selector,
                                                                                Offset = x.PlcRawData.Offset,
                                                                                Length = x.PlcRawData.Size > 0 ? x.PlcRawData.Size : 1
                                                                            }))
                           .ToDictionary(x => x.Key, x => x.Value);
        }


        private bool GetOrAddMapping(string mapping, out IEntry entry)
        {
            if (_mappings.TryGetValue(mapping, out entry))
            {
                return true;
            }

            // If the mapping is an absolute mapping we can create an entry
            switch (mapping)
            {
                case "IB":
                case "FB":
                case "QB":
                case "TM":
                case "CT":
                case var s when Regex.IsMatch(s, "^DB\\d+$"):
                    using (var upgradeableGuard = new UpgradeableGuard(_mappingsLock))
                    {
                        if (!_mappings.TryGetValue(mapping, out entry))
                        {
                            entry = new RawEntry(this, mapping, ReadDataBlockSize, 0);
                            using (upgradeableGuard.UpgradeToWriterLock())
                                _mappings.TryAdd(mapping, entry);
                        }
                    }
                    return true;
            }

            return false;
        }

        #endregion

    }
}
