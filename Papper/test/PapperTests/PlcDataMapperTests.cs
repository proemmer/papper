using Papper;
using Papper.Notification;
using PapperTests.Mappings;
using System;
using System.Buffers.Binary;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using UnitTestSuit.Mappings;
using UnitTestSuit.Util;
using Xunit;

//run in sequence because of db sharing
[assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly)]
namespace UnitTestSuit
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class PlcDataMapperTests
    {
        private PlcDataMapper _papper = new PlcDataMapper(960);

        public PlcDataMapperTests()
        {
            _papper.OnRead += Papper_OnRead;
            _papper.OnWrite += Papper_OnWrite;

            _papper.AddMapping(typeof(DB_Safety));
            _papper.AddMapping(typeof(ArrayTestMapping));
            _papper.AddMapping(typeof(StringArrayTestMapping));
            _papper.AddMapping(typeof(PrimitiveValuesMapping));

            var vars = _papper.GetVariablesOf(nameof(DB_Safety));
            Assert.Equal(4596, vars.Count());
        }



        [Fact]
        public void BitAccessTest()
        {
            var mapping = "DB_Safety";
            var accessDict = new Dictionary<string, object> {
                    { "SafeMotion.Slots[0].Commands.TakeoverPermitted", true },
                    { "SafeMotion.Slots[0].Commands.TakeoverRefused", true},
                    { "SafeMotion.Slots[0].Motion.ManualOperation1", true},
                    { "SafeMotion.Slots[0].Motion.ManualOperation2", true},
                    { "SafeMotion.Slots[42].Commands.TakeoverPermitted", true },
                    { "SafeMotion.Slots[42].Commands.TakeoverRefused", true},
                    { "SafeMotion.Slots[42].Motion.ManualOperation1", true},
                    { "SafeMotion.Slots[42].Motion.ManualOperation2", true},
                    { "SafeMotion.Slots[160].Commands.TakeoverPermitted", true },
                    { "SafeMotion.Slots[160].Commands.TakeoverRefused", true},
                    { "SafeMotion.Slots[160].Motion.ManualOperation1", true},
                    { "SafeMotion.Slots[160].Motion.ManualOperation2", true},
                    { "SafeMotion.Slots[254].Commands.TakeoverPermitted", true },
                    { "SafeMotion.Slots[254].Commands.TakeoverRefused", true},
                    { "SafeMotion.Slots[254].Motion.ManualOperation1", true},
                    { "SafeMotion.Slots[254].Motion.ManualOperation2", true},
                };
            
            Test(mapping, accessDict, false);
        }

        [Fact]
        public void Int16AccessTest()
        {
            var mapping = "DB_Safety";
            var accessDict = new Dictionary<string, object> {
                    { "SafeMotion.Header.NumberOfActiveSlots",(Int16) 1 },
                    { "SafeMotion.Slots[0].AggregateDBNummer", (Int16)2},
                    { "SafeMotion.Slots[0].AggregateOffset", (Int16)3},
                    { "SafeMotion.Slots[0].SafeSlotVersion", (Int16)4},
                    { "SafeMotion.Slots[100].AggregateDBNummer", (Int16)5},
                    { "SafeMotion.Slots[100].AggregateOffset",(Int16)6},
                    { "SafeMotion.Slots[100].SafeSlotVersion", (Int16)7},
                    { "SafeMotion.Slots[254].AggregateDBNummer", (Int16)8},
                    { "SafeMotion.Slots[254].AggregateOffset", (Int16)9},
                    { "SafeMotion.Slots[254].SafeSlotVersion", (Int16)10},
                };
            
            Test(mapping, accessDict, (Int16)0);
        }

        [Fact]
        public void UInt16AccessTest()
        {
            var mapping = "DB_Safety";
            var accessDict = new Dictionary<string, object> {
                    { "SafeMotion.Slots[0].UnitChecksum", (UInt16)2},
                    { "SafeMotion.Slots[100].UnitChecksum", (UInt16)5},
                    { "SafeMotion.Slots[254].UnitChecksum", (UInt16)8},

                };
            
            Test(mapping, accessDict, (UInt16)0);
        }

        [Fact]
        public void UInt32AccessTest()
        {
            var mapping = "DB_Safety";
            var accessDict = new Dictionary<string, object> {
                    { "SafeMotion.Slots[0].HmiId", (UInt32)3},
                    { "SafeMotion.Slots[0].AccessRightReqFromHmiId", (UInt32)4},
                    { "SafeMotion.Slots[100].HmiId",(UInt32)6},
                    { "SafeMotion.Slots[100].AccessRightReqFromHmiId", (UInt32)7},
                    { "SafeMotion.Slots[254].HmiId", (UInt32)9},
                    { "SafeMotion.Slots[254].AccessRightReqFromHmiId", (UInt32)10},
                };
            
            Test(mapping, accessDict, (UInt32)0);
        }

        [Fact]
        public void DateAccessTest()
        {
            var mapping = "DB_Safety";
            var accessDict = new Dictionary<string, object> {
                    { "SafeMotion.Header.Generated", Normalize(DateTime.Now)},
                    { "SafeMotion.Slots[0].UnitTimestamp", Normalize(DateTime.Now)},
                    { "SafeMotion.Slots[100].UnitTimestamp", Normalize(DateTime.Now)},
                    { "SafeMotion.Slots[254].UnitTimestamp",Normalize( DateTime.Now)},
                };
            
            Test(mapping, accessDict, new DateTime(599266080000000000));  //01.01.1900
        }


        [Fact]
        public void SingleAccessTest()
        {
            var mapping = "PrimitiveValuesMapping";
            var accessDict = new Dictionary<string, object> {
                    { "Single", (Single)2.2},
                };

            Test(mapping, accessDict, (Single)0);
        }

        [Fact]
        public void ArrayElementsAccessTest()
        {
            var mapping = "ARRAY_TEST_MAPPING_1";
            var accessDict = new Dictionary<string, object> {
                    { "ByteElements[10]", (byte)0x05},
                    { "ByteElements[5]", (byte)0x06},
                    { "ByteElements[1]", (byte)0x07},
                    { "CharElements[10]", 'a'},
                    { "CharElements[5]", 'b'},
                    { "CharElements[1]", 'c'},
                    { "IntElements[10]", 10},
                    { "IntElements[5]", 20},
                    { "IntElements[1]", 30},
                };

            
            Test(mapping, accessDict.Take(3).ToDictionary(kvp => kvp.Key, kvp => kvp.Value), default(byte));
            Test(mapping, accessDict.Skip(3).Take(3).ToDictionary(kvp => kvp.Key, kvp => kvp.Value), default(char));
            Test(mapping, accessDict.Skip(6).Take(3).ToDictionary(kvp => kvp.Key, kvp => kvp.Value), default(int));
        }

        [Fact]
        public void BigByteArrayAccessTest()
        {
            var mapping = "ARRAY_TEST_MAPPING_2";
            var accessDict = new Dictionary<string, object> {
                    { "BigByteArray", Enumerable.Repeat<byte>(0x01,50000).ToArray()},
                };
            
            Test(mapping, accessDict, Enumerable.Repeat<byte>(0x00, 50000).ToArray());
        }

        [Fact]
        public void BigCharArrayAccessTest()
        {
            var mapping = "ARRAY_TEST_MAPPING_3";
            var accessDict = new Dictionary<string, object> {
                    { "BigCharArray", Enumerable.Repeat<char>('a',50000).ToArray()},
                };
            
            Test(mapping, accessDict, Enumerable.Repeat<char>(default(char), 50000).ToArray());
        }

        [Fact]
        public void BigIntArrayAccessTest()
        {
            var mapping = "ARRAY_TEST_MAPPING_4";
            var accessDict = new Dictionary<string, object> {
                    { "BigIntArray", Enumerable.Repeat(2,5000).ToArray()},
                };
            
            Test(mapping, accessDict, Enumerable.Repeat(0, 5000).ToArray());
        }


        [Fact]
        public void TestStructuralAccess()
        {
            var mapping = "DB_Safety2";
            var header = new UDT_SafeMotionHeader
            {
                Generated = Normalize(DateTime.Now),
                NumberOfActiveSlots = 2,
                Commands = new UDT_SafeMotionHeader_Commands
                {
                    AllSlotsLocked = true,
                    UpdateAllowed = true
                },
                States = new UDT_SafeMotionHeader_States
                {
                    ChecksumInvalid = true,
                    UpdateRequested = true
                }
            };

            var accessDict = new Dictionary<string, object> {
                    { "SafeMotion.Header", header},
                };

            var result = _papper.ReadAsync(accessDict.Keys.Select( variable => PlcReadReference.FromAddress($"{mapping}.{variable}")).ToArray()).GetAwaiter().GetResult(); 
            Assert.Equal(accessDict.Count, result.Length);
            var writeResults = _papper.WriteAsync(PlcWriteReference.FromRoot(mapping, accessDict.ToArray()).ToArray()).GetAwaiter().GetResult();
            foreach (var item in writeResults)
            {
                Assert.Equal(ExecutionResult.Ok, item.ActionResult);
            }
            var result2 = _papper.ReadAsync(accessDict.Keys.Select(variable => PlcReadReference.FromAddress($"{mapping}.{variable}")).ToArray()).GetAwaiter().GetResult(); 
            Assert.Equal(accessDict.Count, result2.Length);
            Assert.False(AreDataEqual(result, result2));


            // Assert.True(AreDataEqual(ToExpando(header), result2.Values.FirstOrDefault()));
        }

        [Fact]
        public void TestDataChange()
        {
            var sleepTime = 1000;
            var mapping = "DB_Safety";
            var intiState = true;
            var originData = new Dictionary<string, object> {
                    { "SafeMotion.Slots[15].SlotId", (byte)0},
                    { "SafeMotion.Slots[15].HmiId", (UInt32)0},
                    { "SafeMotion.Slots[15].Commands.TakeoverPermitted", false },
                };
            var writeData = new Dictionary<string, object> {
                    { "SafeMotion.Slots[15].SlotId", (byte)3},
                    { "SafeMotion.Slots[15].HmiId", (UInt32)4},
                    { "SafeMotion.Slots[15].Commands.TakeoverPermitted", false },
                };
            var are = new AutoResetEvent(false);


            using (var subscription = _papper.CreateSubscription())
            {
                subscription.AddItems(originData.Keys.Select(variable => PlcReadReference.FromAddress($"{mapping}.{variable}")));
                var t = Task.Run(async () =>
               {
                   try
                   {
                       while (!subscription.Watching.IsCompleted)
                       {
                           var res = await subscription.DetectChangesAsync();

                           if (!res.IsCompleted && !res.IsCanceled)
                           {
                               if (!intiState)
                               {
                                   Assert.Equal(2, res.Results.Length);
                               }
                               else
                               {
                                   Assert.Equal(3, res.Results.Length);
                               }

                               foreach (var item in res.Results)
                               {
                                   try
                                   {
                                       if (!intiState)
                                           Assert.Equal(writeData[item.Variable], item.Value);
                                       else
                                           Assert.Equal(originData[item.Variable], item.Value);
                                   }
                                   catch (Exception ex)
                                   {

                                   }
                               }

                               are.Set();
                           } 
                       }
                   }
                   catch(Exception ex)
                   {

                   }
               });

                //waiting for initialize
                Assert.True(are.WaitOne(sleepTime));
                intiState = false;
                var writeResults = _papper.WriteAsync(PlcWriteReference.FromRoot(mapping, writeData.ToArray()).ToArray()).GetAwaiter().GetResult();
                foreach (var item in writeResults)
                {
                    Assert.Equal(ExecutionResult.Ok, item.ActionResult);
                }
                //waiting for write update
                Assert.True(are.WaitOne(sleepTime));

                //test if data change only occurred if data changed
                Assert.False(are.WaitOne(sleepTime));

            }
        }

        [Fact]
        public void AddAndRemoveSubscriptions()
        {
            var writeData = new Dictionary<string, object> {
                    { "W88", (UInt16)3},
                    { "X99_0", true  },
                };
            var items = writeData.Keys.Select(variable => PlcReadReference.FromAddress($"DB15.{variable}")).ToArray();

            using (var sub = _papper.CreateSubscription())
            {
                Assert.True(sub.AddItems(items));
                var c = sub.DetectChangesAsync();  // returns because we start a new detection
                Thread.Sleep(100);
                Assert.True(sub.RemoveItems(items.FirstOrDefault()));
                Assert.True(sub.RemoveItems(items.FirstOrDefault())); // <- modified is already true
                c = sub.DetectChangesAsync();      // returns because we moditied the detection
                Assert.False(sub.RemoveItems(items.FirstOrDefault()));
            }
        }

        [Fact]
        public async void TestDuplicateDetection()
        {
            var writeData = new Dictionary<string, object> {
                    { "W88", (UInt16)3},
                    { "X99_0", true  },
                };
            var items = writeData.Keys.Select(variable => PlcReadReference.FromAddress($"DB15.{variable}")).ToArray();

            using (var sub = _papper.CreateSubscription())
            {
                await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                {
                    var c1 = sub.DetectChangesAsync();  // returns because we start a new detection
                    var c2 = await sub.DetectChangesAsync();  // returns because we start a new detection
                    await c1;
                });
            }
        }

        [Fact]
        public async void TestCancellation()
        {
            var writeData = new Dictionary<string, object> {
                    { "W88", (UInt16)3},
                    { "X99_0", true  },
                };
            var items = writeData.Keys.Select(variable => PlcReadReference.FromAddress($"DB15.{variable}")).ToArray();

            using (var sub = _papper.CreateSubscription())
            {
                Assert.True(sub.AddItems(items));
                var c = sub.DetectChangesAsync();  // returns because we start a new detection
                Thread.Sleep(500);
                var res = await c;
                Assert.False(res.IsCanceled);
                Assert.False(res.IsCompleted);
                Assert.NotNull(res.Results);
                Assert.Equal(2, res.Results.Length);

                c = sub.DetectChangesAsync(); 
                Thread.Sleep(500);
                sub.Pause();
                res = await c;
                Assert.True(res.IsCanceled);
                Assert.False(res.IsCompleted);
                Assert.Null(res.Results);

            }
        }

        [Fact]
        public void PerformRawDataChange()
        {
            var intiState = true;
            var originData = new Dictionary<string, object> {
                    { "W88", (UInt16)0},
                    { "X99_0", false  },
                    { "DW100", (UInt32)0},
                };
            var writeData = new Dictionary<string, object> {
                    { "W88", (UInt16)3},
                    { "X99_0", true  },
                    { "DW100", (UInt32)5},
                };
            var are = new AutoResetEvent(false);
            void callback(object s, PlcNotificationEventArgs e)
            {
                foreach (var item in e)
                {
                    if (!intiState)
                        Assert.Equal(writeData[item.Variable], item.Value);
                    else
                        Assert.Equal(originData[item.Variable], item.Value);
                }
                are.Set();
            }
            var subscription = _papper.SubscribeDataChanges(callback, writeData.Keys.Select(variable => PlcReadReference.FromAddress($"DB15.{variable}")).ToArray());


            //waiting for initialize
            Assert.True(are.WaitOne(5000));
            intiState = false;
            var writeResults = _papper.WriteAsync(PlcWriteReference.FromRoot("DB15", writeData.ToArray()).ToArray()).GetAwaiter().GetResult();
            foreach (var item in writeResults)
            {
                Assert.Equal(ExecutionResult.Ok, item.ActionResult);
            }

            //waiting for write update
            Assert.True(are.WaitOne(5000));

            //test if data change only occurred if data changed
            Assert.False(are.WaitOne(5000));

            subscription.Dispose();
        }

        [Fact]
        public void ArrayIndexAccessTest()
        {
            var mapping = "ARRAY_TEST_MAPPING_5";
            var accessDict = new Dictionary<string, object> {
                    { "BigCharArray[1]", 'X'},
                };

            Test(mapping, accessDict, default(char));
        }

        [Fact]
        public void ArrayStringAccessTest()
        {
            var mapping = "STRING_ARRAY_TEST_MAPPING";
            var accessDict = new Dictionary<string, object> {
                    { "TEXT[1]", "TEST1"},
                    { "TEXT[5]", "TEST5"},
                };

            Test(mapping, accessDict, "");

            //Byte data check
            var dbData = MockPlc.GetPlcEntry("DB30").Data;
            Assert.True(dbData.SubArray(0, 2).SequenceEqual(new byte[] { 35, 5 }));
            Assert.True(dbData.SubArray(2, 5).SequenceEqual("TEST1".ToByteArray(5)));

            Assert.True(dbData.SubArray(152, 2).SequenceEqual(new byte[] { 35, 5 }));
            Assert.True(dbData.SubArray(154, 5).SequenceEqual("TEST5".ToByteArray(5)));
        }


        [Fact]
        public void TODTest()
        {
            var mapping = "STRING_ARRAY_TEST_MAPPING_1";
            var accessDict = new Dictionary<string, object> {
                    { "Time[1]", TimeSpan.FromHours(12)},
                    { "Time[2]", TimeSpan.FromHours(1)},
                    { "Time[3]", TimeSpan.FromSeconds(10)},
                    { "Time[4]", TimeSpan.FromMilliseconds(111)},
                };

            Test(mapping, accessDict, new TimeSpan(0));


        }


        [Fact]
        public void ReadNonExistingValue()
        {
            var mapping = "STRING_ARRAY_TEST_MAPPING";
            var accessDict = new Dictionary<string, object> {
                    { "XXX", "TEST1"}
                };


            var result = _papper.ReadAsync(PlcReadReference.FromRoot(mapping, accessDict.Keys.ToArray()).ToArray()).GetAwaiter().GetResult(); ;
            Assert.Empty(result);
        }

        [Fact]
        public void ConvertTest()
        {
            Span<byte> data = new Span<byte>(new byte[] { 0x01, 0x02, 0x03, 0x04 });

            var v2 = BinaryPrimitives.ReadInt32BigEndian(data);
            var v3 = BinaryPrimitives.ReadInt32LittleEndian(data);

            var data1 = new Span<byte>(new byte[4]);
            Single s = 25.4f;
            BinaryPrimitives.WriteInt32BigEndian(data1, Convert.ToInt32(s));
            var res = Convert.ToSingle(BinaryPrimitives.ReadInt32BigEndian(data1));

            var data4 = new Span<byte>(new byte[4]);
            Converter.WriteSingleBigEndian(data4, s);
            var x4 = Converter.ReadSingleBigEndian(data4);
        }


    

        #region Helper


        private void Test<T>(string mapping, Dictionary<string, object> accessDict, T defaultValue)
        {
            //Initial read to ensure all are false
            var toRead = accessDict.Keys.Select(variable => PlcReadReference.FromAddress($"{mapping}.{variable}")).ToArray();
            var result = _papper.ReadAsync(toRead).GetAwaiter().GetResult();
            Assert.Equal(accessDict.Count, result.Length);
            foreach (var item in result)
                Assert.Equal(defaultValue, (T)item.Value);

            //Write the value
            _papper.WriteAsync(PlcWriteReference.FromRoot(mapping, accessDict.ToArray()).ToArray()).GetAwaiter().GetResult();

            //Second read to ensure correct written
            result = _papper.ReadAsync(accessDict.Keys.Select(variable => PlcReadReference.FromAddress($"{mapping}.{variable}")).ToArray()).GetAwaiter().GetResult(); 
            Assert.Equal(accessDict.Count, result.Length);
            foreach (var item in result)
                Assert.Equal((T)accessDict[item.Variable], (T)item.Value);
        }

        /// <summary>
        /// This method remove the nanoseconds from the date time, because plc's could not handle that
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private DateTime Normalize(DateTime dt)
        {
            return dt.AddTicks((dt.Ticks % 10000) * -1);
        }

        private static async Task Papper_OnRead(IEnumerable<DataPack> reads)
        {
            var result = reads.ToList();
            foreach (var item in result)
            {
                Console.WriteLine($"OnRead: selector:{item.Selector}; offset:{item.Offset}; length:{item.Length}");
                var res = MockPlc.GetPlcEntry(item.Selector, item.Offset + item.Length).Data.SubArray(item.Offset, item.Length);
                if(res != null)
                {
                    item.ApplyData(res);
                    item.ExecutionResult = ExecutionResult.Ok;
                }
                else
                {
                    item.ExecutionResult = ExecutionResult.Error;
                }
            }
            await Task.CompletedTask;
        }

        private async Task Papper_OnWrite(IEnumerable<DataPack> reads)
        {
            var result = reads.ToList();
            foreach (var item in result)
            {
                if (item.BitMask == 0)
                {
                    Console.WriteLine($"OnWrite: selector:{item.Selector}; offset:{item.Offset}; length:{item.Length}");
                    Array.Copy(item.Data, 0, MockPlc.GetPlcEntry(item.Selector, item.Offset + item.Length).Data, item.Offset, item.Length);
                    item.ExecutionResult = ExecutionResult.Ok;
                }
                else
                {
                    foreach (var bItem in item.Data)
                    {
                        var bm = item.BitMask;
                        for (var i = 0; i < 8; i++)
                        {
                            var bit = bm.GetBit(i);
                            if (bit)
                            {
                                var b = MockPlc.GetPlcEntry(item.Selector, item.Offset + 1).Data[item.Offset];
                                MockPlc.GetPlcEntry(item.Selector, item.Offset + 1).Data[item.Offset] = b.SetBit(i, bItem.GetBit(i));
                                item.ExecutionResult = ExecutionResult.Ok;
                                bm = bm.SetBit(i, false);
                                if (bm == 0)
                                    break;
                            }
                        }
                    }
                }
            }
            await Task.CompletedTask;
        }


        //private static bool Papper_OnWrite(string selector, int offset, byte[] data, byte bitMask = 0)
        //{
        //    try
        //    {
        //        var length = data.Length;
        //        if (bitMask == 0)
        //        {
        //            Console.WriteLine($"OnWrite: selector:{selector}; offset:{offset}; length:{length}");
        //            Array.Copy(data, 0, MockPlc.GetPlcEntry(selector, offset + length).Data, offset, length);
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
        //                        Console.WriteLine($"OnWriteBit: selector:{selector}; offset:{offset + 1};");
        //                        var b = MockPlc.GetPlcEntry(selector, offset + 1).Data[offset];
        //                        MockPlc.GetPlcEntry(selector, offset + 1).Data[offset] = b.SetBit(i, item.GetBit(i));
        //                        bm = bm.SetBit(i, false);
        //                        if (bm == 0)
        //                            break;
        //                    }
        //                }
        //            }
        //        }
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}

        private static ExpandoObject ToExpando<T>(T instance)
        {
            var obj = new ExpandoObject();
            foreach (var item in instance.GetType().GetTypeInfo().DeclaredProperties)
            {
                if(!item.PropertyType.Namespace.StartsWith("System"))
                {
                    AddProperty(obj, item.Name, ToExpando(item.GetValue(instance)));
                }
                else
                {
                    AddProperty(obj, item.Name, item.GetValue(instance));
                }
            }
            return obj;
        }

        private static void AddProperty(dynamic parent, string name, object value)
        {
            var list = (parent as List<dynamic>);
            if (list != null)
            {
                list.Add(value);
            }
            else
            {
                var dictionary = parent as IDictionary<string, object>;
                if (dictionary != null)
                    dictionary[name] = value;
            }
        }

        private static bool AreDataEqual(object obj1, object obj2)
        {
            var t1 = obj1.GetType();
            var t2 = obj2.GetType();

            if (t1 == t2) { return t1 != typeof(ExpandoObject) ? ElementEqual(obj1, obj2) : DynamicObjectCompare(obj1, obj2); }
            try { return ElementEqual(obj1, Convert.ChangeType(obj2, t1)); } catch { }
            return false;
        }

        private static bool ElementEqual(object obj1, object obj2)
        {
            var list1 = obj1 as IEnumerable;
            var list2 = obj2 as IEnumerable;

            if (list1 != null && list2 != null)
            {
                var enumerator1 = list1.GetEnumerator();
                var enumerator2 = list2.GetEnumerator();
                while (true)
                {
                    var e1 = enumerator1.MoveNext();
                    var e2 = enumerator2.MoveNext();
                    if (e1 && e2)
                    {
                        if (!AreDataEqual(enumerator1.Current, enumerator2.Current))
                            return false;
                    }
                    else
                        return e1 == e2; //Length not the same?
                }
            }
            return obj1.Equals(obj2);
        }

        private static bool DynamicObjectCompare(object obj1, object obj2)
        {
            var dictionary1 = obj1 as IDictionary<string, object>;
            var dictionary2 = obj2 as IDictionary<string, object>;

            if (dictionary1 != null && dictionary2 != null)
            {
                foreach (var o1 in dictionary1)
                {
                    if (!dictionary2.TryGetValue(o1.Key, out object o2) || !AreDataEqual(o1.Value, o2))
                        return false;

                }
            }
            return true;
        }

        #endregion
    }
}
