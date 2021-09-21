using BenchmarkDotNet.Attributes;
using Benchmarks.Mappings;
using Papper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Benchmarks
{
    [MemoryDiagnoser]
    [RankColumn]
    public class ParalleVsSeqBenchmarks
    {
        private List<DataPack> _reads;
        private List<DataValueTMP> _results;

        [Params(1, 2, 4, 5, 6, 12)]
        public int NumberOfReds { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            _reads = new List<DataPack>();
            _results = new List<DataValueTMP>();
            for (int i = 0; i < 30; i++)
            {
                _reads.Add(new DataPackAbsolute
                {

                });
                _results.Add(new DataValueTMP());
            }
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
        }


        [Benchmark]
        public void CreateResultsParallel()
        {

            _reads.Take(NumberOfReds).AsParallel().Select((item, index) =>
            {
                if (index < NumberOfReds)
                {
                    var result = _results.ElementAt(index);
                    item.ApplyResult(result.ReturnCode == ItemResponseRetValue.Success ? ExecutionResult.Ok : ExecutionResult.Error, result.Data);
                }
                else
                {
                    item.ApplyResult(ExecutionResult.Error);
                }
                return true;
            }).ToList();

        }

        [Benchmark]
        public void CreateResultsSync()
        {

            _reads.Take(NumberOfReds).Select((item, index) =>
            {
                if (index < NumberOfReds)
                {
                    var result = _results.ElementAt(index);
                    item.ApplyResult(result.ReturnCode == ItemResponseRetValue.Success ? ExecutionResult.Ok : ExecutionResult.Error, result.Data);
                }
                else
                {
                    item.ApplyResult(ExecutionResult.Error);
                }
                return true;
            }).ToList();

        }

    }


    internal enum ItemResponseRetValue : byte
    {
        Reserved = 0,
        HardwareFault = 1,
        AccessFault = 3,
        OutOfRange = 5,
        NotSupported = 6,
        SizeMismatch = 7,
        DataError = 10,
        Success = 255
    }

    internal class DataValueTMP
    {
        public Memory<byte> Data { get; } = new byte[100];
        public ItemResponseRetValue ReturnCode { get; } = ItemResponseRetValue.Success;
    }
}
