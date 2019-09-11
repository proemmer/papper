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
        public IEnumerable<PlcRawData> CreateRawReadOperations(string selector, IEnumerable<KeyValuePair<string, Tuple<int, PlcObject>>> objects, int readDataBlockSize)
        {
            var rawBlocks = new List<PlcRawData>();
            PlcRawData pred = null;
            var offsetCountedAsBoolean = -1;
            var offset = 0;
            foreach (var item in objects.OrderBy(i => i.Value.Item1 + i.Value.Item2.Offset.Bytes)
                                        .ThenBy(i => i.Value.Item2.Offset.Bits)
                                        .ThenBy(i => i.Key.Length))
            {
                var count = true;
                var currentOffset = item.Value.Item1 + item.Value.Item2.Offset.Bytes;
                var sizeInBytes = item.Value.Item2.Size.Bytes;

                if (item.Value.Item2 is PlcBool)
                {
                    if (currentOffset != offsetCountedAsBoolean)
                        offsetCountedAsBoolean = currentOffset;
                    else
                        count = false;
                }

                var current = new PlcRawData(readDataBlockSize)
                {
                    Offset = currentOffset,
                    Size = sizeInBytes == 0 && count ? 1 : sizeInBytes,
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


                pred.AddReference(item.Key, offset, item.Value.Item2);


                //if (item.Value.Item2 is PlcArray array)
                //{
                //    HandleArray(offset, array, item, pred);
                //}
            }

            return rawBlocks;
        }

        /// <summary>
        /// Create Raw Read operations for Array Elements
        /// </summary>
        private void HandleArray(int offset, PlcArray array, KeyValuePair<string, Tuple<int, PlcObject>> item, PlcRawData pred, string dimension = "")
        {
            var arrayOffset = offset;
            for (var i = array.From; i <= array.To; i++)
            {
                var index = $"[{i}]";
                var element = array.ArrayType is PlcStruct
                    ? new PlcObjectRef(index, array.ArrayType)
                    : PlcObjectFactory.CreatePlcObjectForArrayIndex(array.ArrayType, i, array.From);

                var name = $"{item.Key}{index}";
                var elemName = string.IsNullOrWhiteSpace(dimension) ? name :  $"{dimension}{index}";
                if (element is PlcArray)
                {
                    
                    pred.AddReference(elemName, arrayOffset, element);
                    HandleArray(arrayOffset, element as PlcArray, item, pred, name);
                    arrayOffset += array.ArrayType.Size.Bytes;
                }
                else
                {
                    pred.AddReference(elemName, arrayOffset, element);
                    arrayOffset += array.ArrayType.Size.Bytes;
                }
            }
        }

    }
}
