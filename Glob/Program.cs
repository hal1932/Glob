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
            var glob = Glob.Parse("[list]s");
            var result = glob.IsMatch("Ls");
            Debug.WriteLine(result);
        }
    }
}
