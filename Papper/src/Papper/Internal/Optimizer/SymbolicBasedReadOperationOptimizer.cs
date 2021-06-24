using Papper.Types;
using System.Collections.Generic;
using System.Linq;

namespace Papper.Internal
{
    internal class SymbolicBasedReadOperationOptimizer : IReadOperationOptimizer
    {
        public bool SymbolicAccess => true;

        /// <summary>
        /// Create RawReadOperations to use it in the reader. This method tries to optimize the PLC access, 
        /// to it creates blocks for minimum operations on plc.
        /// </summary>
        public IEnumerable<PlcRawData> CreateRawReadOperations(string name, string selector, IEnumerable<KeyValuePair<string, OperationItem>> objects, int readDataBlockSize)
        {
            var rawBlocks = new List<PlcRawData>();
            PlcRawData? pred = null;
            var predIsBool = false;
            var offsetCountedAsBoolean = -1;
            var offset = 0;
            foreach (var item in objects.OrderBy(i => i.Value.Offset + i.Value.PlcObject.Offset.Bytes)
                                        .ThenBy(i => i.Value.PlcObject.Offset.Bits)
                                        .ThenBy(i => i.Key.Length).ToList())
            {
                var count = true;
                var currentIsBool = false;
                var currentOffset = item.Value.Offset + item.Value.PlcObject.Offset.Bytes;
                var sizeInBytes = item.Value.PlcObject.Size == null || item.Value.PlcObject.Size.Bytes == 0 && count ? 1 : item.Value.PlcObject.Size.Bytes;

                if (item.Value.PlcObject is PlcBool)
                {
                    currentIsBool = true;
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
                    Size = sizeInBytes,
                    Selector = selector,
                    ContainsReadOnlyParts = item.Value.PlcObject.IsReadOnly || item.Value.PlcObject.HasReadOnlyChilds,
                    SymbolicAccessName = $"{name}{item.Value.SymbolicPath}"
                };


                if (pred == null || predIsBool) // bools can not have childs in symbolic access
                {
                    rawBlocks.Add(current);
                    pred = current;
                    predIsBool = currentIsBool;
                    offset = 0;
                }
                else
                {
                    var directOffset = pred.Offset + pred.Size;
                    var newElementSize = pred.Size + current.Size;
                    if (directOffset > current.Offset)  // TODO: we have to check if the subelemenbt can be extrated from the parent 
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
                        rawBlocks.Add(current);
                        pred = current;
                        predIsBool = currentIsBool;
                        offset = 0;
                    }
                }


                pred.AddReference(item.Key, offset, item.Value.PlcObject);
            }

            OptimizerFactory.CreateWriteAreas(rawBlocks);
            return rawBlocks;
        }
    }
}
