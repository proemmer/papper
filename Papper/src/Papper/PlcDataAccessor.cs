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
        /// Get a value from the structure binary data
        /// </summary>
        /// <typeparam name="TStruct">The structure of the given byte array</typeparam>
        /// <typeparam name="TValue">The type of the value to read</typeparam>
        /// <param name="data"></param>
        /// <param name="variable"></param>
        /// <returns></returns>
        public TValue GetValue<TStruct, TValue>(string variable, Span<byte> data)
            => GetValue<TValue>(typeof(TStruct), variable, data);

        /// <summary>
        /// Get a value from the structure binary data
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="type"></param>
        /// <param name="variable"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public TValue GetValue<TValue>(Type type, string variable, Span<byte> data)
        {
            var mappingEntry = _mappingEntryProvider.GetMappingEntryForType(type);
            _mappingEntryProvider.UpdateVariables(mappingEntry, variable);
            return mappingEntry.Bindings.TryGetValue(variable, out var binding) ? (TValue)binding.ConvertFromRaw(data) : default;
        }


        /// <summary>
        /// Set a value to the structure binary data
        /// </summary>
        /// <typeparam name="TStruct">The structure of the given byte array</typeparam>
        /// <typeparam name="TValue">The type of the value to write</typeparam>
        /// <param name="variable"></param>
        /// <param name="data"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetValue<TStruct, TValue>(string variable, TValue value, Span<byte> data)
            => SetValue(typeof(TStruct), variable, value, data);

        /// <summary>
        /// Set a value to the structure binary data
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="type"></param>
        /// <param name="variable"></param>
        /// <param name="value"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool SetValue<TValue>(Type type, string variable, TValue value, Span<byte> data)
        {
            var mappingEntry = _mappingEntryProvider.GetMappingEntryForType(type);
            _mappingEntryProvider.UpdateVariables(mappingEntry, variable);
            if (mappingEntry.Bindings.TryGetValue(variable, out var binding))
            {
                binding.ConvertToRaw(value, data);
                return true;
            }
            return false;
        }
    }
}
