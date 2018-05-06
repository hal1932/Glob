using Glob.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glob.Evaluators
{
    class CharRangeEvaluator : IEvaluator
    {
        public int MinimumMatchLength => 1;
        public bool HasFixedMatchLength => true;

        public CharRangeEvaluator(Token literal)
        {
            _token = literal;
            _negate = literal.Value[0] == '!';
            if (_negate)
            {
                _start = literal.Value[1];
                _end = literal.Value[3];
            }
            else
            {
                _start = literal.Value[0];
                _end = literal.Value[2];
            }
        }

        public bool IsMatch(string value, int charIndex, out int nextCharIndex)
        {
            nextCharIndex = charIndex + 1;

            if (value.Length - charIndex < 1)
            {
                return false;
            }

            var currentChar = value[charIndex];
            var isContained = (_start <= currentChar && currentChar <= _end);
            return (_negate) ? !isContained : isContained;
        }

        public override string ToString()
        {
            return $"{nameof(CharRangeEvaluator)} {_token.Value}";
        }

        private Token _token;
        private bool _negate;
        private char _start;
        private char _end;
    }
}
