using BenchmarkDotNet.Attributes;
using Benchmarks.Mappings;
using Papper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Benchmarks
{
    [MemoryDiagnoser]
    [ClrJob(baseline: true), CoreJob]
    [RankColumn]
    public class Benchmarks
    {
        private PlcDataMapper _papper;

        [GlobalSetup]
        public void GlobalSetup()
        {
            _papper = new PlcDataMapper(960, Papper_OnRead, Papper_OnWrite);
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            _papper?.Dispose();
        }


        //[Benchmark]
        public PlcReadReference ParseReadReference() => PlcReadReference.FromAddress($"DB_Safety2.SafeMotion.Header.States.ChecksumInvalid");

        //[Benchmark]
        public PlcWriteReference ParseWriteReference() => PlcWriteReference.FromAddress($"DB_Safety2.SafeMotion.Header.States.ChecksumInvalid", true);

        //[Benchmark]
        public void AddMapping()
        {
            var papper = new PlcDataMapper(960, Papper_OnRead, Papper_OnWrite);
            papper.AddMapping(typeof(DB_MotionHMI));
        }

        [Benchmark]
        public async Task WriteAsync()
        {
            await _papper.WriteAsync(PlcWriteReference.FromAddress($"DB_Safety2.SafeMotion.Header.States.ChecksumInvalid", true));
        }

        [Benchmark]
        public async Task ReadAsync()
        {
            await _papper.ReadAsync(PlcReadReference.FromAddress($"DB_Safety2.SafeMotion.Header.States.ChecksumInvalid"));
        }


        private static Task Papper_OnRead(IEnumerable<DataPack> reads) => Task.CompletedTask;


        private static Task Papper_OnWrite(IEnumerable<DataPack> reads) => Task.CompletedTask;
    }
}
