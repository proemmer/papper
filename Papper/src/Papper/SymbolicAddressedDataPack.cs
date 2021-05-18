using System;

namespace Papper
{
    public class SymbolicAddressedDataPack : DataPack
    {
        public string? SymbolicName { get; set; }

        public Type Type { get; set; }
        public object? Value { get; internal set; }

        public override DataPack ApplyData<T>(T data)
        {
            Value = data;
            return this;
        }

        public override string ToString() => SymbolicName ?? string.Empty;

    }
}
