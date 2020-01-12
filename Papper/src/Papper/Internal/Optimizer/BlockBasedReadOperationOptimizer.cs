﻿using Papper.Types;
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
        public IEnumerable<PlcRawData> CreateRawReadOperations(string selector, IEnumerable<KeyValuePair<string, Tuple<int, PlcObject>>> objects, int readDataBlockSize)
        {
            var rawBlocks = new List<PlcRawData>();
            PlcRawData? pred = null;
            var offsetCountedAsBoolean = -1;
            var offset = 0;
            foreach (var item in objects.OrderBy(i => i.Value.Item1 + i.Value.Item2.Offset.Bytes)
                                        .ThenBy(i => i.Value.Item2.Offset.Bits)
                                        .ThenBy(i => i.Key.Length)
                                        .ToList())
            {
                CalculateRawBlocks(selector, item.Key, readDataBlockSize, rawBlocks, ref pred, ref offsetCountedAsBoolean, ref offset, item.Value.Item1, item.Value.Item2);
            }

            return rawBlocks;
        }

        private static void CalculateRawBlocks(string selector, string itemName, int readDataBlockSize, List<PlcRawData> rawBlocks, ref PlcRawData? pred, ref int offsetCountedAsBoolean, ref int offset, int baseOffset, PlcObject item)
        {
            var currentOffset = baseOffset + item.Offset.Bytes;
            if (item.HasReadOnlyChilds)
            {
                foreach (var child in item.ChildVars.OfType<PlcObject>())
                {
                    CalculateRawBlocks(selector, itemName, readDataBlockSize, rawBlocks, ref pred, ref offsetCountedAsBoolean, ref offset, currentOffset, child);
                }
            }
            else
            {
                var count = true;
                var sizeInBytes = item.Size == null ? 0 : item.Size.Bytes;

                if (item is PlcBool)
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
                if (item is PlcArray arr && arr.Size.Bits > 0)
                {
                    sizeInBytes += 1;
                }

                var current = new PlcRawData(readDataBlockSize)
                {
                    Offset = currentOffset,
                    Size = sizeInBytes == 0 && count ? 1 : sizeInBytes,
                    Selector = selector,
                    IsReadOnly = item.IsReadOnly
                };


                if (pred == null || pred.IsReadOnly != current.IsReadOnly)
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
                        }
                        else
                        {
                            rawBlocks.Add(current);
                            pred = current;
                            offset = 0;
                        }
                    }
                }

                pred?.AddReference(itemName, offset, item);
            }
        }
    }
}
