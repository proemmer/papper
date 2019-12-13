using Papper.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Papper.Internal
{
    internal class ItemBasedReadOperationOptimizer : IReadOperationOptimizer
    {


        /// <summary>
        /// Create RawReadOperations to use it in the reader. This method tries to optimize the PLC access, 
        /// to it creates blocks for minimum operations on plc.
        /// </summary>
        public IEnumerable<PlcRawData> CreateRawReadOperations(string selector, IEnumerable<KeyValuePair<string, Tuple<int, PlcObject>>> objects, int readDataBlockSize)
        {
            var rawBlocks = new List<PlcRawData>();
            PlcRawData? pred = null;
            var offsetCountedAsBoolean = -1;
            var offset = 0;
            foreach (var item in objects.OrderBy(i => i.Value.Item1 + i.Value.Item2.Offset.Bytes)
                                        .ThenBy(i => i.Value.Item2.Offset.Bits)
                                        .ThenBy(i => i.Key.Length).ToList())
            {
                var count = true;
                var currentOffset = item.Value.Item1 + item.Value.Item2.Offset.Bytes;
                var sizeInBytes = item.Value.Item2.Size == null || item.Value.Item2.Size.Bytes == 0 && count ? 1 : item.Value.Item2.Size.Bytes;

                if (item.Value.Item2 is PlcBool)
                {
                    if (currentOffset != offsetCountedAsBoolean)
                    {
                        offsetCountedAsBoolean = currentOffset;
                    }
                    else
                    {
                        count = false;
                    }
                }
                if (item.Value.Item2 is PlcArray arr && arr.Size.Bits > 0)
                {
                    sizeInBytes += 1;
                }

                var current = new PlcRawData(readDataBlockSize)
                {
                    Offset = currentOffset,
                    Size = sizeInBytes,
                    Selector = selector
                };


                if (pred == null)
                {
                    rawBlocks.Add(current);
                    pred = current;
                    offset = 0;
                }
                else
                {
                    var directOffset = pred.Offset + pred.Size;
                    if (directOffset == current.Offset)
                    {
                        //follows direct
                        offset = current.Offset - pred.Offset;
                        pred.Size += current.Size;
                    }
                    else if (directOffset > current.Offset)
                    {
                        //is subElement
                        offset = current.Offset - pred.Offset;
                        var freeBytesInParent = pred.Size - offset;
                        if (current.Size > freeBytesInParent)
                        {
                            //Update size if we have an overlapping item
                            pred.Size += (current.Size - freeBytesInParent);
                        }
                    }
                    else
                    {
                        rawBlocks.Add(current);
                        pred = current;
                        offset = 0;
                    }
                }


                pred.AddReference(item.Key, offset, item.Value.Item2);
            }

            return rawBlocks;
        }
    }
}
