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
        public int MinimumMatchLength { get; }
        public bool HasFixedMatchLength => true;

        public LiteralEvaluator(Token literal)
        {
            _token = literal;
            _tokenValue = literal.Value.ToArray();
            MinimumMatchLength = _tokenValue.Length;
        }

        public bool IsMatch(string value, int charIndex, out int nextCharIndex)
        {
            if (value.Length - charIndex < _tokenValue.Length)
            {
                nextCharIndex = value.Length;
                return false;
            }

            nextCharIndex = charIndex;
            for (int i = 0, count = _tokenValue.Length; i < count; ++i)
            {
                if (value[charIndex + i] != _tokenValue[i])
                {
                    return false;
                }
            }
            nextCharIndex += _tokenValue.Length;

            return true;
        }

        public override string ToString()
        {
            return $"{nameof(LiteralEvaluator)} {_token.Value}";
        }

        private Token _token;
        private char[] _tokenValue;
    }
}
