using BenchmarkDotNet.Attributes;
using Papper;

namespace Benchmarks
{
    [MemoryDiagnoser]
    [ClrJob(baseline:true), CoreJob]
    [RankColumn]
    public class Benchmarks
    {

        [Benchmark]
        public PlcReadReference ParseReadReference() => PlcReadReference.FromAddress($"DB_Safety2.SafeMotion.Header.States.ChecksumInvalid");

        [Benchmark]
        public PlcWriteReference ParseWriteReference() => PlcWriteReference.FromAddress($"DB_Safety2.SafeMotion.Header.States.ChecksumInvalid", true);


    }
}
