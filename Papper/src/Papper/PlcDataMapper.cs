﻿using Papper.Attributes;
using Papper.Common;
using Papper.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

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
        /// <param name="selector">Selector from the MappingAttribute</param>
        /// <param name="offset">Offset in byte to the first byte to read</param>
        /// <param name="length">Number of bytes to read.</param>
        /// <returns></returns>
        public delegate byte[] ReadOperation(string selector, int offset, int length);

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
        private const int PduSizeDefault = 480;
        private const int ReadDataHeaderLength = 18;
        private readonly PlcMetaDataTree _tree = new PlcMetaDataTree();
        private readonly IDictionary<string, IEntry> _mappings = new Dictionary<string, IEntry>();
        private event ReadOperation _onRead;
        private event WriteOperation _onWrite;
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
        #endregion

        public PlcDataMapper(int pduSize = PduSizeDefault)
        {
            PduSize = pduSize;
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
            IEntry entry;
            var result = new List<string>();
            if (_mappings.TryGetValue(mapping, out entry))
                return PlcObjectResolver.GetLeafs(entry.PlcObject, result);
            throw new KeyNotFoundException($"The mapping {mapping} does not exist.");
        }

        /// <summary>
        /// Add a type with an MappingAttribute to register this type as an mapping for read and write operations
        /// </summary>
        /// <param name="type">Has to be a type with at least one an MappingAttribute</param>
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
                IEntry existingMapping;
                if (_mappings.TryGetValue(mapping.Name, out existingMapping))
                {
                    var mappingEntry = existingMapping as MappingEntry;
                    return mappingEntry.Mapping == mapping && mappingEntry.Type == type;
                }
                _mappings.Add(mapping.Name, new MappingEntry(mapping, type, _tree, ReadDataBlockSize, mapping.ObservationRate));
            }
            return true;
        }

        /// <summary>
        /// Read variables from an given mapping
        /// </summary>
        /// <param name="mapping">mapping name specified in the MappingAttribute</param>
        /// <param name="vars"></param>
        /// <returns>return a dictionary with all variables and the red value</returns>
        public Dictionary<string, object> Read(string mapping, params string[] vars)
        {
            if (string.IsNullOrWhiteSpace(mapping))
                throw new ArgumentException("The given argument could not be null or whitespace.", "mapping");

            IEntry entry;
            var result = new Dictionary<string, object>();
            if(_mappings.TryGetValue(mapping, out entry))
            {
                foreach (var execution in entry.GetOperations(vars))
                {
                    if (ExecuteRead(execution))
                    {
                        foreach (var item in execution.Bindings)
                            result.Add(item.Key, item.Value.ConvertFromRaw());
                    } 
                }
            }
            return result;
        }

        /// <summary>
        /// Read variables from an given mapping
        /// </summary>
        /// <param name="mapping">mapping name specified in the MappingAttribute</param>
        /// <param name="vars"></param>
        /// <returns>return a dictionary with all variables and the red value</returns>
        public Dictionary<string, object> ReadAbs(string from, params string[] vars)
        {
            IEntry entry;
            var result = new Dictionary<string, object>();
            var key = $"$ABSSYMBOLS$_{from}";
            if (!_mappings.TryGetValue(key, out entry))
            {
                entry = new RawEntry(from, _tree, ReadDataBlockSize, 0);
                _mappings.Add(key, entry);
            }

            if(entry != null)
            { 
                foreach (var execution in entry.GetOperations(vars))
                {
                    if (ExecuteRead(execution))
                    {
                        foreach (var item in execution.Bindings)
                            result.Add(item.Key, item.Value.ConvertFromRaw());
                    }
                }
            }
            return result;
        }

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

            IEntry entry;
            var result = true;
            if (_mappings.TryGetValue(mapping, out entry))
            {
                foreach (var execution in entry.GetOperations(values.Keys.ToArray()))
                {
                    foreach (var binding in execution.Bindings)
                    {
                        if (!binding.Value.MetaData.IsReadOnly)
                        {
                            lock (binding.Value.RawData)
                            {
                                binding.Value.ConvertToRaw(values[binding.Key]);
                                if (!Write(binding.Value))
                                {
                                    Debug.WriteLine($"Error writing {binding.Key}");
                                    result = false;
                                }
                            }
                        }
                        else
                            throw new UnauthorizedAccessException($"You could not write the variable {binding.Key} because you have only read access to it!");
                    }
                }
            }
            return result;
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

            IEntry entry;
            var result = new Dictionary<string, object>();
            if (_mappings.TryGetValue(mapping, out entry))
            {
                Tuple<int, Types.PlcObject> varibleEntry;
                if (entry.Variables.TryGetValue(variable, out varibleEntry))
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

        private bool ExecuteRead(Execution exec)
        {
            if (exec.Partitions != null)
            {
                if (exec.Partitions.Min(x => x.LastUpdate).AddMilliseconds(exec.ValidationTimeMs) < DateTime.Now)
                    return ReadPartitions(exec.PlcRawData, exec.Partitions);
            }
            else if (exec.PlcRawData.LastUpdate.AddMilliseconds(exec.ValidationTimeMs) < DateTime.Now)
                return Read(exec.PlcRawData);
            return false;
        }

        private bool Read(PlcRawData rawData)
        {
            lock (rawData)
            {
                var size = rawData.Size > 0 ? rawData.Size : 1;
                if (Read(rawData.Selector, rawData.Offset, size, rawData.Data))
                {
                    rawData.LastUpdate = DateTime.Now;
                    return true;
                }
                return false;
            }
        }

        private bool ReadPartitions(PlcRawData rawData, IEnumerable<Partiton> partitons)
        {
            lock (rawData)
            {
                try
                {
                    var startPartition = partitons.First();
                    var offset = rawData.Offset + startPartition.Offset;
                    var readSize = partitons.Sum(x => x.Size);

                    Partiton prev = null;
                    var blocks = new List<Tuple<int, int>>();
                    foreach (var part in partitons)
                    {
                        if (prev != null)
                        {
                            var pos = prev.Offset + prev.Size;
                            if (pos != part.Offset)
                            {
                                blocks.Add(new Tuple<int, int>(offset, pos));
                                prev = null;
                            }
                            else
                                prev = part;
                        }
                        else
                        {
                            offset = rawData.Offset + part.Offset; // offset of block
                            prev = part;
                        }
                    }
                    if (prev != null)
                        blocks.Add(new Tuple<int, int>(offset, prev.Offset + prev.Size));

                    var targetOffset = startPartition.Offset;
                    foreach (var item in blocks)
                    {
                        if (Read(rawData.Selector, item.Item1, item.Item2, rawData.Data, targetOffset))
                        {
                            foreach (var partiton in partitons)
                                partiton.LastUpdate = DateTime.Now;
                        }
                    }
                }
                catch (Exception)
                {
                    return false;
                }
                return true;
            }
        }

        private bool Read(string selector, int offset, int length, byte[] data, int targetOffset = 0)
        {
            if (_onRead == null)
                throw new NullReferenceException("The event handler for the read method is not registered.");

            try
            {
                byte[] red = _onRead(selector,  offset, length);
                if (red != null)
                    red.CopyTo(data, targetOffset);
                else
                    return false;
            }
            catch(Exception)
            {
                return false;
            }
            return true;
        }

        private bool Write(PlcObjectBinding binding)
        {
            if (_onWrite == null)
                throw new NullReferenceException("The event handler for the write method is not registered.");

            lock (binding.RawData)
            {
                var rawData = binding.RawData;
                var offset = rawData.Offset + binding.Offset;
                if (binding.Size != 0)
                {
                    var size = binding.Size;
                    var data = rawData.Data.SubArray(binding.Offset, size);
                    return _onWrite(rawData.Selector, offset, data);
                }

                var bitData = rawData.Data.SubArray(binding.Offset,1);
                return _onWrite(rawData.Selector, offset, bitData, Converter.SetBit(0, binding.MetaData.Offset.Bits, true));
            }
        }

        #endregion

    }
}
