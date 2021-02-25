using Papper.Attributes;
using Papper.Extensions.Metadata;
using Papper.Extensions.Notification;
using Papper.Internal;
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
    public class PlcDataMapper : IDisposable
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
        private ReadOperation? _readEventHandler;
        private WriteOperation? _writeEventHandler;
        private UpdateMonitoring? _updateHandler;
        private ReadBlockInfo? _blockInfoHandler;
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
                            ReadOperation? readEventHandler,
                            WriteOperation? writeEventHandler,
                            UpdateMonitoring? updateHandler,
                            ReadBlockInfo? blockInfoHandler,
                            OptimizerType optimizer = OptimizerType.Block)
        {
            PduSize = pduSize;
            _readEventHandler = readEventHandler;
            _writeEventHandler = writeEventHandler;
            _updateHandler = updateHandler;
            _blockInfoHandler = blockInfoHandler;
            Optimizer = OptimizerFactory.CreateOptimizer(optimizer);
            ReadDataBlockSize = pduSize - _readDataHeaderLength;
            if (ReadDataBlockSize <= 0)
            {
                ExceptionThrowHelper.ThrowInvalidPduSizeException(_readDataHeaderLength);
            }

            PlcMetaDataTreePath.CreateAbsolutePath(PlcObjectResolver.RootNodeName);
        }

        /// <summary>
        /// Return a list of all registered Mappings
        /// </summary>
        public IEnumerable<string> Mappings => EntriesByName.Keys;

        /// <summary>
        /// Return all variable names of a mapping
        /// </summary>
        /// <param name="mapping"></param>
        /// <returns></returns>
        public IEnumerable<string> GetVariablesOf(string mapping)
        {
            var result = new List<string>();
            if (EntriesByName.TryGetValue(mapping, out var entry))
            {
                return PlcObjectResolver.GetLeafs(entry.PlcObject, result);
            }

            ExceptionThrowHelper.ThrowMappingNotFoundException(mapping);
            return Array.Empty<string>();
        }

        /// <summary>
        /// Return all writeable variable names of a mapping
        /// </summary>
        /// <param name="mapping"></param>
        /// <returns></returns>
        public IEnumerable<string> GetWriteableVariablesOf(string mapping)
        {
            var result = new List<string>();
            if (EntriesByName.TryGetValue(mapping, out var entry))
            {
                return PlcObjectResolver.GetLeafs(entry.PlcObject, result, true);
            }

            ExceptionThrowHelper.ThrowMappingNotFoundException(mapping);
            return Array.Empty<string>();
        }

        /// <summary>
        /// Return all writeable variable names of a mapping
        /// </summary>
        /// <param name="mapping"></param>
        /// <returns></returns>
        public IEnumerable<string> GetWriteableBlocksOf(string mapping)
        {
            var result = new List<string>();
            if (EntriesByName.TryGetValue(mapping, out var entry))
            {
                return PlcObjectResolver.GetWriteableBlocks(entry.PlcObject, result, out _);
            }

            ExceptionThrowHelper.ThrowMappingNotFoundException(mapping);
            return Array.Empty<string>();
        }

        /// <summary>
        /// Add a type with an MappingAttribute to register this type as an mapping for read and write operations
        /// </summary>
        /// <param name="type">Has to be a type with at least one MappingAttribute</param>
        /// <returns></returns>
        public bool AddMapping(Type type)
        {
            if (type == null)
            {
                ExceptionThrowHelper.ThrowArgumentNullException<Type>(nameof(type));
            }

            var mappingAttributes = type!.GetTypeInfo().GetCustomAttributes<MappingAttribute>().ToList();
            if (!mappingAttributes.Any())
            {
                ExceptionThrowHelper.ThrowMappingAttributeNotFoundForTypeException(type!);
            }

            return AddMappingsInternal(type!, mappingAttributes);
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
            {
                ExceptionThrowHelper.ThrowArgumentNullException<Type>(nameof(type));
            }

            if (!mappingAttributes.Any())
            {
                ExceptionThrowHelper.ThrowMappingAttributeNotFoundForTypeException(type!);
            }

            return AddMappingsInternal(type!, mappingAttributes);
        }

        /// <summary>
        /// Removes all mappings defined by the mapping attributes for the given type.
        /// </summary>
        /// <param name="type">Has to be a type with at least one MappingAttribute</param>
        /// <returns></returns>
        public bool RemoveMappings(Type type)
        {
            if (type == null)
            {
                ExceptionThrowHelper.ThrowArgumentNullException<Type>(nameof(type));
            }

            var mappingAttributes = type.GetTypeInfo().GetCustomAttributes<MappingAttribute>().ToList();
            if (!mappingAttributes.Any())
            {
                ExceptionThrowHelper.ThrowMappingAttributeNotFoundForTypeException(type!);
            }

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
                var values = vars.ToDictionary(x => x.Address, x => x.Value);
                // determine executions
                var executions = DetermineExecutions(vars);

                var prepared = executions.Select(execution => execution.CreateWriteDataPack(values, memoryBuffer))
                                         .ToDictionary(x => x.Key, x => x.Value);

                await WriteToPlcAsync(prepared.Values.SelectMany(x => x)).ConfigureAwait(false);
                executions.ForEach(exec =>
                {
                    if(exec.ExecutionResult == ExecutionResult.Ok)
                        exec.Invalidate();
                });

                return CreatePlcWriteResults(values, prepared);
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

        internal Task ReadFromPlcAsync(IEnumerable<DataPack> packs) => _readEventHandler != null ? _readEventHandler.Invoke(packs) : Task.CompletedTask;

        internal Task WriteToPlcAsync(IEnumerable<DataPack> packs) => _writeEventHandler != null ? _writeEventHandler.Invoke(packs) : Task.CompletedTask;

        internal Task UpdateMonitoringItemsAsync(IEnumerable<DataPack> monitoring, bool add = true) => _updateHandler != null ? _updateHandler.Invoke(monitoring, add) : Task.CompletedTask;

        internal Task ReadBlockInfos(IEnumerable<MetaDataPack> infos) => _blockInfoHandler != null ? _blockInfoHandler.Invoke(infos) : Task.CompletedTask;

        internal List<Execution> DetermineExecutions<T>(IEnumerable<T> vars) where T : IPlcReference
        {
            return vars.GroupBy(x => x.Mapping)
                                .Select((execution) => GetOrAddMapping(execution.Key, out var entry)
                                                                ? (execution, entry)
                                                                : (null, null))
                                .Where(x => x.execution != null)
                                .SelectMany(x => x.entry.GetOperations(x.execution.Select(exec => exec.Variable).ToList()))
                                .ToList();
        }

        internal static PlcReadResult[] CreatePlcReadResults(IEnumerable<Execution> executions,
                                                      Dictionary<Execution, DataPack> needUpdate,
                                                      DateTime? changedAfter = null,
                                                      Func<IEnumerable<KeyValuePair<string, PlcObjectBinding>>,
                                                      IEnumerable<KeyValuePair<string, PlcObjectBinding>>>? filter = null,
                                                      bool doNotConvert = false)
        {
            if (filter == null)
            {
                filter = (x) => x;
            }
            return executions.Select(exec => needUpdate.TryGetValue(exec, out var pack) ? exec.ApplyDataPack(pack) : exec)
                             .Where(exec => HasChangesSinceLastRun(exec, changedAfter))
                             .GroupBy(exec => exec.ExecutionResult) // Group by execution result
                             .Where(res => res.Key == ExecutionResult.Ok) // filter by OK results
                             .SelectMany(group => filter(group.SelectMany(g => g.Bindings))
                                                       .Select(b => new PlcReadResult(b.Key,
                                                                                      ConvertToResult(b.Value, doNotConvert),
                                                                                      group.Key)
                                                       )).ToArray();
        }

        private static PlcWriteResult[] CreatePlcWriteResults(Dictionary<string, object> values, Dictionary<Execution, IEnumerable<DataPack>> prepared)
        {
            var results = new PlcWriteResult[values.Count];
            var index = 0;
            foreach (var value in values)
            {
                var writePackages = prepared.Where(p => p.Key.Bindings.ContainsKey(value.Key)).SelectMany(x => x.Value);
                var execResult = writePackages.Any() ? ExecutionResult.Ok : ExecutionResult.Unknown;
                foreach (var package in writePackages)
                {
                    if (package.ExecutionResult != ExecutionResult.Ok)
                    {
                        execResult = package.ExecutionResult;
                        break;
                    }
                }

                results[index++] = new PlcWriteResult(value.Key, execResult);
            }

            return results;
        }

        private static bool HasChangesSinceLastRun(Execution exec, DateTime? changedAfter)
            => changedAfter == null || exec.LastChange > changedAfter;

        internal static PlcReadResult[] CreatePlcReadResults(IEnumerable<Execution> executions, IEnumerable<DataPack> packs)
        {
            return packs.Select(pack => executions.FirstOrDefault(x => pack.Selector == x.PlcRawData.Selector &&
                                                                pack.Offset == x.PlcRawData.Offset &&
                                                                pack.Length == (x.PlcRawData.Size > 0 ? x.PlcRawData.Size : 1))?.ApplyDataPack(pack))
                        .Where(exec => exec != null)
                        .GroupBy(exec => exec!.ExecutionResult) // Group by execution result
                        .Where(res => res.Key == ExecutionResult.Ok) // filter by OK results
                        .SelectMany(group => group.SelectMany(g => g!.Bindings)
                                                    .Select(b => new PlcReadResult(b.Key,
                                                                    b.Value?.ConvertFromRaw(b.Value.RawData.ReadDataCache.Span),
                                                                    group.Key)
                                                    )).ToArray();
        }

        internal static Dictionary<Execution, DataPack> UpdateableItems(List<Execution> executions, bool onlyOutdated, Func<IEnumerable<string>, DateTime, bool>? forceUpdate = null)
        {
            return executions.Where(exec => !onlyOutdated ||
                                            exec.ValidationTimeMs <= 0 ||
                                            exec.PlcRawData.LastUpdate.AddMilliseconds(exec.ValidationTimeMs) < DateTime.Now ||
                                            (forceUpdate != null && forceUpdate(exec.PlcRawData.References.Keys, exec.PlcRawData.LastUpdate)))
                             .Select(x => x.CreateReadDataPack())
                             .ToDictionary(x => x.Key, x => x.Value);
        }


        internal bool IsValidReference(PlcWatchReference watchs)
            => GetOrAddMapping(watchs.Mapping, out var entry) &&
                (entry is MappingEntry && entry.PlcObject.Get(new PlcMetaDataTreePath(watchs.Variable)) != null) ||
                (entry is RawEntry);
        #endregion


        

        private async Task<PlcReadResult[]> InternalReadAsync(IEnumerable<PlcReadReference> vars, bool doNotConvert)
        {
            var variables = vars;
            // determine executions
            var executions = DetermineExecutions(variables);

            // determine outdated
            var needUpdate = UpdateableItems(executions, true);  // true = read some items from cache!!

            // read from plc
            await ReadFromPlcAsync(needUpdate.Values).ConfigureAwait(false);

            // transform to result
            return CreatePlcReadResults(executions, needUpdate, null, null, doNotConvert);
        }

        private static object? ConvertToResult(PlcObjectBinding binding, bool doNotConvert = false)
        {
            if (doNotConvert)
            {
                return binding.RawData.ReadDataCache.Slice(binding.Offset, binding.Size).ToArray();
            }

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
                using var upgradeableGuard = new UpgradeableGuard(_mappingsLock);
                if (EntriesByName.TryGetValue(mapping.Name, out var existingMapping) && existingMapping is MappingEntry mappingEntry)
                {
                    if (mappingEntry.Mapping == mapping && mappingEntry.Type == type)
                    {
                        continue; // mapping already exists
                    }

                    return false; // mapping is invalid, because it exists for an other type
                }
                using (upgradeableGuard.UpgradeToWriterLock())
                {
                    EntriesByName.Add(mapping.Name, new MappingEntry(this, mapping, type, _tree, mapping.ObservationRate));
                }
            }
            return true;
        }

        private bool RemoveMappingsInternal(IEnumerable<string>? mappingNames)
        {
            if (mappingNames == null)
            {
                return false;
            }

            foreach (var mapping in mappingNames)
            {
                using var upgradeableGuard = new UpgradeableGuard(_mappingsLock);
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
            return true;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _mappingsLock.Dispose();
                }

                _readEventHandler = null;
                _writeEventHandler = null;
                _updateHandler = null;
                _blockInfoHandler = null;

                disposedValue = true;
            }
        }

        ~PlcDataMapper()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
