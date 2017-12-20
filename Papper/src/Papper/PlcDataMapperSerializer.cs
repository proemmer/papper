using Papper.Common;
using Papper.Helper;
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
            binding.RawData.Data = new byte[binding.Size];
            binding.ConvertToRaw(data);
            return binding.RawData.Data;
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
            binding.RawData.Data = data;
            return (T)binding.ConvertFromRaw();
        }

        /// <summary>
        /// Returns the size in bytes of the given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public int SerializedByteSize<T>()
        {
            var t = typeof(T);
            var plcObject = PlcObjectResolver.GetMapping(t.Name, _tree, t, true);
            return plcObject.ByteSize;
        }


        private PlcObjectBinding GetTypeForConversion(Type type)
        {
            var plcObject = PlcObjectResolver.GetMapping(type.Name, _tree, type, true);
            return new PlcObjectBinding(new PlcRawData(plcObject.ByteSize), plcObject, 0, 0, true);
        }
    }
}
