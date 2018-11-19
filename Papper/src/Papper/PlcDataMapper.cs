using Papper.Attributes;
using Papper.Internal;
using Papper.Extensions.Notification;
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Papper.Extensions.Metadata;

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
        /// This delegate is used to invoke the write operations.
        /// </summary>
        /// <returns></returns>
        public delegate Task UpdateMonitoring(IEnumerable<DataPack> monitoring, bool add = true);

        /// <summary>
        /// This delegate is used to invoke the block info operations.
        /// </summary>
        /// <returns></returns>
        public delegate Task ReadBlockInfo(IEnumerable<MetaDataPack> metadatas);

        #endregion

        #region Fields
        private HashSet<Subscription> _subscriptions = new HashSet<Subscription>();
        private const int PduSizeDefault = 480;
        private const int ReadDataHeaderLength = 18;
        private readonly PlcMetaDataTree _tree = new PlcMetaDataTree();
        private readonly ReaderWriterLockSlim _mappingsLock = new ReaderWriterLockSlim();
        private ReadOperation _readEventHandler;
        private WriteOperation _writeEventHandler;
        private UpdateMonitoring _updateHandler;
        private ReadBlockInfo _blockInfoHandler;
        #endregion

        #region Properties

        public int ReadDataBlockSize { get; private set; }
        public int PduSize { get; private set; }
        internal IReadOperationOptimizer Optimizer { get; }

        internal ConcurrentDictionary<string, IEntry> EntriesByName { get; } = new ConcurrentDictionary<string, IEntry>();

        #endregion


        public PlcDataMapper(int pduSize,
                     ReadBlockInfo blockInfoHandler,
                     OptimizerType optimizer = OptimizerType.Block) : this(pduSize, null, null, null, blockInfoHandler, optimizer)
        {
        }

        public PlcDataMapper(int pduSize,
                    ReadOperation readEventHandler,
                    OptimizerType optimizer = OptimizerType.Block) : this(pduSize, readEventHandler, null, null, null, optimizer)
        {
        }

        public PlcDataMapper(int pduSize,
                WriteOperation writeEventHandler,
                OptimizerType optimizer = OptimizerType.Block) : this(pduSize, null, writeEventHandler, null, null, optimizer)
        {
        }

        public PlcDataMapper(int pduSize,
                            ReadOperation readEventHandler,
                            WriteOperation writeEventHandler,
                            OptimizerType optimizer = OptimizerType.Block) : this(pduSize, readEventHandler, writeEventHandler, null, null, optimizer)
        {
        }

        public PlcDataMapper(int pduSize,
                    ReadOperation readEventHandler,
                    WriteOperation writeEventHandler,
                    UpdateMonitoring updateHandler,
                    OptimizerType optimizer = OptimizerType.Block) : this(pduSize, readEventHandler, writeEventHandler, updateHandler, null, optimizer)
        {
        }

        public PlcDataMapper(int pduSize,
                            ReadOperation readEventHandler,
                            WriteOperation writeEventHandler,
                            UpdateMonitoring updateHandler,
                            ReadBlockInfo blockInfoHandler,
                            OptimizerType optimizer = OptimizerType.Block)
        {
            PduSize = pduSize;
            _readEventHandler = readEventHandler;
            _writeEventHandler = writeEventHandler;
            _updateHandler = updateHandler;
            _blockInfoHandler = blockInfoHandler;
            Optimizer = OptimizerFactory.CreateOptimizer(optimizer);
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
            _blockInfoHandler = null;
        }

        /// <summary>
        /// Return a list of all registered Mappings
        /// </summary>
        public IEnumerable<string> Mappings { get { return EntriesByName.Keys; } }

        /// <summary>
        /// Return all variable names of an mapping
        /// </summary>
        /// <param name="mapping"></param>
        /// <returns></returns>
        public IEnumerable<string> GetVariablesOf(string mapping)
        {
            var result = new List<string>();
            if (EntriesByName.TryGetValue(mapping, out IEntry entry))
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

            return AddMappingsInternal(type, mappingAttributes);
        }

        /// <summary>
        /// Add a type with and a list og MappingAttributes to register this type as an mapping for read and write operations
        /// </summary>
        /// <param name="type">Could be any type</param>
        /// <param name="mappingAttributes">mappings to add</param>
        /// <returns></returns>
        public bool AddMapping(Type type, params MappingAttribute[] mappingAttributes)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (!mappingAttributes.Any())
                throw new ArgumentException("No MappingAttributes given");

            return AddMappingsInternal(type, mappingAttributes);
        }

        /// <summary>
        /// Removes all mappings defined by the mapping attributes for the given type.
        /// </summary>
        /// <param name="type">Has to be a type with at least one MappingAttribute</param>
        /// <returns></returns>
        public bool RemoveMappings(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            var mappingAttributes = type.GetTypeInfo().GetCustomAttributes<MappingAttribute>().ToList();
            if (!mappingAttributes.Any())
                throw new ArgumentException("The given type has no MappingAttribute", "type");

            return RemoveMappingsInternal(mappingAttributes.Select(x => x.Name));
        }

        /// <summary>
        /// Removes all given mappings.
        /// </summary>
        /// <param name="mappings">Mappings names to remove</param>
        /// <returns></returns>
        public bool RemoveMappings(IEnumerable<string> mappings) => RemoveMappingsInternal(mappings);

        /// <summary>
        /// Removes all given mappings.
        /// </summary>
        /// <param name="mappings">Mappings names to remove</param>
        /// <returns></returns>
        public bool RemoveMappings(params string[] mappings) => RemoveMappingsInternal(mappings);


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
            var needUpdate = UpdateableItems(executions, true);  // true = read some items from cache!!

            // read from plc
            await ReadFromPlcAsync(needUpdate);

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
                    pack.Data = pack.Data.Slice(dataOffset, pack.Length);
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

                await WriteToPlcAsync(prepared.Values);

                executions.ForEach(exec => exec.Invalidate());

                return prepared.Select(x => new PlcWriteResult(x.Key, x.Value.ExecutionResult)).ToArray();
            }
            finally
            {
                memoryBuffer.Values.ToList().ForEach(x => ArrayPool<byte>.Shared.Return(x));
            }
        }


        /// <summary>
        /// If a client supports datachanges it has to call this method on any changes
        /// </summary>
        /// <param name="changed"></param>
        public void OnDataChanges(IEnumerable<DataPack> changed)
        {
            foreach (var item in _subscriptions.ToList())
            {
                item.OnDataChanged(changed);
            }
        }


        #region internal

        internal bool AddSubscription(Subscription sub)
        {
            return _subscriptions.Add(sub);
        }

        internal bool RemoveSubscription(Subscription sub)
        {
            return _subscriptions.Remove(sub);
        }

        internal Task ReadFromPlcAsync(Dictionary<Execution, DataPack> needUpdate)
        {
            return _readEventHandler?.Invoke(needUpdate.Values);
        }

        internal Task WriteToPlcAsync(IEnumerable<DataPack> packs) => _writeEventHandler?.Invoke(packs);

        internal Task UpdateMonitoringItemsAsync(IEnumerable<DataPack> monitoring, bool add = true) => _updateHandler?.Invoke(monitoring, add);

        internal Task ReadBlockInfos(IEnumerable<MetaDataPack> infos) => _blockInfoHandler?.Invoke(infos);

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
                                                                                      b.Value?.ConvertFromRaw(b.Value.RawData.ReadDataCache.Span), 
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
                                                                    b.Value?.ConvertFromRaw(b.Value.RawData.ReadDataCache.Span),
                                                                    group.Key)
                                                    )).ToArray();
        }

        internal Dictionary<Execution, DataPack> UpdateableItems(List<Execution> executions, bool onlyOutdated, Func<IEnumerable<string>, DateTime, bool> forceUpdate = null)
        {
            return executions.Where(exec => !onlyOutdated || 
                                            exec.ValidationTimeMs <= 0 || 
                                            exec.PlcRawData.LastUpdate.AddMilliseconds(exec.ValidationTimeMs) < DateTime.Now ||
                                            (forceUpdate != null && forceUpdate(exec.PlcRawData.References.Keys, exec.PlcRawData.LastUpdate)))
                           .Select(x => new KeyValuePair<Execution, DataPack>(x,
                                                                            new DataPack
                                                                            {
                                                                                Selector = x.PlcRawData.Selector,
                                                                                Offset = x.PlcRawData.Offset,
                                                                                Length = x.PlcRawData.Size > 0 ? x.PlcRawData.Size : 1
                                                                            }))
                           .ToDictionary(x => x.Key, x => x.Value);
        }

        #endregion

        private bool GetOrAddMapping(string mapping, out IEntry entry)
        {
            if (EntriesByName.TryGetValue(mapping, out entry))
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
                    {
                        using (var upgradeableGuard = new UpgradeableGuard(_mappingsLock))
                        {
                            if (!EntriesByName.TryGetValue(mapping, out entry))
                            {
                                entry = new RawEntry(this, mapping, ReadDataBlockSize, 0);
                                using (upgradeableGuard.UpgradeToWriterLock())
                                {
                                    EntriesByName.TryAdd(mapping, entry);
                                }
                            }
                        }
                        return true;
                    }
            }

            return false;
        }

        private bool AddMappingsInternal(Type type, IEnumerable<MappingAttribute> mappingAttributes)
        {
            foreach (var mapping in mappingAttributes)
            {
                using (var upgradeableGuard = new UpgradeableGuard(_mappingsLock))
                {
                    if (EntriesByName.TryGetValue(mapping.Name, out IEntry existingMapping) && existingMapping is MappingEntry mappingEntry)
                    {
                        if (mappingEntry.Mapping == mapping && mappingEntry.Type == type)
                            continue; // mapping already exists
                        return false; // mapping is invlaid, because it exists for a nother type
                    }
                    using (upgradeableGuard.UpgradeToWriterLock())
                        EntriesByName.TryAdd(mapping.Name, new MappingEntry(this, mapping, type, _tree, ReadDataBlockSize, mapping.ObservationRate));
                }
            }
            return true;
        }

        private bool RemoveMappingsInternal(IEnumerable<string> mappingNames)
        {
            foreach (var mapping in mappingNames)
            {
                using (var upgradeableGuard = new UpgradeableGuard(_mappingsLock))
                {
                    if (EntriesByName.TryGetValue(mapping, out IEntry existingMapping) && existingMapping is MappingEntry mappingEntry)
                    {
                        if (mappingEntry.Mapping.Name == mapping)
                        {
                            using (upgradeableGuard.UpgradeToWriterLock())
                            {
                                if(!EntriesByName.TryRemove(mapping, out var removed))
                                {
                                    return false;
                                }
                            }
                        }
                     
                    }
                }
            }
            return true;
        }
    }
}
