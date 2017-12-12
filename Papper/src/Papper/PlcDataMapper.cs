using Papper.Attributes;
using Papper.Internal;
using System;
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
        public delegate Task WriteOperation(IEnumerable<DataPack> reads);


        #endregion

        #region Fields
        private HashSet<Subscription> _subscriptions = new HashSet<Subscription>();
        private const int PduSizeDefault = 480;
        private const int ReadDataHeaderLength = 18;
        private readonly PlcMetaDataTree _tree = new PlcMetaDataTree();
        private readonly ConcurrentDictionary<string, IEntry> _mappings = new ConcurrentDictionary<string, IEntry>();
        private readonly ReaderWriterLockSlim _mappingsLock = new ReaderWriterLockSlim();
        private event ReadOperation _onRead;
        private event WriteOperation _onWrite;
        private readonly IReadOperationOptimizer _optimizer;
        #endregion

        #region Properties


        public int ReadDataBlockSize { get; private set; }
        public int PduSize { get; private set; }
        public event ReadOperation OnRead
        {
            add
            {
                _onRead += value;
            }
            remove
            {
                _onRead -= value;
            }
        }

        public event WriteOperation OnWrite
        {
            add
            {
                _onWrite += value;
            }
            remove
            {
                _onWrite -= value;
            }
        }

        internal IReadOperationOptimizer Optimizer => _optimizer;
        #endregion

        public PlcDataMapper(int pduSize = PduSizeDefault, OptimizerType optimizer = OptimizerType.Block)
        {
            PduSize = pduSize;
            _optimizer = OptimizerFactory.CreateOptimizer(optimizer);
            ReadDataBlockSize = pduSize - ReadDataHeaderLength;
            if (ReadDataBlockSize <= 0)
                throw new ArgumentException($"PDU size have to be greater then {ReadDataHeaderLength}", "pduSize");
            PlcMetaDataTreePath.CreateAbsolutePath(PlcObjectResolver.RootNodeName);
        }

        ~PlcDataMapper()
        {
            if(_onRead != null)
                _onRead -= _onRead;
            if(_onWrite != null)
                _onWrite -= _onWrite;
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
        public async Task<PlcReadResult[]> ReadAsync(params PlcReadReference[] vars)
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
        public async Task<PlcWriteResult[]> WriteAsync(params PlcWriteReference[] vars)
        {
            var values = vars.ToDictionary(x => $"{x.Mapping}.{x.Variable}", x => x.Value);
            var memoryBuffer = new Dictionary<PlcRawData, byte[]>();
            // determine executions
            var executions = DetermineExecutions(vars);

            KeyValuePair<string,DataPack> UpdatePack(string key, DataPack pack, int dataOffset)
            {
                pack.Data = pack.Data.SubArray(dataOffset, pack.Length);
                return new KeyValuePair<string, DataPack>(key, pack);
            }

            var prepared = executions.SelectMany(execution => execution.Bindings.Where(b => !b.Value.MetaData.IsReadOnly))
                                     .Select(x => UpdatePack(x.Key, new DataPack
                                                    {
                                                        Selector = x.Value.RawData.Selector,
                                                        Offset = x.Value.Offset + x.Value.RawData.Offset,
                                                        Length = x.Value.Size > 0 ? x.Value.Size : 1,
                                                        BitMask = x.Value.Size <= 0 ? Converter.SetBit(0, x.Value.MetaData.Offset.Bits, true) : (byte)0,
                                                        Data = GetOrCreateBufferAndApplyValue(
                                                                                                x.Value, 
                                                                                                memoryBuffer, 
                                                                                                values[x.Key])
                                                    }, x.Value.Offset))
                                            .ToDictionary(x => x.Key, x => x.Value);

            await WriteToPlc(prepared.Values);

            executions.ForEach(exec => exec.Invalidate());

            return prepared.Select(x => new PlcWriteResult
                                        {
                                            Address = x.Key,
                                            ActionResult = x.Value.ExecutionResult
                                        }).ToArray();
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
        public Subscription CreateSubscription()
        {
            var sub = new Subscription(this);
            _subscriptions.Add(sub);
            return sub;
        }




        #region internal read write operations

        internal bool RemoveSubscription(Subscription sub)
        {
            return _subscriptions.Remove(sub);
        }


        internal async Task ReadFromPlc(Dictionary<Execution, DataPack> needUpdate)
        {
            // read outdated
            await _onRead(needUpdate.Values);
        }

        internal async Task WriteToPlc(IEnumerable<DataPack> packs)
        {
            await _onWrite(packs);
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


        internal PlcReadResult[] CreatePlcReadResults(IEnumerable<Execution> executions, Dictionary<Execution, 
                                                      DataPack> needUpdate,
                                                      DateTime? changedAfter = null,
                                                      Func<IEnumerable<KeyValuePair<string, PlcObjectBinding>>, IEnumerable<KeyValuePair<string, 
                                                      PlcObjectBinding>>> filter = null)
        {
            if (filter == null)
            {
                filter = (x) => x;
            }
            return executions.Select(exec => needUpdate.TryGetValue(exec, out var pack) ? exec.ApplyDataPack(pack) : exec)
                             .Where(exec => changedAfter == null || exec.LastChange > changedAfter) // filter by data area
                             .GroupBy(exec => exec.ExecutionResult) // Group by execution result
                             .SelectMany(group => filter(group.SelectMany(g => g.Bindings))
                                                       .Select(b => new PlcReadResult
                                                       {
                                                           Address = b.Key,
                                                           Value = b.Value?.ConvertFromRaw(b.Value.RawData.ReadDataCache),  // TODO: Change
                                                           ActionResult = group.Key
                                                       })).ToArray();
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


        private byte[] GetOrCreateBufferAndApplyValue(PlcObjectBinding binding, Dictionary<PlcRawData, byte[]> dict, object value)
        {
            if (!dict.TryGetValue(binding.RawData, out var buffer))
            {
                buffer = new byte[binding.RawData.MemoryAllocationSize];
                dict.Add(binding.RawData, buffer);
            }
            binding.ConvertToRaw(value, buffer);
            return buffer;
        }


        #endregion

    }
}
