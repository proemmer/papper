using Papper.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Papper.Internal
{


    internal class WriteSlot
    {
        public int Offset { get; set; }
        public int Size { get; set; }
        public byte Mask { get; set; } = 0xFF;


        public override string ToString() => $"{Offset},{Size}#{Mask}";
    }


    internal class PlcRawData
    {
        private int _size;

        public IDictionary<string, Tuple<int, PlcObject>> References { get; private set; } = new Dictionary<string, Tuple<int, PlcObject>>();

        public List<WriteSlot> WriteSlots { get; private set; } = new List<WriteSlot>();

        public string? Selector { get; set; }
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

        public bool ContainsReadOnlyParts { get; internal set; }
        public int MemoryAllocationSize { get; private set; }
        public Memory<byte> ReadDataCache { get; set; } = Memory<byte>.Empty;
        public DateTime LastUpdate { get; set; }

        private static int CalcRawDataSize(int size)
        {
            size = size > 0 ? size : 2;
            if (size % 2 != 0)
            {
                size++;
            }

            return size;
        }

        public PlcRawData(int readDataBlockSize)
        {
            MemoryAllocationSize = readDataBlockSize;
        }



        public void AddReference(string name, int offset, PlcObject plcObject)
        {
            if (!References.ContainsKey(name))
            {
                var item = new KeyValuePair<string, Tuple<int, PlcObject>>(name, new Tuple<int, PlcObject>(offset, plcObject));
                References.Add(item);
            }
        }

        public void AddWriteSlot(int offset, int size, byte mask = 0xFF)
        {
            var lastSlot = WriteSlots.LastOrDefault();
            if (lastSlot == null || offset >= (lastSlot.Offset + lastSlot.Size))
            {
                WriteSlots.Add(new WriteSlot
                {
                    Offset = offset,
                    Size = size,
                    Mask = mask
                });
            }
        }
    }
}
