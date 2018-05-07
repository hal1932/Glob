using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Columns;
using BenchmarkDotNet.Attributes.Exporters;
using BenchmarkDotNet.Attributes.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glob.Benchmarks
{
    [ClrJob, MemoryDiagnoser, MarkdownExporter, MinColumn, MaxColumn]
    public class GlobBenchmarks
    {

        private Glob _glob;

        private List<string> _testMatchingStringsList;
        //private List<string> _testNonMatchingStringsList;

        [GlobalSetup]
        public void SetupData()
        {
            _testMatchingStringsList = new List<string>(NumberOfMatches);
            //_testNonMatchingStringsList = new List<string>(NumberOfMatches);
            _glob = Glob.Parse(GlobPattern);
            var generator = new GlobMatchStringGenerator(_glob.Tokens);

            for (int i = 0; i < 10000; i++)
            {
                var matchString = generator.GenerateRandomMatch();
                _testMatchingStringsList.Add(matchString);
                //_testNonMatchingStringsList.Add(generator.GenerateRandomNonMatch());
            }
        }

        [Params(1, 10, 100, 200, 500, 1000)]
        public int NumberOfMatches { get; set; }

        [Params("p?th/a[e-g].txt",
                "p?th/a[bcd]b[e-g].txt",
                "p?th/a[bcd]b[e-g]a[1-4][!wxyz][!a-c][!1-3].txt")]
        public string GlobPattern { get; set; }

        [Benchmark]
        public bool IsMatch_True()
        {
            var result = true;
            for (int i = 0; i < NumberOfMatches; i++)
            {
                var testString = _testMatchingStringsList[i];
                result &= _glob.IsMatch(testString);
            }
            return result;
        }

        //[Benchmark]
        //public bool IsMatch_False()
        //{
        //    var result = false;
        //    for (int i = 0; i < NumberOfMatches; i++)
        //    {
        //        var testString = _testNonMatchingStringsList[i];
        //        result &= !_glob.IsMatch(testString);
        //    }
        //    return result;
        //}

    }
}
