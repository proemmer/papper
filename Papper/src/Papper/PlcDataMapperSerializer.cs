using Papper.Internal;
using System;

namespace Papper
{
    public class PlcDataMapperSerializer
    {
        private readonly PlcMetaDataTree _tree = new PlcMetaDataTree();

        /// <summary>
        /// Converts a data type to a plc known format.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] Serialize<T>(T data)
        {
            var binding = GetTypeForConversion(typeof(T));
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
            var binding = GetTypeForConversion(typeof(T));
            if (data.Length < binding.Size)
                throw new ArgumentOutOfRangeException($"{nameof(data)}");
            return (T)binding.ConvertFromRaw(data);
        }


        private PlcObjectBinding GetTypeForConversion(Type type)
        {
            var plcObject = PlcObjectResolver.GetMapping(type.Name, _tree, type, true);
            return new PlcObjectBinding(new PlcRawData(plcObject.ByteSize), plcObject, 0, 0, true);
        }
    }
}
