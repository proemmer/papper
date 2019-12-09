using Papper.Internal;
using Papper.Types;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

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
            foreach (var item in objects.OrderBy(i => i.Value.Item1 + i.Value.Item2.Offset.Bytes).ThenBy(i => i.Value.Item2.Offset.Bits).ThenBy(i => i.Key.Length).ToList())
            {
                var count = true;
                if (item.Value.Item2 is PlcBool)
                {
                    var currentOffset = item.Value.Item1 + item.Value.Item2.Offset.Bytes;
                    if (currentOffset != offsetCountedAsBoolean)
                        offsetCountedAsBoolean = item.Value.Item1 + item.Value.Item2.Offset.Bytes;
                    else
                        count = false;
                }

                var current = new PlcRawData(readDataBlockSize)
                {
                    Offset = item.Value.Item1 + item.Value.Item2.Offset.Bytes,
                    Size = item.Value.Item2.Size == null || item.Value.Item2.Size.Bytes == 0 && count ? 1 : item.Value.Item2.Size.Bytes,
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
                var element = array.ArrayType is PlcStruct
                    ? new PlcObjectRef(string.Format(CultureInfo.InvariantCulture, "[{0}]", i), array.ArrayType)
                    : PlcObjectFactory.CreatePlcObjectForArrayIndex(array.ArrayType, i, array.From);

                if (element is PlcArray plcArray)
                {
                    var elemName = string.IsNullOrWhiteSpace(dimension) ? string.Format(CultureInfo.InvariantCulture, "{0}[{1}]", item.Key, i) : dimension + string.Format(CultureInfo.InvariantCulture, "[{0}]", i);
                    pred.AddReference(elemName, arrayOffset, element);
                    HandleArray(arrayOffset, plcArray, item, pred, string.Format(CultureInfo.InvariantCulture, "{0}[{1}]", item.Key, i));
                    arrayOffset += array.ArrayType.Size == null ? 0 : array.ArrayType.Size.Bytes;
                }
                else if(element != null)
                {
                    var elemName = string.IsNullOrWhiteSpace(dimension) ? string.Format(CultureInfo.InvariantCulture, "{0}[{1}]", item.Key, i) : dimension + string.Format(CultureInfo.InvariantCulture, "[{0}]", i);
                    pred.AddReference(elemName, arrayOffset, element);
                    arrayOffset += array.ArrayType.Size == null ? 0 : array.ArrayType.Size.Bytes;
                }
            }
        }

    }
}
