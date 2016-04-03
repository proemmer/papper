using Papper;
using Papper.Attributes;
using System;
using System.Collections.Generic;

namespace PapperCmd
{
    #region InaxSafety
    public class UDT_SafeMotionHeader_States
    {
        public bool ChecksumInvalid { get; set; }
        public bool UpdateRequested { get; set; }
    }



    public class UDT_SafeMotionHeader_Commands
    {
        public bool UpdateAllowed { get; set; }
        public bool AllSlotsLocked { get; set; }
    }



    public class UDT_SafeMotionSlot_Commands
    {
        public bool TakeoverPermitted { get; set; }
        public bool TakeoverRefused { get; set; }
    }



    public class UDT_SafeMotionSlot_Handshake
    {
        public bool MotionSelected { get; set; }
        public bool Button1Pressed { get; set; }
        public bool Button2Pressed { get; set; }
        public Int16 HandshakeTime { get; set; }
    }



    public class UDT_SafeMotionSlot_Motion
    {
        public bool ManualEnable1 { get; set; }
        public bool ManualEnable2 { get; set; }
        public bool ManualOperation1 { get; set; }
        public bool ManualOperation2 { get; set; }
    }



    public class UDT_SafeMotionHeader
    {
        public DateTime Generated { get; set; }
        public Int16 NumberOfActiveSlots { get; set; }
        public UDT_SafeMotionHeader_States States { get; set; }
        public UDT_SafeMotionHeader_Commands Commands { get; set; }

    }


    public class UDT_SafeMotionSlot
    {
        public Int16 SafeSlotVersion { get; set; }
        public byte SlotId { get; set; }
        public DateTime UnitTimestamp { get; set; }
        public UInt16 UnitChecksum { get; set; }
        public Int16 AggregateDBNummer { get; set; }
        public Int16 AggregateOffset { get; set; }
        public UInt32 HmiId { get; set; }
        public UInt32 AccessRightReqFromHmiId { get; set; }
        public UDT_SafeMotionSlot_Commands Commands { get; set; }
        public UDT_SafeMotionSlot_Handshake Handshake { get; set; }
        public UDT_SafeMotionSlot_Motion Motion { get; set; }

    }


    public class UDT_SafeMotion
    {
        public UDT_SafeMotionHeader Header { get; set; }

        [ArrayBounds(0,254)]
        public UDT_SafeMotionSlot[] Slots { get; set; }

    }

    [Mapping("DB_InaxSafety", "DB15", 0)]
    public class DB_InaxSafety
    {
        public UDT_SafeMotion SafeMotion { get; set; }

    }

    #endregion

    public class Program
    {
        private static bool _toggle;
        private class PlcBlock
        {
            public byte[] Data { get; private set; }
            public int MinSize { get { return Data.Length; } }

            public PlcBlock(int minSize)
            {
                Data = new byte[minSize];
            }

            public void UpdateBlockSize(int size)
            {
                if(Data.Length < size)
                {
                    var tmp = new byte[size];
                    Array.Copy(Data, 0, tmp, 0, Data.Length);
                    Data = tmp;
                }
            }
        }
        private static Dictionary<string, PlcBlock> _plc = new Dictionary<string, PlcBlock>();


        public static void Main(string[] args)
        {
            var papper = new PlcDataMapper(960);
            papper.OnRead += Papper_OnRead;
            papper.OnWrite += Papper_OnWrite;
            papper.OnWriteBits += Papper_OnWriteBits;

            papper.AddMapping(typeof(DB_InaxSafety));

            PerformReadFull(papper);
            PerformRead(papper);
            PerformWrite(papper);
            PerformRead(papper);
            PerformWrite(papper);
            PerformRead(papper);
        }

        private static void PerformReadFull(PlcDataMapper papper)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            var result = papper.Read("DB_InaxSafety",
                                     "SafeMotion");
            foreach (var item in result)
            {
                Console.WriteLine($"Red:{item.Key} = {item.Value}");
            }
            Console.ResetColor();
        }

