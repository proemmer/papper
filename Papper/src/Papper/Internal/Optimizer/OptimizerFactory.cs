using Papper.Types;
using System.Collections.Generic;
using System.Linq;

namespace Papper.Internal
{
    internal static class OptimizerFactory
    {
        public static IReadOperationOptimizer CreateOptimizer(OptimizerType type)
        {
            switch (type)
            {
                case OptimizerType.Block:
                    return new BlockBasedReadOperationOptimizer();
                case OptimizerType.Items:
                    return new ItemBasedReadOperationOptimizer();
                default:
                    ExceptionThrowHelper.ThrowUnknownOptimizrException(type);
                    return new ItemBasedReadOperationOptimizer(); // will not be called because of exception throwing
            }

        }

        internal static void CreateWriteAreas(List<PlcRawData> rawBlocks)
        {
            foreach (var rawData in rawBlocks)
            {
                foreach (var item in rawData.References)
                {
                    AddWritableAreas(item.Value.Item2, rawData);
                }
            }
        }



        private static void AddWritableAreas(PlcObject item, PlcRawData rawData)
        {
            if (item.HasReadOnlyChilds)
            {
                static void HandleChilds(PlcObject plcObj, PlcRawData rd, int baseOffset)
                {
                    var offset = 0;
                    var itemsToAdd = false;
                    byte bitMask = 0xff;
                    var bitMaskOffset = -1;
                    var inReadOnlyArea = false;
                    PlcObject? pred = null;
                    WorkOnChild(rd, baseOffset, ref inReadOnlyArea, ref offset, ref itemsToAdd, ref bitMask, ref bitMaskOffset, ref pred, plcObj);

                    if (bitMaskOffset != -1)
                    {
                        rd.AddWriteSlot(bitMaskOffset, 1, bitMask);
                    }
                    else if (itemsToAdd)
                    {
                        var size = plcObj.ByteSize - offset;
                        if (size > 0)
                        {
                            rd.AddWriteSlot(offset, size);
                        }
                    }
                }

                HandleChilds(item, rawData, rawData.Offset);
            }
            else if (!item.IsReadOnly)
            {
                rawData.AddWriteSlot(rawData.Offset, rawData.Size);
            }
            // other wise we have no write slot
        }

        private static void WorkOnChild(PlcRawData rd, int baseOffset, ref bool inReadOnlyArea, ref int offset, ref bool itemsToAdd, ref byte bitMask, ref int bitMaskOffset, ref PlcObject? pred, PlcObject plcObj)
        {
            var currentOffset = baseOffset + plcObj.ByteOffset;
            // if we had a bit mask and we change the byte, add the mask to the slot.
            if (bitMaskOffset != -1 && bitMaskOffset != currentOffset)
            {
                if (bitMask != 0x00)
                {
                    rd.AddWriteSlot(bitMaskOffset, 1, bitMask);  // we need no mask because the whole byte will be skipped
                    itemsToAdd = false;
                }
                bitMaskOffset = -1;
                bitMask = 0xFF;
                inReadOnlyArea = true; // avoid a second write in the readonly area
            }

            if (plcObj.IsReadOnly)
            {
                // readonly bit found so update bit mask.
                if (plcObj is PlcBool)
                {
                    bitMask = bitMask.SetBit(plcObj.BitOffset, false);
                    bitMaskOffset = currentOffset;
                } 
                
                if (!inReadOnlyArea)
                {
                    // if we were in a write area before this value, we can add a write slot for the items
                    if (pred != null)
                    {
                        var size = currentOffset - offset;
                        if (size > 0)
                        {
                            rd.AddWriteSlot(offset, size);
                            itemsToAdd = false;
                        }
                    }
                }
            }
            else if (plcObj.HasReadOnlyChilds)
            {
                foreach (var child in plcObj.ChildVars.OfType<PlcObject>())
                {
                    WorkOnChild(rd, currentOffset, ref inReadOnlyArea, ref offset, ref itemsToAdd, ref bitMask, ref bitMaskOffset, ref pred, child);
                    if (plcObj is PlcArray array)
                    {
                        currentOffset += array.ArrayType.ByteSize;
                        if (currentOffset % 2 != 0) currentOffset++;
                    }
                }
            }
            else if (inReadOnlyArea)
            {
                offset = currentOffset;
                itemsToAdd = true;
            }

            inReadOnlyArea = plcObj.IsReadOnly;
            pred = plcObj;
        }
    }
}
