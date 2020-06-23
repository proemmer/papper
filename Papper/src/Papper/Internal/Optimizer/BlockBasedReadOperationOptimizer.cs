using Papper.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Papper.Internal
{




    internal class BlockBasedReadOperationOptimizer : IReadOperationOptimizer
    {


        /// <summary>
        /// Create RawReadOperations to use it in the reader. This method tries to optimize the PLC access, 
        /// to it creates blocks for minimum operations on plc.
        /// </summary>
        public IEnumerable<PlcRawData> CreateRawReadOperations(string selector, IEnumerable<KeyValuePair<string, OperationItem>> objects, int readDataBlockSize)
        {
            var rawBlocks = new List<PlcRawData>();
            PlcRawData? pred = null;
            var offsetCountedAsBoolean = -1;
            var offset = 0;
            foreach (var item in objects.OrderBy(i => i.Value.Offset + i.Value.PlcObject.Offset.Bytes)
                                        .ThenBy(i => i.Value.PlcObject.Offset.Bits)
                                        .ThenBy(i => i.Key.Length)
                                        .ToList())
            {
                var count = true;
                var currentOffset = item.Value.Offset + item.Value.PlcObject.Offset.Bytes;
                var sizeInBytes = item.Value.PlcObject.Size == null ? 0 : item.Value.PlcObject.Size.Bytes;

                if (item.Value.PlcObject is PlcBool)
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
                if (item.Value.PlcObject is PlcArray arr && arr.Size.Bits > 0)
                {
                    sizeInBytes += 1;
                }

                var current = new PlcRawData(readDataBlockSize)
                {
                    Offset = currentOffset,
                    Size = sizeInBytes == 0 && count ? 1 : sizeInBytes,
                    Selector = selector,
                    ContainsReadOnlyParts = item.Value.PlcObject.IsReadOnly || item.Value.PlcObject.HasReadOnlyChilds
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
                        if (current.ContainsReadOnlyParts)
                        {
                            pred.ContainsReadOnlyParts = true;
                        }
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
                        if (current.ContainsReadOnlyParts)
                        {
                            pred.ContainsReadOnlyParts = true;
                        }
                    }
                    else
                    {
                        var unusedBytes = current.Offset - directOffset;
                        var modifiedSize = pred.Size + unusedBytes + current.Size;
                        if (pred.Size + unusedBytes + current.Size <= readDataBlockSize)
                        {
                            //It is OK to use one block, because its in one PDU. Better one read than many
                            if (count)
                            {
                                offset = pred.Size + unusedBytes;
                                pred.Size = modifiedSize;
                            }
                            if (current.ContainsReadOnlyParts)
                            {
                                pred.ContainsReadOnlyParts = true;
                            }
                        }
                        else
                        {
                            rawBlocks.Add(current);
                            pred = current;
                            offset = 0;
                        }
                    }
                }

                pred.AddReference(item.Key, offset, item.Value.PlcObject);
            }

            OptimizerFactory.CreateWriteAreas(rawBlocks);
            return rawBlocks;
        }

        
    }
}