using System;

namespace Papper
{
    public class PlcDataMapperSerializer
    {
        private readonly MappingEntryProvider _mappingEntryProvider;

        public PlcDataMapperSerializer(MappingEntryProvider mappingEntryProvider = null)
        {
            _mappingEntryProvider = mappingEntryProvider ?? new MappingEntryProvider();
        }


        /// <summary>
        /// Converts a data type to a plc known format.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] Serialize<T>(T data)
        {
            var binding = _mappingEntryProvider.GetMappingEntryForType(typeof(T)).BaseBinding;
            var buffer = new byte[binding.RawData.MemoryAllocationSize];  // TODO handle a reusable buffer
            binding.ConvertToRaw(data, buffer);
            return buffer;
        }

        /// <summary>
        /// Converts a plc known format to a datatype
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public T Deserialize<T>(byte[] data)
        {
            var binding = _mappingEntryProvider.GetMappingEntryForType(typeof(T)).BaseBinding;
            if (data.Length < binding.Size) ThrowArgumentOutOfRangeException(nameof(data));
            return (T)binding.ConvertFromRaw(data);
        }


        /// <summary>
        /// Returns the size in bytes of the given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public int SerializedByteSize<T>() => _mappingEntryProvider.GetMappingEntryForType(typeof(T)).PlcObject.ByteSize;


        /// <summary>
        /// Throw helper
        /// </summary>
        /// <param name="argumentName"></param>
        private void ThrowArgumentOutOfRangeException(string argumentName) => throw new ArgumentOutOfRangeException(argumentName);

    }
}
