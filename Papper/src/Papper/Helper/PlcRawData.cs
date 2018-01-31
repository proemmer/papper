using System;
using System.Collections.Generic;
using System.Linq;
using Papper.Types;

namespace Papper.Helper
{
    internal class PlcRawData
    {
        private byte[] _data;
        private readonly int _partitionSize;
        private readonly int _readDataBlockSize;
        private DateTime _lastUpdate;
        public IDictionary<string, Tuple<int, PlcObject>> References { get; private set; }
        private Dictionary<int,Partiton> Partitons { get; set; }
        public string Selector { get; set; }
        public int Offset { get; set; }
        public int Size { get; set; }
        

        public PlcRawData(int readDataBlockSize)
        {
            _readDataBlockSize = readDataBlockSize;
            _partitionSize = _readDataBlockSize;
            References = new Dictionary<string, Tuple<int, PlcObject>>();
            Partitons = new Dictionary<int, Partiton>();
        }

        public byte[] Data
        {
            get { return _data; }
            set
            {
                if (_data != value)
                {
                    _data = value;
                    CreatePartitions();
                }
            }
        }

        public DateTime LastUpdate
        {
            get { return _lastUpdate; }
            set
            {
                _lastUpdate = value;
                foreach (var p in Partitons)
                {
                    p.Value.LastUpdate = value;
                }
            }
        }

        public void AddReference(string name, int offset, PlcObject plcObject)
        {
            if (!References.ContainsKey(name))
            {
                var item = new KeyValuePair<string, Tuple<int, PlcObject>>(name, new Tuple<int, PlcObject>(offset, plcObject));
                References.Add(item);
            }
        }

        public IList<Partiton> GetPartitonsByOffset(IEnumerable<Tuple<int,int>> offsets)
        {
            var partitions = new List<Partiton>();
            foreach (var item in offsets)
            {
                var part = GetPartitonsByOffset(item.Item1, item.Item2);
                foreach (var p in part)
                {
                    if (!partitions.Contains(p))
                        partitions.Add(p);
                }
            }
            return partitions;
        }


        public IList<Partiton> GetPartitonsByOffset(int offset, int size)
        {
            if (Data != null && Data.Length > _partitionSize && size != Data.Length)
            {
                var partitions = new List<Partiton>();
                var index = offset;
                do
                {
                    var partitionId = index / _partitionSize;
                    if (Partitons.TryGetValue(partitionId, out Partiton partiton) && !partitions.Contains(partiton))
                    {
                        partitions.Add(partiton);
                        var off = (partiton.Size - offset);
                        if (off > 0)
                        {
                            size -= off;
                            index += off;
                            offset = 0;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                } while (size > 0);
                return partitions;
            }

            return Partitons.Values.ToList();
        }

        private void CreatePartitions()
        {
            var length = Data.Length;
            var numberOfPartitons = Data.Length/ _partitionSize;
            if ((length % _partitionSize) > 0)
                numberOfPartitons++;

            var offset = 0;
            for (var i = 0; i < numberOfPartitons; i++)
            {
                var size = length >= _partitionSize ? _partitionSize : length;
                Partitons.Add(i, new Partiton
                {
                    LastUpdate = DateTime.MinValue,
                    Offset = offset,
                    Size = size
                });
                offset += size;
                length -= size;
            }
        }
    }
}
