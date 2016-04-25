using Papper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnitTestSuit.Mappings;
using UnitTestSuit.Util;
using Xunit;

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

            _papper.AddMapping(typeof(DB_InaxSafety));
            _papper.AddMapping(typeof(ArrayTestMapping));
        }

        [Fact]
        public void BitAccessTest()
        {
            var mapping = "DB_InaxSafety";
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
            var mapping = "DB_InaxSafety";
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
            var mapping = "DB_InaxSafety";
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
            var mapping = "DB_InaxSafety";
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
            var mapping = "DB_InaxSafety";
            var accessDict = new Dictionary<string, object> {
                    { "SafeMotion.Header.Generated", Normalize(DateTime.Now)},
                    { "SafeMotion.Slots[0].UnitTimestamp", Normalize(DateTime.Now)},
                    { "SafeMotion.Slots[100].UnitTimestamp", Normalize(DateTime.Now)},
                    { "SafeMotion.Slots[254].UnitTimestamp",Normalize( DateTime.Now)},
                };

            Test(mapping, accessDict, new DateTime(599266080000000000));  //01.01.1900
        }

        [Fact]
        public void BigByteArrayAccessTest()
        {
            var mapping = "ARRAY_TEST_MAPPING";
            var accessDict = new Dictionary<string, object> {
                    { "BigByteArray", Enumerable.Repeat<byte>(0x01,50000).ToArray()},
                };

            Test(mapping, accessDict, Enumerable.Repeat<byte>(0x00, 50000).ToArray());
        }

        [Fact]
        public void BigCharArrayAccessTest()
        {
            var mapping = "ARRAY_TEST_MAPPING";
            var accessDict = new Dictionary<string, object> {
                    { "BigCharArray", Enumerable.Repeat<char>('a',50000).ToArray()},
                };

            Test(mapping, accessDict, Enumerable.Repeat<char>(default(char), 50000).ToArray());
        }

        [Fact]
        public void BigIntArrayAccessTest()
        {
            var mapping = "ARRAY_TEST_MAPPING";
            var accessDict = new Dictionary<string, object> {
                    { "BigIntArray", Enumerable.Repeat<Int32>(2,5000).ToArray()},
                };

            Test(mapping, accessDict, Enumerable.Repeat<int>((int)0, 5000).ToArray());
        }


        #region Helper


        private void Test<T>(string mapping, Dictionary<string, object> accessDict, T defaultValue)
        {
            //Initial read to ensure all are false
            var result = _papper.Read(mapping, accessDict.Keys.ToArray());
            Assert.Equal(accessDict.Count, result.Count);
            foreach (var item in result)
                Assert.Equal(defaultValue, (T)item.Value);

            //Write the value
            Assert.True(_papper.Write(mapping, accessDict));

            //Second read to ensure correct written
            result = _papper.Read(mapping, accessDict.Keys.ToArray());
            Assert.Equal(accessDict.Count, result.Count);
            foreach (var item in result)
                Assert.Equal((T)accessDict[item.Key], (T)item.Value);
        }

        /// <summary>
        /// This method remove the nanoseconds from the datetime, because plc's could not handle that
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private DateTime Normalize(DateTime dt)
        {
            return dt.AddTicks((dt.Ticks % 10000) * -1);
        }

        private static byte[] Papper_OnRead(string selector, int offset, int length)
        {
            Console.WriteLine($"OnRead: selector:{selector}; offset:{offset}; length:{length}");
            return MockPlc.GetPlcEntry(selector, offset + length).Data.SubArray(offset, length);
        }

        private static bool Papper_OnWrite(string selector, int offset, byte[] data, byte bitMask = 0)
        {
            try
            {
                var length = data.Length;
                if (bitMask == 0)
                {
                    Console.WriteLine($"OnWrite: selector:{selector}; offset:{offset}; length:{length}");
                    Array.Copy(data, 0, MockPlc.GetPlcEntry(selector, offset + length).Data, offset, length);
                }
                else
                {
                    foreach (var item in data)
                    {
                        var bm = bitMask;
                        for (var i = 0; i < 8; i++)
                        {
                            var bit = bm.GetBit(i);
                            if (bit)
                            {
                                Console.WriteLine($"OnWriteBit: selector:{selector}; offset:{offset + 1};");
                                var b = MockPlc.GetPlcEntry(selector, offset + 1).Data[offset];
                                MockPlc.GetPlcEntry(selector, offset + 1).Data[offset] = b.SetBit(i, item.GetBit(i));
                                bm = bm.SetBit(i, false);
                                if (bm == 0)
                                    break;
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion
    }
}
