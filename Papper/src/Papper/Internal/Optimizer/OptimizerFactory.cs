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

        internal static void AddWritableAreas(PlcObject item, PlcRawData rawData)
        {
            if (item.HasReadOnlyChilds)
            {
                static void HandleChilds(PlcObject plcObj, PlcRawData rd, int baseOffset, bool inReadOnlyArea = false)
                {
                    var offset = 0;
                    var itemsToAdd = false;
                    byte bitMask = 0xff;
                    var bitMaskOffset = -1;
                    PlcObject? pred = null;
                    WorkOnChild(rd, baseOffset, ref inReadOnlyArea, ref offset, ref itemsToAdd, ref bitMask, ref bitMaskOffset, ref pred, plcObj);

                    if (bitMaskOffset != -1)
                    {
                        rd.AddWriteSlot(bitMaskOffset, 1, bitMask);
                    }
                    else if (itemsToAdd)
                    {
                        rd.AddWriteSlot(offset, plcObj.ByteSize - offset);
                    }
                }

                HandleChilds(item, rawData, rawData.Offset);
            }
            else if (!item.IsReadOnly)
            {
                rawData.AddWriteSlot(rawData.Offset, rawData.Size);
            }
        }

        private static void WorkOnChild(PlcRawData rd, int baseOffset, ref bool inReadOnlyArea, ref int offset, ref bool itemsToAdd, ref byte bitMask, ref int bitMaskOffset, ref PlcObject? pred, PlcObject plcObj)
        {
            if (bitMaskOffset != -1 && bitMaskOffset != plcObj.ByteOffset)
            {
                rd.AddWriteSlot(bitMaskOffset, 1, bitMask);
                bitMaskOffset = -1;
                bitMask = 0xFF;
                inReadOnlyArea = true;
            }

            if (plcObj.IsReadOnly)
            {
                if (plcObj is PlcBool)
                {
                    bitMask = bitMask.SetBit(plcObj.BitOffset, false);
                    bitMaskOffset = plcObj.ByteOffset;
                }

                if (!inReadOnlyArea)
                {
                    if (pred != null)
                    {
                        rd.AddWriteSlot(offset, baseOffset + plcObj.ByteOffset - offset);
                    }
                }
            }
            else if (plcObj.HasReadOnlyChilds)
            {
                var currentOffset = baseOffset + plcObj.ByteOffset;
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
                offset = baseOffset + plcObj.ByteOffset;
                itemsToAdd = true;
            }

            inReadOnlyArea = plcObj.IsReadOnly;
            pred = plcObj;
        }
    }
}
