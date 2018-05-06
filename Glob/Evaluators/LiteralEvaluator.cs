using Glob.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glob.Evaluators
{
    class LiteralEvaluator : IEvaluator
    {
        public int MinimumMatchLength => _token.Value.Length;
        public bool HasFixedMatchLength => true;

        public LiteralEvaluator(Token literal)
        {
            _token = literal;
        }

        public bool IsMatch(string value, int charIndex, out int nextCharIndex)
        {
            if (value.Length - charIndex < _token.Value.Length)
            {
                nextCharIndex = value.Length;
                return false;
            }

            nextCharIndex = charIndex;
            for (var i = 0; i < _token.Value.Length; ++i)
            {
                if (value[charIndex + i] != _token.Value[i])
                {
                    return false;
                }
                ++nextCharIndex;
            }

            return true;
        }

        public override string ToString()
        {
            return $"{nameof(LiteralEvaluator)} {_token.Value}";
        }

        private Token _token;
    }
}
