using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glob.Evaluators
{
    interface IEvaluator
    {
        int MinimumMatchLength { get; }
        bool HasFixedMatchLength { get; }
        bool IsMatch(string value, int charIndex, out int nextCharIndex);
    }
}
