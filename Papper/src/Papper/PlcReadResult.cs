﻿using System;

namespace Papper
{
    /// <summary>
    /// This struct represents the result of a read command. It includes the read metadata and 
    /// the <see cref="ExecutionResult"/> of the read operation.
    /// </summary>
    public struct PlcReadResult
    {
        private string _address;
        private object _value;
        private ExecutionResult _executionResult;
        private int _dot;

        /// <summary>
        /// Full address is composed of mapping and variable
        /// </summary>
        public string Address => _address;

        /// <summary>
        /// read value
        /// </summary>
        public object Value => _value;

        /// <summary>
        /// The result of an Executed action
        /// </summary>
        public ExecutionResult ActionResult => _executionResult;

        /// <summary>
        /// mapping part of the address
        /// </summary>
        public string Mapping => Address.Substring(0, _dot);

        /// <summary>
        /// variable part of the address.
        /// </summary>
        public string Variable => Address.Substring(_dot + 1);


        /// <summary>
        /// Create an instance of a <see cref="PlcReadResult"/>
        /// </summary>
        /// <param name="address">The address of a variable. e.g. [Mapping].[Variable]</param>
        /// <param name="value">The read value.</param>
        /// <param name="executionResult">The result itselve. <see cref="ExecutionResult"/></param>
        public PlcReadResult(string address, object value, ExecutionResult executionResult)
        {
            _address = address;
            _value = value;
            _executionResult = executionResult;
            _dot = address.IndexOf(".");
        }

        
        /// <summary>
        /// Determine if this reasul is part of the given mapping
        /// </summary>
        /// <param name="mapping">mapping to test</param>
        /// <returns></returns>
        public bool IsPartOfMapping(string mapping)
        {
            return mapping.AsReadOnlySpan().SequenceEqual(Address.AsReadOnlySpan().Slice(0, Address.IndexOf(".")));
        }

    }
}