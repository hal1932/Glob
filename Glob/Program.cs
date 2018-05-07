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
            var glob = Glob.Parse("p?th/a[e-g].txt");
            var result = glob.IsMatch("path/ae.txt");
            Debug.WriteLine(result);
        }
    }
}
