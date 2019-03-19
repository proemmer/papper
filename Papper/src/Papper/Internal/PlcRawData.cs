using Papper.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Papper.Internal
{
    internal class PlcRawData
    {
        private readonly object _lock = new object();
        private int _size;
        private Memory<byte> _data = Memory<byte>.Empty;
        private readonly int _partitionSize;
        private readonly int _readDataBlockSize;
        private DateTime _lastUpdate;
        private Dictionary<int, Partiton> _partitons;

        public IDictionary<string, Tuple<int, PlcObject>> References { get; private set; }
        
        public string Selector { get; set; }
        public int Offset { get; set; }
        public int Size
        {
            get => _size;
            set
            {
                _size = value;
                MemoryAllocationSize = CalcRawDataSize(value);
            }
        }

        

        public int MemoryAllocationSize { get; private set; }

        private int CalcRawDataSize(int size)
        {
            size = size > 0 ? size : 2;
            if (size % 2 != 0)
                size++;
            return size;
        }

        public PlcRawData(int readDataBlockSize)
        {
            _readDataBlockSize = MemoryAllocationSize = readDataBlockSize;
            _partitionSize = _readDataBlockSize;
            References = new Dictionary<string, Tuple<int, PlcObject>>();
        }

        public Memory<byte> ReadDataCache
        {
            get => _data;
            set
            {
                _data = value;
                if (_partitons == null || !_partitons.Any())
                {
                    lock (_lock)
                    {
                        if (_partitons == null || !_partitons.Any())
                            CreatePartitions();
                    }
                }
            }
        }

        public DateTime LastUpdate
        {
            get => _lastUpdate;
            set
            {
                _lastUpdate = value;
                if (_partitons == null) return;
                foreach (var p in _partitons)
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

        public IList<Partiton> GetPartitonsByOffset(IEnumerable<Tuple<int, int>> offsets)
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
            if (_partitons == null) return new List<Partiton>();
            if (!ReadDataCache.IsEmpty && ReadDataCache.Length > _partitionSize && size != ReadDataCache.Length)
            {
                var partitions = new List<Partiton>();
                var index = offset;
                do
                {
                    var partitionId = index / _partitionSize;
                    if (_partitons.TryGetValue(partitionId, out var partiton) && !partitions.Contains(partiton))
                    {
                        partitions.Add(partiton);
                        var off = offset > 0 ? ((partiton.Offset + partiton.Size) - offset) : partiton.Size;
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

            return _partitons.Values.ToList();
        }

        private void CreatePartitions()
        {
            if (_partitons == null) _partitons = new Dictionary<int, Partiton>();
            var length = ReadDataCache.Length;
            var numberOfPartitons = ReadDataCache.Length / _partitionSize;
            if ((length % _partitionSize) > 0)
                numberOfPartitons++;

            var offset = 0;
            for (var i = 0; i < numberOfPartitons; i++)
            {
                var size = length >= _partitionSize ? _partitionSize : length;
                _partitons.Add(i, new Partiton
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