        private static void PerformRead(PlcDataMapper papper)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            var result = papper.Read("DB_InaxSafety", 
                                     "SafeMotion.Header.NumberOfActiveSlots", 
                                     "SafeMotion.Header.Generated",
                                     "SafeMotion.Slots[42].SlotId",
                                     "SafeMotion.Slots[42].HmiId",
                                     "SafeMotion.Slots[42].Commands.TakeoverPermitted");
            foreach (var item in result)
            {
                Console.WriteLine($"Red:{item.Key} = {item.Value}");
            }
            Console.ResetColor();
        }

        private static void PerformWrite(PlcDataMapper papper)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            var writeData = new Dictionary<string, object> {
                    { "SafeMotion.Header.NumberOfActiveSlots", 2 },
                    { "SafeMotion.Header.Generated", DateTime.Now},
                    { "SafeMotion.Slots[42].SlotId", 3},
                    { "SafeMotion.Slots[42].HmiId", 4},
                    { "SafeMotion.Slots[42].Commands.TakeoverPermitted", !_toggle ? true : false },
                    { "SafeMotion.Slots[250].SlotId", 1},
                    { "SafeMotion.Slots[150].Handshake.MotionSelected", !_toggle ? true : false}
                };
            _toggle = !_toggle;
            foreach (var item in writeData)
            {
                Console.WriteLine($"Write:{item.Key} = {item.Value}");
            }
            var result = papper.Write("DB_InaxSafety", writeData);
            Console.ResetColor();
        }

        private static PlcBlock GetPlcEntry(string selector, int minSize)
        {
            PlcBlock plcblock;
            if (!_plc.TryGetValue(selector, out plcblock))
            {
                plcblock = new PlcBlock(minSize);
                _plc.Add(selector, plcblock);
                return plcblock;
            }
            plcblock.UpdateBlockSize(minSize);
            return plcblock;
        }

        private static bool Papper_OnWrite(string selector, int offset, int length, byte[] data)
        {
            try
            {
                Console.WriteLine($"OnWrite: selector:{selector}; offset:{offset}; length:{length}");
                Array.Copy(data, 0, GetPlcEntry(selector, offset + length).Data, offset, length);
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        private static byte[] Papper_OnRead(string selector, int offset, int length)
        {
            Console.WriteLine($"OnRead: selector:{selector}; offset:{offset}; length:{length}");
            return SubArray(GetPlcEntry(selector, offset + length).Data,offset, length);
        }

        private static bool Papper_OnWriteBits(string selector, int offset, byte[] data, byte mask = 0)
        {
            try
            {
                Console.WriteLine($"OnWriteBits: selector:{selector}; offset:{offset}; mask:{mask}");
                var baseOffset = (offset / 8);
                var bitLikeBaseOffset = (baseOffset * 8);
                var bitOffset = offset - bitLikeBaseOffset;
                for (var i = 0; i < 8; i++)
                {
                    if (mask == 0 || GetBit(mask, i))
                    {
                        if (mask != 0)
                        {
                            bitOffset = i;
                            offset = bitLikeBaseOffset + i;
                        }

                        var b = GetPlcEntry(selector, baseOffset + 1).Data[baseOffset];
                        GetPlcEntry(selector, baseOffset + 1).Data[baseOffset] = SetBit(b, bitOffset, GetBit(data[0], bitOffset));
                        mask = SetBit(mask, i, false);
                    }
                    if (mask == 0)
                        break;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static T[] SubArray<T>(T[] data, int skip, int length = -1, bool realloc = false)
        {
            var dataLength = data.Length;
            if (length == -1)
                length = dataLength - skip;
            if (skip == 0 && length == dataLength && !realloc) //No manipulation and no copying
                return data;
            var result = new T[length];
            Array.Copy(data, skip, result, 0, length);
            return result;
        }

        public static byte SetBit(byte data, int bit, bool value)
        {
            if (value)
                return (Byte)(data | (1U << bit));
            return (Byte)(data & (~(1U << bit)));
        }

        public static bool GetBit(byte data, int bit)
        {
            // Shift the bit to the first location
            data = (Byte)(data >> bit);

            // Isolate the value
            return (data & 1) == 1;
        }
    }
}
