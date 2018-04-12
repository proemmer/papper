using Papper;
using Papper.Attributes;
using Papper.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PapperCmd
{
    #region Safety
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

    [Mapping("DB_Safety", "DB15", 0)]
    public class DB_Safety
    {
        public UDT_SafeMotion SafeMotion { get; set; }

    }

    #endregion

    public class Program
    {
        private static bool _toggle;
        private class PlcBlock
        {
            public Memory<byte> Data { get; private set; }
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
                    Data.CopyTo(tmp);
                    Data = tmp;
                }
            }
        }
        private static Dictionary<string, PlcBlock> _plc = new Dictionary<string, PlcBlock>();


        public static void Main(string[] args)
        {
            var papper = new PlcDataMapper(960, Papper_OnRead, Papper_OnWrite);
            papper.AddMapping(typeof(DB_Safety));

            PerformReadFull(papper);
            PerformRead(papper);
            PerformWrite(papper);
            PerformRead(papper);
            PerformWrite(papper);
            PerformRead(papper);
            PerformDataChange(papper);
            PerformRawDataChange(papper);
        }



        private static void PerformReadFull(PlcDataMapper papper)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            var result = papper.ReadAsync(PlcReadReference.FromAddress("DB_Safety.SafeMotion")).GetAwaiter().GetResult();
            foreach (var item in result)
            {
                Console.WriteLine($"Red:{item.Address} = {item.Value}");
            }
            Console.ResetColor();
        }

        private static void PerformRead(PlcDataMapper papper)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            var result = papper.ReadAsync(PlcReadReference.FromRoot("DB_Safety", 
                                     "SafeMotion.Header.NumberOfActiveSlots", 
                                     "SafeMotion.Header.Generated",
                                     "SafeMotion.Slots[42].SlotId",
                                     "SafeMotion.Slots[42].HmiId",
                                     "SafeMotion.Slots[42].Commands.TakeoverPermitted").ToArray()).GetAwaiter().GetResult(); ;
            foreach (var item in result)
            {
                Console.WriteLine($"Red:{item.Address} = {item.Value}");
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
            var result = papper.WriteAsync(PlcWriteReference.FromRoot("DB_Safety", writeData.ToArray()).ToArray());
            Console.ResetColor();
        }

        private static PlcBlock GetPlcEntry(string selector, int minSize)
        {
            if (!_plc.TryGetValue(selector, out PlcBlock plcblock))
            {
                plcblock = new PlcBlock(minSize);
                _plc.Add(selector, plcblock);
                return plcblock;
            }
            plcblock.UpdateBlockSize(minSize);
            return plcblock;
        }

        private static void PerformDataChange(PlcDataMapper papper)
        {
            var mapping = "DB_Safety";
            Console.WriteLine();
            Console.WriteLine($"Start PerformDataChange");
            Console.ForegroundColor = ConsoleColor.Red;
            var writeData = new Dictionary<string, object> {
                    { "SafeMotion.Slots[15].SlotId", 3},
                    { "SafeMotion.Slots[15].HmiId", 4},
                    { "SafeMotion.Slots[15].Commands.TakeoverPermitted", !_toggle ? true : false },
                };
            _toggle = !_toggle;
            var are = new AutoResetEvent(false);
            void callback(object s, PlcNotificationEventArgs e)
            {
                foreach (var item in e)
                    Console.WriteLine($"DataChanged detected: {item.Address} = {item.Value}");
                are.Set();
            }
            var items = writeData.Keys.Select(variable => PlcReadReference.FromAddress($"{mapping}.{variable}")).ToArray();
            var subscription = papper.SubscribeDataChanges(callback, items);

            //waiting for initialize
            if (!are.WaitOne(10000))
                Console.WriteLine($"Error-> change!!!!!");

            foreach (var item in writeData)
                Console.WriteLine($"Write:{item.Key} = {item.Value}");

            var result = papper.WriteAsync(PlcWriteReference.FromRoot("DB_Safety", writeData.ToArray()).ToArray()).GetAwaiter().GetResult();

            //waiting for write update
            if (!are.WaitOne(10000))
                Console.WriteLine($"Error-> change!!!!!");

            //test if data change only occurred if data changed
            if (are.WaitOne(5000))
                Console.WriteLine($"Error-> no change!!!!!");

            subscription.Dispose();
            Console.ResetColor();
            Console.WriteLine($"Finished PerformDataChange");
        }

        private static void PerformRawDataChange(PlcDataMapper papper)
        {
            Console.WriteLine();
            Console.WriteLine($"Start PerformRawDataChange");
            Console.ForegroundColor = ConsoleColor.Red;
            var writeData = new Dictionary<string, object> {
                    { "W0", 3},
                    { "X5_0", !_toggle ? true : false },
                };
            _toggle = !_toggle;
            var are = new AutoResetEvent(false);
            OnChangeEventHandler callback = (s, e) =>
            {
                foreach (var item in e)
                    Console.WriteLine($"DataChanged detected:{item.Address} = {item.Value}");
                are.Set();
            };
            //papper.SubscribeRawDataChanges("DB15", callback);
            //papper.SetRawActiveState(true, "DB15", writeData.Keys.ToArray());

            ////waiting for initialize
            //if (!are.WaitOne(10000))
            //    Console.WriteLine($"Error-> change!!!!!");

            //foreach (var item in writeData)
            //    Console.WriteLine($"Write:{item.Key} = {item.Value}");

            //var result = papper.WriteAbs("DB15", writeData);

            ////waiting for write update
            //if (!are.WaitOne(10000))
            //    Console.WriteLine($"Error-> change!!!!!");

            ////test if data change only occurred if data changed
            //if (are.WaitOne(5000))
            //    Console.WriteLine($"Error-> no change!!!!!");

            //papper.SetRawActiveState(false, "DB15", writeData.Keys.ToArray());
            //papper.Unsubscribe("DB15", callback);
            Console.ResetColor();
            Console.WriteLine($"Finished PerformDataChange");
        }

        private static Task Papper_OnWrite(IEnumerable<DataPack> reads)
        {
            var result = reads.ToList();
            foreach (var item in result)
            {
                if(item.BitMask == 0)
                {
                    Console.WriteLine($"OnWrite: selector:{item.Selector}; offset:{item.Offset}; length:{item.Length}");
                    item.Data.Slice(0, item.Length).CopyTo(GetPlcEntry(item.Selector, item.Offset + item.Length).Data.Slice(item.Offset, item.Length));
                }
                else
                {
                    for (int j = 0; j < item.Data.Length; j++)
                    {
                        var bItem = item.Data.Span[j];
                        var bm = item.BitMask;
                        for (var i = 0; i < 8; i++)
                        {
                            var bit = bm.GetBit(i);
                            if (bit)
                            {
                                var b = GetPlcEntry(item.Selector, item.Offset + 1).Data.Span[item.Offset];
                                GetPlcEntry(item.Selector, item.Offset + 1).Data.Span[item.Offset] = b.SetBit(i, bItem.GetBit(i));
                                item.ExecutionResult = ExecutionResult.Ok;
                                bm = bm.SetBit(i, false);
                                if (bm == 0)
                                    break;
                            }
                        }
                    }
                }
            }
            return Task.CompletedTask;
        }


        //private static bool Papper_OnWrite(string selector, int offset, byte[] data, byte bitMask = 0)
        //{
        //    try
        //    {
        //        var length = data.Length;
        //        if (bitMask == 0)
        //        {
        //            Console.WriteLine($"OnWrite: selector:{selector}; offset:{offset}; length:{length}");
        //            Array.Copy(data, 0, GetPlcEntry(selector, offset + length).Data, offset, length);
        //        }
        //        else
        //        {
        //            foreach (var item in data)
        //            {
        //                var bm = bitMask;
        //                for (var i = 0; i < 8; i++)
        //                {
        //                    var bit = bm.GetBit(i);
        //                    if (bit)
        //                    {
        //                        var b = GetPlcEntry(selector, offset + 1).Data[offset];
        //                        GetPlcEntry(selector, offset + 1).Data[offset] = b.SetBit(i, item.GetBit( i));
        //                        bm = bm.SetBit(i, false);
        //                        if (bm == 0)
        //                            break;
        //                    }
        //                }
        //            }
        //        }
        //        return true;
        //    }
        //    catch(Exception)
        //    {
        //        return false;
        //    }
        //}


        private static Task Papper_OnRead(IEnumerable<DataPack> reads)
        {
            var result = reads.ToList();
            foreach (var item in result)
            {
                Console.WriteLine($"OnRead: selector:{item.Selector}; offset:{item.Offset}; length:{item.Length}");
                var res = GetPlcEntry(item.Selector, item.Offset + item.Length).Data.Slice(item.Offset, item.Length);
                if (!res.IsEmpty)
                {
                    item.ApplyData(res);
                    item.ExecutionResult = ExecutionResult.Ok;
                }
                else
                {
                    item.ExecutionResult = ExecutionResult.Error;
                }
            }
            return Task.CompletedTask;
        }


    }
}
