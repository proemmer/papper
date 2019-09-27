using Papper.Attributes;
using Papper.Extensions.Metadata;
using Papper.Extensions.Notification;
using Papper.Internal;
using Papper.Types;
using System;
using System.Buffers;
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
        private readonly HashSet<Subscription> _subscriptions = new HashSet<Subscription>();
        private const int _readDataHeaderLength = 18;
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

        internal Dictionary<string, IEntry> EntriesByName { get; } = new Dictionary<string, IEntry>();

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
            ReadDataBlockSize = pduSize - _readDataHeaderLength;
            if (ReadDataBlockSize <= 0) ExceptionThrowHelper.ThrowInvalidPduSizeException(_readDataHeaderLength);
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
        public IEnumerable<string> Mappings => EntriesByName.Keys;

        /// <summary>
        /// Return all variable names of an mapping
        /// </summary>
        /// <param name="mapping"></param>
        /// <returns></returns>
        public IEnumerable<string> GetVariablesOf(string mapping)
        {
            var result = new List<string>();
            if (EntriesByName.TryGetValue(mapping, out var entry))
                return PlcObjectResolver.GetLeafs(entry.PlcObject, result);
            ExceptionThrowHelper.ThrowMappingNotFoundException(mapping);
            return null;
        }

        /// <summary>
        /// Add a type with an MappingAttribute to register this type as an mapping for read and write operations
        /// </summary>
        /// <param name="type">Has to be a type with at least one MappingAttribute</param>
        /// <returns></returns>
        public bool AddMapping(Type type)
        {
            if (type == null)
                ExceptionThrowHelper.ThrowArgumentNullException<Type>(nameof(type));

            var mappingAttributes = type.GetTypeInfo().GetCustomAttributes<MappingAttribute>().ToList();
            if (!mappingAttributes.Any())
                ExceptionThrowHelper.ThrowMappingAttributeNotFoundForTypeException(type);
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
                ExceptionThrowHelper.ThrowArgumentNullException<Type>(nameof(type));

            if (!mappingAttributes.Any())
                ExceptionThrowHelper.ThrowMappingAttributeNotFoundForTypeException(type);

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
                ExceptionThrowHelper.ThrowArgumentNullException<Type>(nameof(type));

            var mappingAttributes = type.GetTypeInfo().GetCustomAttributes<MappingAttribute>().ToList();
            if (!mappingAttributes.Any())
                ExceptionThrowHelper.ThrowMappingAttributeNotFoundForTypeException(type);

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
        /// Read variables from an given mapping as the mapping specified type
        /// </summary>
        /// <param name="vars"></param>
        /// <returns>return a dictionary with all variables and the read value</returns>
        public Task<PlcReadResult[]> ReadAsync(IEnumerable<PlcReadReference> vars) => InternalReadAsync(vars, false);

        /// <summary>
        /// Read variables from an given mapping as byte[]
        /// </summary>
        /// <param name="vars"></param>
        /// <returns>return a dictionary with all variables and the read value</returns>
        public Task<PlcReadResult[]> ReadBytesAsync(IEnumerable<PlcReadReference> vars) => InternalReadAsync(vars, true);


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



                var values = vars.ToDictionary(x => x.Address, x => x.Value);
                // determine executions
                var executions = DetermineExecutions(vars);

                var prepared = executions.SelectMany(execution => execution.Bindings.Where(b => !b.Value.MetaData.IsReadOnly))
                                         .Select(x => UpdatePack(x.Key, Create(x.Key, x.Value, values, memoryBuffer), x.Value.Offset))
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
        /// If a client supports data changes it has to call this method on any changes
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

        internal bool AddSubscription(Subscription sub) => _subscriptions.Add(sub);

        internal bool RemoveSubscription(Subscription sub) => _subscriptions.Remove(sub);

        internal Task ReadFromPlcAsync(Dictionary<Execution, DataPack> needUpdate) => _readEventHandler?.Invoke(needUpdate.Values);

        internal Task WriteToPlcAsync(IEnumerable<DataPack> packs) => _writeEventHandler?.Invoke(packs);

        internal Task UpdateMonitoringItemsAsync(IEnumerable<DataPack> monitoring, bool add = true) => _updateHandler?.Invoke(monitoring, add);

        internal Task ReadBlockInfos(IEnumerable<MetaDataPack> infos) => _blockInfoHandler?.Invoke(infos);

        internal List<Execution> DetermineExecutions<T>(IEnumerable<T> vars) where T : IPlcReference
        {
            return vars.GroupBy(x => x.Mapping)
                                .Select((execution) => GetOrAddMapping(execution.Key, out var entry)
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
                                                      IEnumerable<KeyValuePair<string, PlcObjectBinding>>> filter = null,
                                                      bool doNotConvert = false)
        {
            if (filter == null)
            {
                filter = (x) => x;
            }
            return executions.Select(exec => needUpdate.TryGetValue(exec, out var pack) ? exec.ApplyDataPack(pack) : exec)
                             .Where(exec => changedAfter == null || exec.LastChange > changedAfter) // filter by data area
                             .GroupBy(exec => exec.ExecutionResult) // Group by execution result
                             .Where(res => res.Key == ExecutionResult.Ok) // filter by OK results
                             .SelectMany(group => filter(group.SelectMany(g => g.Bindings))
                                                       .Select(b => new PlcReadResult(b.Key,
                                                                                      ConvertToResult(b.Value, doNotConvert),
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
                        .Where(res => res.Key == ExecutionResult.Ok) // filter by OK results
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


        internal bool IsValidReference(PlcWatchReference watchs)
            => GetOrAddMapping(watchs.Mapping, out var entry) &&
                (entry is MappingEntry && entry.PlcObject.Get(new PlcMetaDataTreePath(watchs.Variable)) != null) ||
                (entry is RawEntry);
        #endregion


        private DataPack Create(string key, PlcObjectBinding binding, Dictionary<string, object> values, Dictionary<PlcRawData, byte[]> memoryBuffer)
        {
            byte[] GetOrCreateBufferAndApplyValue(PlcObjectBinding plcBinding, Dictionary<PlcRawData, byte[]> dict, object value)
            {
                if (!dict.TryGetValue(plcBinding.RawData, out var buffer))
                {
                    buffer = ArrayPool<byte>.Shared.Rent(plcBinding.RawData.MemoryAllocationSize);
                    dict.Add(plcBinding.RawData, buffer);
                }

                if (value is byte[] b && plcBinding.Size == b.Length)
                {
                    // we got raw data for the type, so we need not to convert them
                    b.CopyTo(buffer, 0);
                }
                else
                {
                    plcBinding.ConvertToRaw(value, buffer);
                }
                return buffer;
            }

            (var begin, var end) = CreateBitMasks(binding);
            return new DataPack
            {
                Selector = binding.RawData.Selector,
                Offset = binding.Offset + binding.RawData.Offset,
                Length = GetSize(binding),
                BitMaskBegin = begin,
                BitMaskEnd = end,
                Data = GetOrCreateBufferAndApplyValue(binding,
                                                        memoryBuffer,
                                                        values[key])
            };
        }


        private int GetSize(PlcObjectBinding binding)
        {
            if (binding.MetaData is PlcBool plcBool)
            {
                return 1;
            }
            else if (binding.MetaData is PlcArray plcArray && plcArray.ArrayType is PlcBool)
            {
                return plcArray.Size.Bytes + (plcArray.Size.Bits > 0 ? 1 : 0);
            }
            return binding.Size;
        }

        private (byte begin, byte end) CreateBitMasks(PlcObjectBinding binding)
        {
            byte begin = 0;
            byte end = 0;
            if (binding.MetaData is PlcBool plcBool)
            {
                begin = Converter.SetBit(0, plcBool.Offset.Bits, true);
            }
            else if (binding.MetaData is PlcArray plcArray && plcArray.ArrayType is PlcBool)
            {
                if (plcArray.Offset.Bits > 0 || ((plcArray.ArrayLength % 8) != 0))
                {
                    var take = 8 - plcArray.Offset.Bits;
                    foreach (var item in plcArray.Childs.Take(take).OfType<PlcObject>())
                    {
                        begin = Converter.SetBit(begin, item.Offset.Bits, true);
                    }

                    var lastItems = ((plcArray.Offset.Bits + plcArray.ArrayLength) % 8);
                    if (lastItems > 0)
                    {
                        foreach (var item in plcArray.Childs.Skip(plcArray.ArrayLength - lastItems).OfType<PlcObject>())
                        {
                            end = Converter.SetBit(end, item.Offset.Bits, true);
                        }

                        if (begin == 0) begin = 0xFF;
                    }
                    else if (begin > 0)
                    {
                        end = 0xFF;
                    }
                }

            }
            return (begin, end);
        }


        private async Task<PlcReadResult[]> InternalReadAsync(IEnumerable<PlcReadReference> vars, bool doNotConvert)
        {
            var variables = vars;
            // determine executions
            var executions = DetermineExecutions(variables);

            // determine outdated
            var needUpdate = UpdateableItems(executions, true);  // true = read some items from cache!!

            // read from plc
            await ReadFromPlcAsync(needUpdate);

            // transform to result
            return CreatePlcReadResults(executions, needUpdate, null, null, doNotConvert);
        }

        private object ConvertToResult(PlcObjectBinding binding, bool doNotConvert = false)
        {
            if (doNotConvert) return binding.RawData.ReadDataCache.Slice(binding.Offset, binding.Size).ToArray();
            return binding?.ConvertFromRaw(binding.RawData.ReadDataCache.Span);
        }

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
                                entry = new RawEntry(this, mapping, 0);
                                using (upgradeableGuard.UpgradeToWriterLock())
                                {
                                    EntriesByName.Add(mapping, entry);
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
                    if (EntriesByName.TryGetValue(mapping.Name, out var existingMapping) && existingMapping is MappingEntry mappingEntry)
                    {
                        if (mappingEntry.Mapping == mapping && mappingEntry.Type == type)
                            continue; // mapping already exists
                        return false; // mapping is invalid, because it exists for an other type
                    }
                    using (upgradeableGuard.UpgradeToWriterLock())
                    {
                        EntriesByName.Add(mapping.Name, new MappingEntry(this, mapping, type, _tree, mapping.ObservationRate));
                    }
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
                    if (EntriesByName.TryGetValue(mapping, out var existingMapping) && existingMapping is MappingEntry mappingEntry)
                    {
                        if (mappingEntry.Mapping.Name == mapping)
                        {
                            using (upgradeableGuard.UpgradeToWriterLock())
                            {
                                if (!EntriesByName.Remove(mapping))
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
