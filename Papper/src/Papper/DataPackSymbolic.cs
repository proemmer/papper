using System;

namespace Papper
{
    public class DataPackSymbolic : DataPack
    {
        public string? SymbolicName { get; set; }

        public Type? Type { get; protected set; }
        public object? Value { get; internal set; }

        public override DataPack ApplyResult<T>(ExecutionResult result, T value)
        {
            if (value != null)
            {
                Value = value;
                Type = typeof(T);
                Timestamp = DateTime.Now;
                ExecutionResult = result;
            }
            else
            {
                ExecutionResult = result == ExecutionResult.Ok ? ExecutionResult.InvalidData : result;
            }
            return this;
        }

        public override string ToString() => SymbolicName ?? string.Empty;

    }
}
