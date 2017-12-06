using Papper.Attributes;
using Papper.Internal;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Papper
{
    public delegate void OnChangeEventHandler(object sender, PlcNotificationEventArgs e);
   
    public enum ExecutionResult
    {
        Unknown,
        Ok,
        Error
    }
    public class DataPack
    {
        public string Selector { get; set; }
        public int Offset { get; set; }
        public int Length { get; set; }

        public byte[] Data { get; private set; }

        public ExecutionResult ExecutionResult { get; set; }


        public void ApplyData(byte[] data)
        {
            Data = data;
        }
    }

    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class PlcDataMapper
    {
        #region Delegates
        /// <summary>
        /// This delegate is used to invoke the read operations
        /// </summary>
        /// <param name="selector">Selector from the MappingAttribute</param>
        /// <param name="offset">Offset in byte to the first byte to read</param>
        /// <param name="length">Number of bytes to read.</param>
        /// <returns></returns>
        public delegate Task ReadOperation(IEnumerable<DataPack> reads);

        /// <summary>
        /// his delegate is used to invoke the write operations.
        /// </summary>
        /// <param name="selector">Selector from the MappingAttribute</param>
        /// <param name="offset">Offset in byte to the first byte to write</param>
        /// <param name="data"></param>
        /// <param name="mask">bit mask, all bits with true will be written </param>
        /// <returns></returns>
        public delegate bool WriteOperation(string selector, int offset, byte[] data, byte bitMask = 0);

        #endregion

        #region Fields
        private HashSet<Subscription> _subscriptions = new HashSet<Subscription>();
        internal enum ReadResult { Successfully, Failed, UseCache }
        private const string ADDRESS_PREFIX = "$ABSSYMBOLS$_";
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
        public async Task<PlcReadResult[]> ReadAsync(params PlcReference[] vars)
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

        internal List<Execution> DetermineExecutions(IEnumerable<PlcReference> vars)
        {
            return vars.GroupBy(x => x.Mapping)
                                .Select((execution) => _mappings.TryGetValue(execution.Key, out IEntry entry) ? (execution, entry) : (null, null))
                                .Where(x => x.execution != null)
                                .SelectMany(x => x.entry.GetOperations(x.execution.SelectMany(exec => exec.Variables)))
                                .ToList(); 
        }

        internal async Task ReadFromPlc(Dictionary<Execution, DataPack> needUpdate)
        {
            // read outdated
            await _onRead(needUpdate.Values);
        }

        internal PlcReadResult[] CreatePlcReadResults(    IEnumerable<Execution> executions, Dictionary<Execution
                                                        , DataPack> needUpdate
                                                        , DateTime? changedAfter = null
                                                        , Func<IEnumerable<KeyValuePair<string, PlcObjectBinding>>, IEnumerable<KeyValuePair<string, PlcObjectBinding>>> filter = null)
        {
            if(filter == null)
            {
                filter = (x) => x; 
            }
            return executions.Select(exec => needUpdate.TryGetValue(exec, out var pack) ? exec.ApplyDataPack(pack) : exec)
                             .Where(exec => changedAfter == null || exec.LastChange > changedAfter) // filter by data area
                             .GroupBy(exec => exec.ExecutionResult)
                             .SelectMany(group => filter(group.SelectMany(g => g.Bindings))
                                                       .Select(b => new PlcReadResult
                                                       {
                                                           Address = b.Key,
                                                           Value = b.Value?.ConvertFromRaw(b.Value.RawData.ReadDataCache),  // TODO: Change
                                                           ActionResult = group.Key
                                                       })).ToArray();
        }

        internal Dictionary<Execution, DataPack> UpdateableItems(List<Execution> executions)
        {
            return executions.Where(exec => exec.ValidationTimeMs <= 0 || exec.PlcRawData.LastUpdate.AddMilliseconds(exec.ValidationTimeMs) < DateTime.Now)
                           .Select(x => new KeyValuePair<Execution, DataPack>(x,
                                                                            new DataPack
                                                                            {
                                                                                Selector = x.PlcRawData.Selector,
                                                                                Offset = x.PlcRawData.Offset,
                                                                                Length = x.PlcRawData.Size > 0 ? x.PlcRawData.Size : 1
                                                                            }))
                           .ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// Read variables from an given address
        /// </summary>
        /// <param name="mapping">mapping name specified in the MappingAttribute</param>
        /// <param name="vars"></param>
        /// <returns>return a dictionary with all variables and the read value</returns>
        //public Dictionary<string, object> ReadAbs(string from, params string[] vars)
        //{
        //    IEntry entry;
        //    var result = new Dictionary<string, object>();
        //    var key = $"$ABSSYMBOLS$_{from}";
        //    using (var upgradeableGuard = new UpgradeableGuard(_mappingsLock))
        //    {
        //        if (!_mappings.TryGetValue(key, out entry))
        //        {
        //            entry = new RawEntry(this, from, ReadDataBlockSize, 0);
        //            using (upgradeableGuard.UpgradeToWriterLock())
        //                _mappings.TryAdd(key, entry);
        //        }
        //    }

        //    if(entry != null)
        //    { 
        //        foreach (var execution in entry.GetOperations(vars))
        //        {
        //            if (ExecuteRead(execution) != ReadResult.Failed)
        //            {
        //                foreach (var item in execution.Bindings)
        //                    result.Add(item.Key, item.Value.ConvertFromRaw());
        //            }
        //        }
        //    }
        //    return result;
        //}

        /// <summary>
        /// Write values to variables of an given mapping
        /// </summary>
        /// <param name="mapping">mapping name specified in the MappingAttribute</param>
        /// <param name="values">variable names and values to write</param>
        /// <returns>return true if all operations are succeeded</returns>
        public bool Write(string mapping, Dictionary<string,object> values)
        {
            if (string.IsNullOrWhiteSpace(mapping))
                throw new ArgumentException("The given argument could not be null or whitespace.", "mapping");

            var result = true;
            if (!_mappings.TryGetValue(mapping, out IEntry entry))
                return result;

            foreach (var execution in entry.GetOperations(values.Keys))
            {
                foreach (var binding in execution.Bindings)
                {
                    if (!binding.Value.MetaData.IsReadOnly)
                    {
                        // temporary workaround
                        var key = binding.Key.Substring(binding.Key.IndexOf(".") + 1);
                        var buffer = new byte[binding.Value.RawData.MemoryAllocationSize];  // TODO create a reusable buffer!!!
                        binding.Value.ConvertToRaw(values[key], buffer);
                        if (!Write(binding.Value, buffer))
                        {
                            Debug.WriteLine($"Error writing {key}");
                            result = false;
                        }
                    }
                    else
                        throw new UnauthorizedAccessException($"You could not write the variable {binding.Key} because you have only read access to it!");
                }

                execution.Invalidate();
            }

            return result;
        }

        ///// <summary>
        ///// Write values to a given address
        ///// </summary>
        ///// <param name="to"></param>
        ///// <param name="values"></param>
        ///// <returns></returns>
        //public bool WriteAbs(string to, Dictionary<string, object> values)
        //{
            
        //    IEntry entry;
        //    var result = true;
        //    var key = $"$ABSSYMBOLS$_{to}";
        //    using (var upgradeableGuard = new UpgradeableGuard(_mappingsLock))
        //    {
        //        if (!_mappings.TryGetValue(key, out entry))
        //        {
        //            entry = new RawEntry(this, to, ReadDataBlockSize, 0);
        //            using (upgradeableGuard.UpgradeToWriterLock())
        //                _mappings.TryAdd(key, entry);
        //        }
        //    }

        //    if (entry != null)
        //    {
        //        foreach (var execution in entry.GetOperations(values.Keys))
        //        {
        //            foreach (var binding in execution.Bindings)
        //            {
        //                if (!binding.Value.MetaData.IsReadOnly)
        //                {
        //                    lock (binding.Value.RawData)
        //                    {
        //                        binding.Value.ConvertToRaw(values[binding.Key], binding.Value.RawData.ReadDataCache);
        //                        if (!Write(binding.Value))
        //                        {
        //                            Debug.WriteLine($"Error writing {binding.Key}");
        //                            result = false;
        //                        }
        //                    }
        //                }
        //                else
        //                    throw new UnauthorizedAccessException($"You could not write the variable {binding.Key} because you have only read access to it!");
        //            }
        //        }
        //    }
        //    return result;
        //}

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


        #region internal read write operations

        private bool Write(PlcObjectBinding binding, byte[] data)
        {
            if (_onWrite == null)
                throw new NullReferenceException("The event handler for the write method is not registered.");


            var rawData = binding.RawData;
            var offset = rawData.Offset + binding.Offset;
            if (binding.Size != 0)
            {
                var size = binding.Size;
                return _onWrite(rawData.Selector, offset, data.SubArray(binding.Offset, size));
            }

            return _onWrite(rawData.Selector, offset, data.SubArray(binding.Offset, 1), Converter.SetBit(0, binding.MetaData.Offset.Bits, true));

        }

        #endregion










        public Subscription CreateSubscribe()
        {
            var sub = new Subscription(this);
            _subscriptions.Add(sub);
            return sub;
        }

        internal bool RemoveSubscription(Subscription sub)
        {
            return _subscriptions.Remove(sub);
        }

    }
}
