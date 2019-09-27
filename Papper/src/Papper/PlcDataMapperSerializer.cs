using System;

namespace Papper
{
    public class PlcDataMapperSerializer
    {
        private readonly MappingEntryProvider _mappingEntryProvider;

        public PlcDataMapperSerializer(MappingEntryProvider mappingEntryProvider = null) => _mappingEntryProvider = mappingEntryProvider ?? new MappingEntryProvider();


        /// <summary>
        /// Converts a data type to a plc known format.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] Serialize<T>(T data)
            => Serialize(typeof(T), data);

        /// <summary>
        /// Converts a data type to a plc known format.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] Serialize(Type type, object data)
        {
            var binding = _mappingEntryProvider.GetMappingEntryForType(type, data).BaseBinding;
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
            => (T)Deserialize(typeof(T), data);

        public object Deserialize(Type t, byte[] data)
        {
            var binding = _mappingEntryProvider.GetMappingEntryForType(t).BaseBinding;
            if (data.Length < binding.Size) ExceptionThrowHelper.ThrowArgumentOutOfRangeException(nameof(data));
            return binding.ConvertFromRaw(data);
        }


        /// <summary>
        /// Returns the size in bytes of the given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public int SerializedByteSize<T>() => _mappingEntryProvider.GetMappingEntryForType(typeof(T)).PlcObject.ByteSize;


        /// <summary>
        /// Returns the size in bytes of the given type.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public int SerializedByteSize(Type t) => _mappingEntryProvider.GetMappingEntryForType(t).PlcObject.ByteSize;




    }
}
