using Papper;
using Papper.Extensions.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestSuit.Mappings;
using UnitTestSuit.Util;
using Xunit;
using Xunit.Abstractions;

namespace PapperTests
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class MemoryUsageTests
    {
        private PlcDataMapper _papper = new PlcDataMapper(960, Papper_OnRead, Papper_OnWrite);
        private readonly ITestOutputHelper _output;

        public MemoryUsageTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [InlineData(typeof(DB_Safety), nameof(DB_Safety))]
        public void TestVariables(Type type, string mapping)
        {

            _papper.AddMapping(type);
            var vars = _papper.GetVariablesOf(mapping);
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


    }
}
