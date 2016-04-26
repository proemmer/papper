using Papper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
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
        public void ArrayElementsAccessTest()
        {
            var mapping = "ARRAY_TEST_MAPPING";
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

        [Fact]
        public void TestStructuralAccess()
        {
            var mapping = "DB_InaxSafety";
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

            var result = _papper.Read(mapping, accessDict.Keys.ToArray());
            Assert.Equal(accessDict.Count, result.Count);
            Assert.True(_papper.Write(mapping, accessDict));
            var result2 = _papper.Read(mapping, accessDict.Keys.ToArray());
            Assert.Equal(accessDict.Count, result2.Count);
            Assert.False(AreDataEqual(result, result2));
            Assert.True(AreDataEqual(ToExpando(header), result2.Values.FirstOrDefault()));
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
        /// This method remove the nanoseconds from the date time, because plc's could not handle that
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
                    object o2;
                    if (!dictionary2.TryGetValue(o1.Key, out o2) || !AreDataEqual(o1.Value, o2))
                        return false;

                }
            }
            return true;
        }

        #endregion
    }
}
