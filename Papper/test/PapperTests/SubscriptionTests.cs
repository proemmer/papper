using Papper;
using Papper.Extensions.Notification;
using PapperTests.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnitTestSuit.Mappings;
using UnitTestSuit.Util;
using Xunit;
using Xunit.Abstractions;

namespace PapperTests
{
    public class SubscriptionTests
    {
        private PlcDataMapper _papper = new PlcDataMapper(960, Papper_OnRead, Papper_OnWrite);
        private readonly ITestOutputHelper _output;

        public SubscriptionTests(ITestOutputHelper output)
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


        [Fact]
        public void AddAndRemoveSubscriptionsTest()
        {
            var writeData = new Dictionary<string, object> {
                    { "W88", (UInt16)3},
                    { "X99.0", true  },
                };
            var items = writeData.Keys.Select(variable => PlcWatchReference.FromAddress($"DB15.{variable}", 100)).ToArray();

            using (var sub = _papper.CreateSubscription())
            {
                Assert.False(sub.HasVariables);
                Assert.True(sub.TryAddItems(items));
                var c = sub.DetectChangesAsync();  // returns because we start a new detection
                Thread.Sleep(100);
                Assert.True(sub.HasVariables);
                Assert.Equal(2, sub.Count);
                Assert.True(sub.RemoveItems(items.FirstOrDefault()));
                Assert.True(sub.RemoveItems(items.FirstOrDefault())); // <- modified is already true
                c = sub.DetectChangesAsync();      // returns because we modified the detection
                Assert.False(sub.RemoveItems(items.FirstOrDefault()));

                Thread.Sleep(100);
                Assert.True(sub.RemoveItems(items.LastOrDefault()));
                c = sub.DetectChangesAsync();      // returns because we modified the detection
                Thread.Sleep(100);

                Assert.Equal(0, sub.Count);
                Assert.False(sub.HasVariables);
            }
        }

        [Fact]
        public void AddAndRemoveAllItemsFromSubscriptionsTest()
        {
            var mapping = "DB_SafetyDataChange1";
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
            using (var are = new AutoResetEvent(false))
            {
                using (var subscription = _papper.CreateSubscription())
                {
                    var items = originData.Keys.Select(variable => PlcWatchReference.FromAddress($"{mapping}.{variable}", 100)).ToArray();
                    subscription.AddItems(items);
                    subscription.RemoveItems(items);
                }
            }
        }


        [Fact]
        public void StartSubscriptionWithOutVaraiblesTest()
        {
            using (var sub = _papper.CreateSubscription())
            {
                Assert.False(sub.HasVariables);
                var c = sub.DetectChangesAsync();  // returns because we start a new detection
                Thread.Sleep(100);
                Assert.False(sub.HasVariables);
            }
        }


        [Fact]
        public async void DuplicateDetectionTest()
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
        public async void SubscriptionCancellationTest()
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
        public void DataChangeTest()
        {
            var sleepTime = 10000;
            var mapping = "DB_SafetyDataChange1";
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
            using (var are = new AutoResetEvent(false))
            {
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
                        catch (Exception)
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
                    for (int j = 0; j < item.Data.Length; j++)
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
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return Task.CompletedTask;
        }


    }
}
