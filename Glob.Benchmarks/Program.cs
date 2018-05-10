using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glob.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
#if true
            BenchmarkRunner.Run<GlobBenchmarks>();
#else
            var benchmark = new GlobBenchmarks();
            benchmark.NumberOfMatches = 1000;
            benchmark.GlobPattern = "**/gfx/**/*.gfx";
            benchmark.SetupData();
            benchmark.IsMatch_True();
#endif
        }
    }
}
