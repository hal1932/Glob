using Glob.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glob.Evaluators
{
    class CharListEvaluator : IEvaluator
    {
        public int MinimumMatchLength => 1;
        public bool HasFixedMatchLength => true;

        public CharListEvaluator(Token literal)
        {
            _token = literal;
            _negate = literal.Value[0] == '!';
            _chars = literal.Value.Select(x => x).ToArray();
        }

        public bool IsMatch(string value, int charIndex, out int nextCharIndex)
        {
            nextCharIndex = charIndex + 1;

            if (value.Length - charIndex < 1)
            {
                return false;
            }

            //var isContained = _chars.Contains(value[charIndex]);
            var c = value[charIndex];
            var isContained = true;
            for (var i = 0; i < _chars.Length; ++i)
            {
                if (_chars[i] == c)
                {
                    isContained = true;
                    break;
                }
            }
            return (_negate) ? !isContained : isContained;
        }

        public override string ToString()
        {
            return $"{nameof(CharRangeEvaluator)} {_token.Value}";
        }

        private Token _token;
        private bool _negate;
        private char[] _chars;
    }
}
