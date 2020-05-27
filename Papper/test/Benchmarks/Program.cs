using BenchmarkDotNet.Running;
using System;

namespace Benchmarks
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hint:   Disable assembly signing!!");
            //var summary = BenchmarkRunner.Run<Benchmarks>();
            var summary = BenchmarkRunner.Run<ParalleVsSeqBenchmarks>();
        }
    }
}
