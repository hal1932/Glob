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
            benchmark.NumberOfMatches = 10000;
            benchmark.GlobPattern = "p?th/a[bcd]b[e-g]a[1-4][!wxyz][!a-c][!1-3].txt";
            benchmark.SetupData();
            benchmark.IsMatch_True();
#endif
        }
    }
}
