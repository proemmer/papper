using System;
using System.Collections.Generic;

namespace Papper
{
    /// <summary>
    /// This structure represents the result of a read command. It includes the read meta data and 
    /// the <see cref="ExecutionResult"/> of the read operation.
    /// </summary>
    public struct PlcReadResult : IEquatable<PlcReadResult>
    {
        private readonly int _dot;

        /// <summary>
        /// Full address is composed of mapping and variable
        /// </summary>
        public string Address { get; }

        /// <summary>
        /// read value
        /// </summary>
        public object? Value { get; }

        /// <summary>
        /// The result of an Executed action
        /// </summary>
        public ExecutionResult ActionResult { get; }

        /// <summary>
        /// mapping part of the address
        /// </summary>
        public string Mapping => _dot == -1 ? Address : Address.Substring(0, _dot);

        /// <summary>
        /// variable part of the address.
        /// </summary>
        public string Variable => _dot == -1 ? string.Empty : Address.Substring(_dot + 1);


        /// <summary>
        /// Create an instance of a <see cref="PlcReadResult"/>
        /// </summary>
        /// <param name="address">The address of a variable. e.g. [Mapping].[Variable]</param>
        /// <param name="value">The read value.</param>
        /// <param name="executionResult">The result itself. <see cref="ExecutionResult"/></param>
        public PlcReadResult(string address, object? value, ExecutionResult executionResult)
        {
            Address = address ?? ExceptionThrowHelper.ThrowArgumentNullException<string>(nameof(address));
            Value = value;
            ActionResult = executionResult;
            _dot = address == null ? -1 : address.IndexOf(".", System.StringComparison.InvariantCulture);
        }


        /// <summary>
        /// Determine if this result is part of the given mapping
        /// </summary>
        /// <param name="mapping">mapping to test</param>
        /// <returns></returns>
        public bool IsPartOfMapping(string mapping) => mapping.AsSpan().SequenceEqual(Address.AsSpan().Slice(0, Address!.IndexOf(".", StringComparison.InvariantCulture)));
        
        public override bool Equals(object? obj) => obj is PlcReadResult result && 
                                                    Address == result.Address && 
                                                    EqualityComparer<object?>.Default.Equals(Value, result.Value) && 
                                                    ActionResult == result.ActionResult && 
                                                    Mapping == result.Mapping && 
                                                    Variable == result.Variable;

        public override int GetHashCode()
        {
            var hashCode = 1249096101;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Address);
            hashCode = hashCode * -1521134295 + EqualityComparer<object?>.Default.GetHashCode(Value);
            hashCode = hashCode * -1521134295 + ActionResult.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Mapping);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Variable);
            return hashCode;
        }

        public static bool operator ==(PlcReadResult left, PlcReadResult right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PlcReadResult left, PlcReadResult right)
        {
            return !(left == right);
        }

        public bool Equals(PlcReadResult other) => Equals(other);
    }
}