using Papper.Helper;
using Papper.Types;
using System;
using System.Collections.Generic;

namespace Papper
{
    public class PlcDataMapperSerializer
    {
        private PlcDataMapper _mapper;
        private string[] VarNames = { "This" };
        private Dictionary<string, Dictionary<string, Tuple<int, PlcObject>>> _plcObjects = new Dictionary<string, Dictionary<string, Tuple<int, PlcObject>>>();

        public PlcDataMapperSerializer(PlcDataMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public byte[] Serialize<T>(T data)
        {
            var binding = GetTypeForConversion(typeof(T));
            binding.RawData.Data = new byte[binding.Size];
            binding.ConvertToRaw(data);
            return binding.RawData.Data;
        }


        public T Deserialize<T>(byte[] data)
        {
            var binding = GetTypeForConversion(typeof(T));
            binding.RawData.Data = data;
            return (T)binding.ConvertFromRaw();
        }


        private PlcObjectBinding GetTypeForConversion(Type type)
        {
            var key = $"PlcDataMapperSerializerExtensions{type.Name}";
            var plcObject = PlcObjectResolver.GetMapping(key, _mapper._tree, type, true);
            return new PlcObjectBinding(new PlcRawData(plcObject.ByteSize), plcObject, 0, 0, true);
            //if(!_plcObjects.TryGetValue(key,out var plcObjectEntry))
            //{
            //    plcObjectEntry = new Dictionary<string, Tuple<int, PlcObject>>();

            //    _plcObjects.Add(key, plcObjectEntry);
            //}
        }
    }
}
