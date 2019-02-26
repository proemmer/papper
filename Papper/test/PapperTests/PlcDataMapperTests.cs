using Papper;
using Papper.Extensions.Metadata;
using Papper.Extensions.Notification;
using PapperTests.Mappings;
using System;
using System.Buffers.Binary;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using UnitTestSuit.Mappings;
using UnitTestSuit.Util;
using Xunit;
using Xunit.Abstractions;

//run in sequence because of db sharing
[assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly)]
namespace DataTypeTests
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class PlcDataMapperTests
    {
        private PlcDataMapper _papper = new PlcDataMapper(960, Papper_OnRead, Papper_OnWrite);
        private readonly ITestOutputHelper _output;

        public PlcDataMapperTests(ITestOutputHelper output)
        {
            _output = output;
            _papper.AddMapping(typeof(DB_Safety));
            _papper.AddMapping(typeof(ArrayTestMapping));
            _papper.AddMapping(typeof(StringArrayTestMapping));
            _papper.AddMapping(typeof(PrimitiveValuesMapping));
            _papper.AddMapping(typeof(DB_MotionHMI));

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
        public void TestStructuralAccessOfRawData()
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

            var result = _papper.ReadBytesAsync(accessDict.Keys.Select(variable => PlcReadReference.FromAddress($"{mapping}.{variable}")).ToArray()).GetAwaiter().GetResult();
            Assert.Equal(accessDict.Count, result.Length);
            var writeResults = _papper.WriteAsync(PlcWriteReference.FromRoot(mapping, accessDict.ToArray()).ToArray()).GetAwaiter().GetResult();
            foreach (var item in writeResults)
            {
                Assert.Equal(ExecutionResult.Ok, item.ActionResult);
            }
            var result2 = _papper.ReadBytesAsync(accessDict.Keys.Select(variable => PlcReadReference.FromAddress($"{mapping}.{variable}")).ToArray()).GetAwaiter().GetResult();
            Assert.Equal(accessDict.Count, result2.Length);
            Assert.False(AreDataEqual(result, result2));

        }


        [Fact]
        public void TestStructuralAllAccess()
        {
            var mapping = "DB_Safety2";

            var t = new Stopwatch();
            t.Start();
            var result = _papper.ReadAsync(PlcReadReference.FromAddress($"{mapping}")).GetAwaiter().GetResult();
            t.Stop();
        }


        [Fact]
        public async Task MixedAccessTest()
        {
            var mapping = "DB_Safety2";

            var t = new Stopwatch();
            t.Start();
            var result1 = _papper.ReadAsync(PlcReadReference.FromAddress($"{mapping}.SafeMotion.Header.NumberOfActiveSlots"), 
                                           PlcReadReference.FromAddress($"{mapping}.SafeMotion.Header.States.ChecksumInvalid"));

            var result2 = await _papper.ReadBytesAsync(new List<PlcReadReference> { PlcReadReference.FromAddress($"{mapping}.SafeMotion") });
            await result1;
            t.Stop();
        }

        [Fact]
        public async Task MixedAccessWithDataChangeTest()
        {
            var mapping = "DB_Safety2";

            var t = new Stopwatch();
            t.Start();
            short value = -1;
            await _papper.WriteAsync(PlcWriteReference.FromAddress($"{mapping}.SafeMotion.Header.NumberOfActiveSlots", (short)0));

            var sub = _papper.SubscribeDataChanges((s, e) => 
            {
               value = (short)e[$"{mapping}.SafeMotion.Header.NumberOfActiveSlots"];
            }, PlcWatchReference.FromAddress($"{mapping}.SafeMotion.Header.NumberOfActiveSlots", 10),
                                                                  PlcWatchReference.FromAddress($"{mapping}.SafeMotion.Header.States.ChecksumInvalid", 10));
            var result2 = await _papper.ReadBytesAsync(new List<PlcReadReference> { PlcReadReference.FromAddress($"{mapping}.SafeMotion") });

            await _papper.WriteAsync(PlcWriteReference.FromAddress($"{mapping}.SafeMotion.Header.NumberOfActiveSlots", (short)1));

            await Task.Delay(2000);

            Assert.Equal((short)1, value);

            sub.Dispose();
            t.Stop();
        }

        [Fact]
        public async Task MixedAccessWithDataChangeTest2()
        {
            var mapping = "DB_Safety2";

            var t = new Stopwatch();
            t.Start();
            short value = -1;
            await _papper.WriteAsync(PlcWriteReference.FromAddress($"{mapping}.SafeMotion.Header.NumberOfActiveSlots", (short)0));

            var result2 = await _papper.ReadBytesAsync(new List<PlcReadReference> { PlcReadReference.FromAddress($"{mapping}.SafeMotion") });
            var sub = _papper.SubscribeDataChanges((s, e) =>
            {
                value = (short)e[$"{mapping}.SafeMotion.Header.NumberOfActiveSlots"];
            }, PlcWatchReference.FromAddress($"{mapping}.SafeMotion.Header.NumberOfActiveSlots", 10),
                                                                  PlcWatchReference.FromAddress($"{mapping}.SafeMotion.Header.States.ChecksumInvalid", 10));
            

            await _papper.WriteAsync(PlcWriteReference.FromAddress($"{mapping}.SafeMotion.Header.NumberOfActiveSlots", (short)1));

            await Task.Delay(2000);

            Assert.Equal((short)1, value);

            sub.Dispose();
            t.Stop();
        }





        [Fact]
        public void TestStructuralAllWithSerializerAccess()
        {
            var mapping = "DB_Safety2";

            var t = new Stopwatch();
            var ser = new PlcDataMapperSerializer();
            t.Start();
            var address = _papper.GetAddressOf(PlcReadReference.FromAddress($"{mapping}")).RawAddress<byte>();
            var result = _papper.ReadAsync(PlcReadReference.FromAddress(address)).GetAwaiter().GetResult().FirstOrDefault();
            var x = ser.Deserialize<DB_Safety>((byte[])result.Value);
            t.Stop();
        }



        [Fact]
        public void TestDataChange()
        {
            var sleepTime = 4000;
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
                subscription.AddItems(originData.Keys.Select(variable => PlcWatchReference.FromAddress($"{mapping}.{variable}", 100)));
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
                                   Assert.Equal(2, res.Results.Count());
                               }
                               else
                               {
                                   Assert.Equal(3, res.Results.Count());
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
                                   catch (Exception)
                                   {

                                   }
                               }

                               are.Set();
                           } 
                       }
                   }
                   catch(Exception)
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
            var items = writeData.Keys.Select(variable => PlcWatchReference.FromAddress($"DB15.{variable}", 100)).ToArray();

            using (var sub = _papper.CreateSubscription())
            {
                Assert.True(sub.TryAddItems(items));
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
            var items = writeData.Keys.Select(variable => PlcWatchReference.FromAddress($"DB15.{variable}", 100)).ToArray();

            using (var sub = _papper.CreateSubscription())
            {
                Assert.True(sub.TryAddItems(items));
                var c = sub.DetectChangesAsync();  // returns because we start a new detection
                Thread.Sleep(500);
                var res = await c;
                Assert.False(res.IsCanceled);
                Assert.False(res.IsCompleted);
                Assert.NotNull(res.Results);
                Assert.Equal(2, res.Results.Count());

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
        public void PerformReadWriteRaw()
        {
            var papper = new PlcDataMapper(960, Papper_OnRead, Papper_OnWrite);
            var readResults = papper.ReadAsync(PlcReadReference.FromAddress("DB2000.W2")).GetAwaiter().GetResult();
            var writeResults = papper.WriteAsync(PlcWriteReference.FromAddress("DB2000.W2", (UInt16)3)).GetAwaiter().GetResult();

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
            var subscription = _papper.SubscribeDataChanges(callback, writeData.Keys.Select(variable => PlcWatchReference.FromAddress($"DB15.{variable}", 100)).ToArray());


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
            Assert.True(dbData.Slice(0, 2).Span.SequenceEqual(new byte[] { 35, 5 }));
            Assert.True(dbData.Slice(2, 5).Span.SequenceEqual("TEST1".ToByteArray(5)));

            Assert.True(dbData.Slice(152, 2).Span.SequenceEqual(new byte[] { 35, 5 }));
            Assert.True(dbData.Slice(154, 5).Span.SequenceEqual("TEST5".ToByteArray(5)));
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

            Assert.Throws<InvalidVariableException>(() =>
           {
               var result = _papper.ReadAsync(PlcReadReference.FromRoot(mapping, accessDict.Keys.ToArray()).ToArray()).GetAwaiter().GetResult();
               Assert.Empty(result);
           });
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

        [Fact]
        public void TestExternalDataChange()
        {
            
            var papper = new PlcDataMapper(960, Papper_OnRead, Papper_OnWrite, UpdateHandler, ReadMetaData, OptimizerType.Items);
            papper.AddMapping(typeof(DB_Safety));
            MockPlc.OnItemChanged = (items) => 
            {
                papper.OnDataChanges(items.Select(i => new DataPack
                {
                    Selector = i.Selector,
                    Offset = i.Offset,
                    Length = i.Length,
                    BitMask = i.BitMask,
                    ExecutionResult = ExecutionResult.Ok
                }.ApplyData(i.Data)));
            };
            var sleepTime = 10000;
            var mapping = "DB_Safety";
            var intiState = true;
            var originData = new Dictionary<string, object> {
                    { "SafeMotion.Slots[16].SlotId", (byte)0},
                    { "SafeMotion.Slots[16].HmiId", (UInt32)0},
                    { "SafeMotion.Slots[16].Commands.TakeoverPermitted", false },
                };
            var writeData = new Dictionary<string, object> {
                    { "SafeMotion.Slots[16].SlotId", (byte)3},
                    { "SafeMotion.Slots[16].HmiId", (UInt32)4},
                    { "SafeMotion.Slots[16].Commands.TakeoverPermitted", false },
                };
            var are = new AutoResetEvent(false);

            // write initial state
            papper.WriteAsync(PlcWriteReference.FromRoot(mapping, originData.ToArray()).ToArray()).GetAwaiter().GetResult();

            using (var subscription = papper.CreateSubscription(ChangeDetectionStrategy.Event))
            {
                subscription.AddItems(originData.Keys.Select(variable => PlcWatchReference.FromAddress($"{mapping}.{variable}", 100)));
                var t = Task.Run(async () =>
                {
                    try
                    {
                        while (!subscription.Watching.IsCompleted)
                        {
                            var res = await subscription.DetectChangesAsync();

                            if (!res.IsCompleted && !res.IsCanceled)
                            {
                                _output.WriteLine($"Changed: initial state is {intiState}");
                                if (!intiState)
                                {
                                    Assert.Equal(2, res.Results.Count());
                                }
                                else
                                {
                                    Assert.Equal(3, res.Results.Count());
                                }

                                foreach (var item in res.Results)
                                {
                                    try
                                    {
                                        _output.WriteLine($"Changed: {item.Variable} = {item.Value}");

                                        if (!intiState)
                                            Assert.Equal(writeData[item.Variable], item.Value);
                                        else
                                            Assert.Equal(originData[item.Variable], item.Value);
                                    }
                                    catch (Exception )
                                    {

                                    }
                                }

                                are.Set();
                            }
                        }
                    }
                    catch (Exception )
                    {

                    }
                });

                //waiting for initialize
                Assert.True(are.WaitOne(sleepTime), "waiting for initialize");
                intiState = false;
                var writeResults = papper.WriteAsync(PlcWriteReference.FromRoot(mapping, writeData.ToArray()).ToArray()).GetAwaiter().GetResult();
                foreach (var item in writeResults)
                {
                    Assert.Equal(ExecutionResult.Ok, item.ActionResult);
                }
                //waiting for write update
                Assert.True(are.WaitOne(sleepTime), "waiting for write update");

                //test if data change only occurred if data changed
                Assert.False(are.WaitOne(sleepTime), $"test if data change only occurred if data changed");

            }
        }

        [Fact]
        public void TestInvalidMappings()
        {

            var papper = new PlcDataMapper(960, Papper_OnRead, Papper_OnWrite, UpdateHandler, ReadMetaData, OptimizerType.Items);
            papper.AddMapping(typeof(DB_Safety));

            using (var subscription = papper.CreateSubscription(ChangeDetectionStrategy.Event))
            {
                Assert.True(subscription.TryAddItems(PlcWatchReference.FromAddress("DB_Safety.SafeMotion.Slots[0]", 100)));
                Assert.False(subscription.TryAddItems(PlcWatchReference.FromAddress("Test.XY", 100)));
                Assert.False(subscription.TryAddItems(PlcWatchReference.FromAddress("DB_Safety.XY", 100)));


                Assert.Throws<InvalidVariableException>( () => subscription.AddItems(PlcWatchReference.FromAddress("Test.XY", 100)));
                Assert.Throws<InvalidVariableException>( () => subscription.AddItems(PlcWatchReference.FromAddress("DB_Safety.XY", 100)));
            }
        }

        [Fact]
        public void TestGetAddressOf()
        {
            var mapping = "DB_Safety2";
            var result = _papper.GetAddressOf(PlcReadReference.FromAddress($"{mapping}.SafeMotion.Slots"));


            Assert.Equal(14, result.Offset.Bytes);
            Assert.Equal(8670, result.Size.Bytes);

        }


        [Fact]
        public void TestGetAddressOfWString()
        {
            var mapping = "DB_MotionHMI";
            var result = _papper.GetAddressOf(PlcReadReference.FromAddress($"{mapping}.HMI.MotionLine[8].Txt.Position[1]"));


            Assert.Equal(14, result.Offset.Bytes);
            Assert.Equal(8670, result.Size.Bytes);

        }


        [Fact]
        public void TestWriteRawDataToStruct()
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

            var s = new PlcDataMapperSerializer();
            accessDict["SafeMotion.Header"] = s.Serialize(header);

            var writeResults = _papper.WriteAsync(PlcWriteReference.FromRoot(mapping, accessDict.ToArray()).ToArray()).GetAwaiter().GetResult();
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

        private Task UpdateHandler(IEnumerable<DataPack> monitoring, bool add = true)
        {
            foreach (var item in monitoring)
            {
                MockPlc.UpdateDataChangeItem(item, !add);
            }
            return Task.CompletedTask;
        }

        private Task ReadMetaData(IEnumerable<MetaDataPack> packs)
        {
            foreach (var item in packs)
            {
                item.ExecutionResult = ExecutionResult.Error;
            }
            return Task.CompletedTask;
        }

        private static Task Papper_OnRead(IEnumerable<DataPack> reads)
        {
            var result = reads.ToList();
            foreach (var item in result)
            {
                Console.WriteLine($"OnRead: selector:{item.Selector}; offset:{item.Offset}; length:{item.Length}");
                var res = MockPlc.GetPlcEntry(item.Selector, item.Offset + item.Length).Data.Slice(item.Offset, item.Length);
                if(!res.IsEmpty)
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

        private static Task Papper_OnWrite(IEnumerable<DataPack> reads)
        {
            var result = reads.ToList();
            foreach (var item in result)
            {
                if (item.BitMask == 0)
                {
                    Console.WriteLine($"OnWrite: selector:{item.Selector}; offset:{item.Offset}; length:{item.Length}");
                    item.Data.Slice(0, item.Length).CopyTo(MockPlc.GetPlcEntry(item.Selector, item.Offset + item.Length).Data.Slice(item.Offset, item.Length));
                    item.ExecutionResult = ExecutionResult.Ok;
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
                                var b = MockPlc.GetPlcEntry(item.Selector, item.Offset + 1).Data.Span[item.Offset];
                                MockPlc.GetPlcEntry(item.Selector, item.Offset + 1).Data.Span[item.Offset] = b.SetBit(i, bItem.GetBit(i));
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
                if (parent is IDictionary<string, object> dictionary)
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
            if (obj1 is IEnumerable list1 &&
                obj2 is IEnumerable list2 )
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
            if (obj1 is IDictionary<string, object> dictionary1 &&
                obj2 is IDictionary<string, object> dictionary2)
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
