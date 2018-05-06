using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glob
{
    class Program
    {
        static void Main(string[] args)
        {
            var tests = new[]
            {
                new
                {
                    pattern = "literal",
                    value = "literal",
                    result = true,
                },
                new
                {
                    pattern = "a/literal",
                    value = "a/literal",
                    result = true,
                },
                new
                {
                    pattern = "path/*atstand",
                    value = "path/fooatstand",
                    result = true,
                },
                new
                {
                    pattern = "path/hats*nd",
                    value = "path/hatsforstand",
                    result = true,
                },
                new
                {
                    pattern = "p?th/**/dir/*a[bcd]b[e-g]a[1-4][!wxyz][!a-c][!1-3].*",
                    value = "path/hoge/dir/abbea1ad0.aaa",
                    result = true,
                }
            };

            foreach (var test in tests)
            {
                var glob = Glob.Parse(test.pattern);
                var result = glob.IsMatch(test.value);
                Debug.WriteLine($"[{(test.result == result ? "OK" : "NG")}] {test.pattern}, {test.pattern} -> {test.result}");
            }
        }
    }
}
