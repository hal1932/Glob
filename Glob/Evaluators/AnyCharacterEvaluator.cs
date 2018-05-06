using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glob.Evaluators
{
    class AnyCharacterEvaluator : IEvaluator
    {
        public int MinimumMatchLength => 1;
        public bool HasFixedMatchLength => true;

        public bool IsMatch(string value, int charIndex, out int nextCharIndex)
        {
            nextCharIndex = charIndex + 1;

            if (value.Length - charIndex < 1)
            {
                return false;
            }

            return value[charIndex] != '/';
        }

        public override string ToString()
        {
            return nameof(AnyCharacterEvaluator);
        }
    }
}
