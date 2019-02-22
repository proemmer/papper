using BenchmarkDotNet.Running;
using System;

namespace Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hint:   Disable assembly signing!!");
            var summary = BenchmarkRunner.Run<Benchmarks>();
        }
    }
}
