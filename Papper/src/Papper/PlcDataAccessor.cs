using System;

namespace Papper
{
    public class PlcDataAccessor
    {
        private readonly MappingEntryProvider _mappingEntryProvider;

        public PlcDataAccessor(MappingEntryProvider mappingEntryProvider = null)
        {
            _mappingEntryProvider = mappingEntryProvider ?? new MappingEntryProvider();
        }

        /// <summary>
        /// Get a value from the structs binary data
        /// </summary>
        /// <typeparam name="TStruct">The struct of the given byte array</typeparam>
        /// <typeparam name="TValue">The type of the value to read</typeparam>
        /// <param name="data"></param>
        /// <param name="variable"></param>
        /// <returns></returns>
        public TValue GetValue<TStruct, TValue>(string variable, Span<byte> data)
        {
            var mappingEntry = _mappingEntryProvider.GetMappingEntryForType(typeof(TStruct));
            _mappingEntryProvider.UpdateVariables(mappingEntry, variable);
            return mappingEntry.Bindings.TryGetValue(variable, out var binding) ? (TValue)binding.ConvertFromRaw(data) : default;
        }


        /// <summary>
        /// Set a value to the structs binary data
        /// </summary>
        /// <typeparam name="TStruct">The struct of the given byte array</typeparam>
        /// <typeparam name="TValue">The type of the value to write</typeparam>
        /// <param name="variable"></param>
        /// <param name="data"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetValue<TStruct, TValue>(string variable, TValue value, Span<byte> data)
        {
            var mappingEntry = _mappingEntryProvider.GetMappingEntryForType(typeof(TStruct));
            _mappingEntryProvider.UpdateVariables(mappingEntry, variable);
            if (mappingEntry.Bindings.TryGetValue(variable, out var binding))
            {
                binding.ConvertToRaw(value, data);
            }
            return false;
        }
    }
}
