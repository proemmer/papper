﻿using System;

namespace Papper
{
    public class PlcDataMapperSerializer
    {
        private readonly MappingEntryProvider _mappingEntryProvider;

        public PlcDataMapperSerializer(MappingEntryProvider? mappingEntryProvider = null) => _mappingEntryProvider = mappingEntryProvider ?? new MappingEntryProvider();


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
        public byte[] Serialize(Type type, object? data)
        {
            if (type == null)
            {
                return ExceptionThrowHelper.ThrowArgumentNullException<byte[]>(nameof(type));
            }

            if (data == null)
            {
                return ExceptionThrowHelper.ThrowArgumentNullException<byte[]>(nameof(data));
            }

            var entry = _mappingEntryProvider.GetMappingEntryForType(type, data);
            if (entry == null)
            {
                ExceptionThrowHelper.ThrowMappingAttributeNotFoundForTypeException(type);
            }

            var binding = entry!.BaseBinding;
            var buffer = new byte[binding.RawData.MemoryAllocationSize]; 
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

        public object Deserialize(Type type, byte[] data)
        {
            if (type == null)
            {
                return ExceptionThrowHelper.ThrowArgumentNullException<object>(nameof(type));
            }

            if (data == null)
            {
                return ExceptionThrowHelper.ThrowArgumentNullException<object>(nameof(data));
            }

            var entry = _mappingEntryProvider.GetMappingEntryForType(type);
            if (entry == null)
            {
                ExceptionThrowHelper.ThrowMappingAttributeNotFoundForTypeException(type!);
            }

            var binding = entry!.BaseBinding;
            if (type != typeof(string) && data.Length < binding.Size)
            {
                ExceptionThrowHelper.ThrowArgumentOutOfRangeException(nameof(data));
            }

            return binding.ConvertFromRaw(data);
        }


        /// <summary>
        /// Returns the size in bytes of the given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public int SerializedByteSize<T>()
        {
            if (_mappingEntryProvider == null)
            {
                return 0;
            }

            var type = typeof(T);
            var entry = _mappingEntryProvider.GetMappingEntryForType(typeof(T));
            if (entry == null)
            {
                ExceptionThrowHelper.ThrowMappingAttributeNotFoundForTypeException(type!);
            }

            return entry!.PlcObject.ByteSize;
        }




        /// <summary>
        /// Returns the size in bytes of the given type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public int SerializedByteSize(Type type)
        {
            if (_mappingEntryProvider == null)
            {
                return 0;
            }

            if (type == null)
            {
                return ExceptionThrowHelper.ThrowArgumentNullException<int>(nameof(type));
            }

            var entry = _mappingEntryProvider.GetMappingEntryForType(type!);
            if (entry == null)
            {
                ExceptionThrowHelper.ThrowMappingAttributeNotFoundForTypeException(type!);
            }

            return entry!.PlcObject.ByteSize;
        }




    }
}
