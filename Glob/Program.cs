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
            var glob = Glob.Parse("**/gfx/**/*.gfx");
            var result = glob.IsMatch("a_b/gfx/bar/foo.gfx");
            Debug.WriteLine(result);
        }
    }
}
