using Papper;
using Papper.Extensions.Metadata;
using Papper.Extensions.Notification;
using Papper.Tests.Mappings;
using Papper.Tests.Util;
using PapperTests.Mappings;
using System;
using System.Buffers.Binary;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

//run in sequence because of db sharing
[assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly)]
namespace Papper.Tests
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public sealed class PlcDataMapperTests : IDisposable
    {
        private readonly PlcDataMapper _papper = new PlcDataMapper(960, Papper_OnRead, Papper_OnWrite);
        private readonly ITestOutputHelper _output;

        public PlcDataMapperTests(ITestOutputHelper output)
        {
            _output = output;
            _papper.AddMapping(typeof(DB_Safety));
            _papper.AddMapping(typeof(ArrayTestMapping));
            _papper.AddMapping(typeof(StringArrayTestMapping));
            _papper.AddMapping(typeof(PrimitiveValuesMapping));
            _papper.AddMapping(typeof(DB_MotionHMI));
            _papper.AddMapping(typeof(DB_BST1_ChargenRV));
            _papper.AddMapping(typeof(MSpindleInterface));


        }

        [Theory]
        [InlineData(nameof(DB_Safety), 4596)]
        [InlineData("ARRAY_TEST_MAPPING_1", 105030)]
        [InlineData("STRING_ARRAY_TEST_MAPPING", 21)]
        [InlineData(nameof(PrimitiveValuesMapping), 6)]
        [InlineData(nameof(DB_MotionHMI), 725)]
        public void TestVariables(string mapping, int expectedVariables)
        {
            var vars = _papper.GetVariablesOf(mapping);
            Assert.Equal(expectedVariables, vars.Count());
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
                    { "SafeMotion.Header.NumberOfActiveSlots",(short) 1 },
                    { "SafeMotion.Slots[0].AggregateDBNummer", (short)2},
                    { "SafeMotion.Slots[0].AggregateOffset", (short)3},
                    { "SafeMotion.Slots[0].SafeSlotVersion", (short)4},
                    { "SafeMotion.Slots[100].AggregateDBNummer", (short)5},
                    { "SafeMotion.Slots[100].AggregateOffset",(short)6},
                    { "SafeMotion.Slots[100].SafeSlotVersion", (short)7},
                    { "SafeMotion.Slots[254].AggregateDBNummer", (short)8},
                    { "SafeMotion.Slots[254].AggregateOffset", (short)9},
                    { "SafeMotion.Slots[254].SafeSlotVersion", (short)10},
                };

            Test(mapping, accessDict, (short)0);
        }

        [Fact]
        public void UInt16AccessTest()
        {
            var mapping = "DB_Safety";
            var accessDict = new Dictionary<string, object> {
                    { "SafeMotion.Slots[0].UnitChecksum", (ushort)2},
                    { "SafeMotion.Slots[100].UnitChecksum", (ushort)5},
                    { "SafeMotion.Slots[254].UnitChecksum", (ushort)8},

                };

            Test(mapping, accessDict, (ushort)0);
        }

        [Fact]
        public void UInt32AccessTest()
        {
            var mapping = "DB_Safety";
            var accessDict = new Dictionary<string, object> {
                    { "SafeMotion.Slots[0].HmiId", (uint)3},
                    { "SafeMotion.Slots[0].AccessRightReqFromHmiId", (uint)4},
                    { "SafeMotion.Slots[100].HmiId",(uint)6},
                    { "SafeMotion.Slots[100].AccessRightReqFromHmiId", (uint)7},
                    { "SafeMotion.Slots[254].HmiId", (uint)9},
                    { "SafeMotion.Slots[254].AccessRightReqFromHmiId", (uint)10},
                };

            Test(mapping, accessDict, (uint)0);
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

            Test(mapping, accessDict, new DateTime(1990, 1, 1));  //01.01.1900
        }


        [Fact]
        public void SingleAccessTest()
        {
            var mapping = "PrimitiveValuesMapping";
            var accessDict = new Dictionary<string, object> {
                    { "Single", (float)2.2},
                };

            Test(mapping, accessDict, (float)0);
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

            var result = _papper.ReadAsync(accessDict.Keys.Select(variable => PlcReadReference.FromAddress($"{mapping}.{variable}")).ToArray()).GetAwaiter().GetResult();
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

            var result2 = await _papper.ReadBytesAsync(new List<PlcReadReference> { PlcReadReference.FromAddress($"{mapping}.SafeMotion") }).ConfigureAwait(false);
            await result1.ConfigureAwait(false);
            t.Stop();
        }

        [Fact]
        public async Task MixedAccessWithDataChangeTest()
        {
            var mapping = "DB_Safety2";

            var t = new Stopwatch();
            t.Start();
            short value = -1;
            await _papper.WriteAsync(PlcWriteReference.FromAddress($"{mapping}.SafeMotion.Header.NumberOfActiveSlots", (short)0)).ConfigureAwait(false);

            var sub = _papper.SubscribeDataChanges((s, e) =>
            {
                value = (short)e[$"{mapping}.SafeMotion.Header.NumberOfActiveSlots"];
            }, PlcWatchReference.FromAddress($"{mapping}.SafeMotion.Header.NumberOfActiveSlots", 10),
                                                                  PlcWatchReference.FromAddress($"{mapping}.SafeMotion.Header.States.ChecksumInvalid", 10));
            var result2 = await _papper.ReadBytesAsync(new List<PlcReadReference> { PlcReadReference.FromAddress($"{mapping}.SafeMotion") }).ConfigureAwait(false);

            await _papper.WriteAsync(PlcWriteReference.FromAddress($"{mapping}.SafeMotion.Header.NumberOfActiveSlots", (short)1)).ConfigureAwait(false);

            await Task.Delay(2000).ConfigureAwait(false);

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
            await _papper.WriteAsync(PlcWriteReference.FromAddress($"{mapping}.SafeMotion.Header.NumberOfActiveSlots", (short)0)).ConfigureAwait(false);

            var result2 = await _papper.ReadBytesAsync(new List<PlcReadReference> { PlcReadReference.FromAddress($"{mapping}.SafeMotion") }).ConfigureAwait(false);
            var sub = _papper.SubscribeDataChanges((s, e) =>
            {
                value = (short)e[$"{mapping}.SafeMotion.Header.NumberOfActiveSlots"];
            }, PlcWatchReference.FromAddress($"{mapping}.SafeMotion.Header.NumberOfActiveSlots", 10),
                                                                  PlcWatchReference.FromAddress($"{mapping}.SafeMotion.Header.States.ChecksumInvalid", 10));


            await _papper.WriteAsync(PlcWriteReference.FromAddress($"{mapping}.SafeMotion.Header.NumberOfActiveSlots", (short)1)).ConfigureAwait(false);

            await Task.Delay(2000).ConfigureAwait(false);

            Assert.Equal((short)1, value);

            sub.Dispose();
            t.Stop();
        }



        [Fact]
        public async Task DataChanegOnBitsTest()
        {
            var t = new Stopwatch();
            t.Start();
            await _papper.WriteAsync(PlcWriteReference.FromAddress($"DB_IDAT_MSpindleData1.IDATInterface.IDATtoPLC.Toggle", false)).ConfigureAwait(false);

            var sub = _papper.SubscribeDataChanges((s, e) =>
            {

            }, PlcWatchReference.FromAddress($"DB_IDAT_MSpindleData.IDATInterface.PLCtoIDAT.UpdateRequired", 10),
               PlcWatchReference.FromAddress($"DB_IDAT_MSpindleData.IDATInterface.IDATtoPLC.Toggle", 10),
               PlcWatchReference.FromAddress($"DB_IDAT_MSpindleData.IDATInterface.PLCtoIDAT.WriteEnable", 10));

            await Task.Delay(100).ConfigureAwait(false);
            await _papper.WriteAsync(PlcWriteReference.FromAddress($"DB_IDAT_MSpindleData.IDATInterface.PLCtoIDAT.UpdateRequired", true)).ConfigureAwait(false);
            //await Task.Delay(100);
            //await _papper.WriteAsync(PlcWriteReference.FromAddress($"DB_IDAT_MSpindleData.IDATInterface.IDATtoPLC.Toggle", false));

            await Task.Delay(30000).ConfigureAwait(false);

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
        public async Task TestWriteSerializedData()
        {
            var ser = new PlcDataMapperSerializer();

            var value = ser.Serialize(TimeTransformationRule.FromTimeZoneInfo(TimeZoneInfo.Local));



            await _papper.WriteAsync(PlcWriteReference.FromAddress($"DB301.S108,5", CultureInfo.CurrentCulture.Name),
                                    PlcWriteReference.FromAddress($"DB301.B10,{value.Length}", value),
                                    PlcWriteReference.FromAddress($"DB301.DT2", DateTime.Now),
                                    PlcWriteReference.FromAddress($"DB301.X106.0", true)).ConfigureAwait(false);
        }





        [Theory]
        [InlineData("DB2000.W2", (ushort)3)]
        [InlineData("DB2001.X0.0", true)]
        [InlineData("DB2002.X0_1", true)]
        [InlineData("DB2003.X0.0,8", new bool[] { false, false, true, true, false, false, true, true })]
        [InlineData("DB2004.X0.1,4", new bool[] { false, false, true, true })]
        [InlineData("DB2005.X1.1,4", new bool[] { false, false, true, true })]
        [InlineData("DB2006.X0.0,10", new bool[] { false, false, true, true, false, false, true, true, false, false })]
        [InlineData("DB2007.X0.0,16", new bool[] { false, false, true, true, false, false, true, true, false, false, true, true, false, false, true, true })]
        [InlineData("DB2008.X0.4,16", new bool[] { false, false, true, true, false, false, true, true, false, false, true, true, false, false, true, true })]
        public void PerformReadWriteRaw(string address, object value)
        {
            using (var papper = new PlcDataMapper(960, Papper_OnRead, Papper_OnWrite))
            {
                var readResults = papper.ReadAsync(PlcReadReference.FromAddress(address)).GetAwaiter().GetResult();
                var writeResults = papper.WriteAsync(PlcWriteReference.FromAddress(address, value)).GetAwaiter().GetResult();
                var afterWriteReadResults = papper.ReadAsync(PlcReadReference.FromAddress(address)).GetAwaiter().GetResult();

                Assert.Equal(value, afterWriteReadResults[0].Value);
            }
        }



        [Fact]
        public void PerformReadStruct()
        {
            var address = _papper.GetAddressOf(PlcReadReference.FromAddress("DB_BST1_ChargenRV")).RawAddress<byte>();
            var readResults = _papper.ReadAsync(PlcReadReference.FromAddress(address)).GetAwaiter().GetResult();
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
            var data = new Span<byte>(new byte[] { 0x01, 0x02, 0x03, 0x04 });

            var v2 = BinaryPrimitives.ReadInt32BigEndian(data);
            var v3 = BinaryPrimitives.ReadInt32LittleEndian(data);

            var data1 = new Span<byte>(new byte[4]);
            var s = 25.4f;
            BinaryPrimitives.WriteInt32BigEndian(data1, Convert.ToInt32(s));
            var res = Convert.ToSingle(BinaryPrimitives.ReadInt32BigEndian(data1));

            var data4 = new Span<byte>(new byte[4]);
            Converter.WriteSingleBigEndian(data4, s);
            var x4 = Converter.ReadSingleBigEndian(data4);
        }




        [Fact]
        public void TestInvalidMappings()
        {

            using var papper = new PlcDataMapper(960, Papper_OnRead, Papper_OnWrite, UpdateHandler, ReadMetaData, OptimizerType.Items);
            papper.AddMapping(typeof(DB_Safety));

            using (var subscription = papper.CreateSubscription(ChangeDetectionStrategy.Event))
            {
                Assert.True(subscription.TryAddItems(PlcWatchReference.FromAddress("DB_Safety.SafeMotion.Slots[0]", 100)));
                Assert.False(subscription.TryAddItems(PlcWatchReference.FromAddress("Test.XY", 100)));
                Assert.False(subscription.TryAddItems(PlcWatchReference.FromAddress("DB_Safety.XY", 100)));


                Assert.Throws<InvalidVariableException>(() => subscription.AddItems(PlcWatchReference.FromAddress("Test.XY", 100)));
                Assert.Throws<InvalidVariableException>(() => subscription.AddItems(PlcWatchReference.FromAddress("DB_Safety.XY", 100)));
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


            Assert.Equal(9442, result.Offset.Bytes);
            Assert.Equal(54, result.Size.Bytes);

        }

        [Fact]
        public void TestGetAddressOfType()
        {
            var mapping = "DB_MotionHMI";
            var result1 = _papper.GetAddressOf(PlcReadReference.FromAddress($"{mapping}.HMI.MotionLine[8].Txt.Position[1]"));

            mapping = "DB_Safety2";
            var result2 = _papper.GetAddressOf(PlcReadReference.FromAddress($"{ mapping}.SafeMotion.Slots"));
            var result3 = _papper.GetAddressOf(PlcReadReference.FromAddress($"{ mapping}.SafeMotion.Slots[2]"));
            var result4 = _papper.GetAddressOf(PlcReadReference.FromAddress($"{ mapping}.SafeMotion.Slots[2].SlotId"));
            var result5 = _papper.GetAddressOf(PlcReadReference.FromAddress($"{ mapping}.SafeMotion.Slots[2].SafeSlotVersion"));



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



        [Fact]
        public async Task ReadBitsAsyncTest()
        {
            var x = await _papper.ReadAsync(PlcReadReference.FromAddress("DB_IDAT_MSpindleData.IDATInterface.PLCtoIDAT.WriteEnable")).ConfigureAwait(false);
        }


        #region Helper


        private void Test<T>(string mapping, Dictionary<string, object> accessDict, T defaultValue)
        {
            //Initial read to ensure all are false
            var toRead = accessDict.Keys.Select(variable => PlcReadReference.FromAddress($"{mapping}.{variable}")).ToArray();
            var result = _papper.ReadAsync(toRead).GetAwaiter().GetResult();
            Assert.Equal(accessDict.Count, result.Length);
            foreach (var item in result)
            {
                Assert.Equal(defaultValue, (T)item.Value);
            }

            //Write the value
            _papper.WriteAsync(PlcWriteReference.FromRoot(mapping, accessDict.ToArray()).ToArray()).GetAwaiter().GetResult();

            //Second read to ensure correct written
            result = _papper.ReadAsync(accessDict.Keys.Select(variable => PlcReadReference.FromAddress($"{mapping}.{variable}")).ToArray()).GetAwaiter().GetResult();
            Assert.Equal(accessDict.Count, result.Length);
            foreach (var item in result)
            {
                Assert.Equal((T)accessDict[item.Variable], (T)item.Value);
            }
        }

        /// <summary>
        /// This method remove the nanoseconds from the date time, because plc's could not handle that
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private DateTime Normalize(DateTime dt) => dt.AddTicks((dt.Ticks % 10000) * -1);

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

        private static Task Papper_OnWrite(IEnumerable<DataPack> reads)
        {
            var result = reads.ToList();
            foreach (var item in result)
            {
                var entry = MockPlc.GetPlcEntry(item.Selector, item.Offset + item.Length);
                if (!item.HasBitMask)
                {
                    Console.WriteLine($"OnWrite: selector:{item.Selector}; offset:{item.Offset}; length:{item.Length}");
                    item.Data.Slice(0, item.Length).CopyTo(entry.Data.Slice(item.Offset, item.Length));
                    item.ExecutionResult = ExecutionResult.Ok;
                }
                else
                {
                    var lastItem = item.Data.Length - 1;
                    for (var j = 0; j < item.Data.Length; j++)
                    {
                        var bItem = item.Data.Span[j];
                        if (j > 0 && j < lastItem)
                        {
                            entry.Data.Span[item.Offset + j] = item.Data.Span[j];
                            item.ExecutionResult = ExecutionResult.Ok;
                        }
                        else
                        {
                            var bm = j == 0 ? item.BitMaskBegin : (j == lastItem) ? item.BitMaskEnd : (byte)0;
                            if (bm == 0xFF)
                            {
                                entry.Data.Span[item.Offset + j] = item.Data.Span[j];
                                item.ExecutionResult = ExecutionResult.Ok;
                            }
                            else if (bm > 0)
                            {
                                for (var i = 0; i < 8; i++)
                                {
                                    var bit = bm.GetBit(i);
                                    if (bit)
                                    {
                                        var b = entry.Data.Span[item.Offset + j];
                                        entry.Data.Span[item.Offset + j] = b.SetBit(i, bItem.GetBit(i));
                                        item.ExecutionResult = ExecutionResult.Ok;
                                        bm = bm.SetBit(i, false);
                                        if (bm == 0)
                                        {
                                            break;
                                        }
                                    }
                                }
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
                if (!item.PropertyType.Namespace.StartsWith("System", false, CultureInfo.InvariantCulture))
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
                {
                    dictionary[name] = value;
                }
            }
        }

        private static bool AreDataEqual(object obj1, object obj2)
        {
            var t1 = obj1.GetType();
            var t2 = obj2.GetType();

            if (t1 == t2) { return t1 != typeof(ExpandoObject) ? ElementEqual(obj1, obj2) : DynamicObjectCompare(obj1, obj2); }
            try { return ElementEqual(obj1, Convert.ChangeType(obj2, t1, CultureInfo.InvariantCulture)); } catch { }
            return false;
        }

        private static bool ElementEqual(object obj1, object obj2)
        {
            if (obj1 is IEnumerable list1 &&
                obj2 is IEnumerable list2)
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
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return e1 == e2; //Length not the same?
                    }
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
                    if (!dictionary2.TryGetValue(o1.Key, out var o2) || !AreDataEqual(o1.Value, o2))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void Dispose() => _papper?.Dispose();

        #endregion
    }
}
