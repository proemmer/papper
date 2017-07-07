using Papper.Common;
using Papper.Helper;
using Papper.Types;
using System;
using System.Collections.Generic;

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
            binding.RawData.Data = data;
            return (T)binding.ConvertFromRaw();
        }


        private PlcObjectBinding GetTypeForConversion(Type type)
        {
            var plcObject = PlcObjectResolver.GetMapping(type.Name, _tree, type, true);
            return new PlcObjectBinding(new PlcRawData(plcObject.ByteSize), plcObject, 0, 0, true);
        }
    }
}
