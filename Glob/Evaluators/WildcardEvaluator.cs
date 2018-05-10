using Glob.Extensions;
using Glob.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glob.Evaluators
{
    class WildcardEvaluator : IEvaluator
    {
        public int MinimumMatchLength => 0;
        public bool HasFixedMatchLength => false;

        public WildcardEvaluator(IReadOnlyList<Token> tokens, int tokenIndex, EvaluatorFactory factory, out int nextTokenIndex)
        {
            _token = tokens[tokenIndex];
            _followingEvaluator = new CompositeEvaluator(tokens, tokenIndex + 1, factory, out nextTokenIndex);
        }

        public bool IsMatch(string value, int charIndex, out int nextCharIndex)
        {
            // この評価器が最後なら、value の末尾まで '/' 以外のすべてにマッチ
            if (_followingEvaluator.SubEvaluatorsCount == 0)
            {
                for (var i = charIndex; i < value.Length; ++i)
                {
                    if (value[i].IsDirectorySeparator())
                    {
                        nextCharIndex = charIndex + i;
                        return false;
                    }
                }

                nextCharIndex = value.Length;
                return true;
            }

            // 以降の評価器の最低文字数に足りなければマッチしない
            if (value.Length - charIndex < _followingEvaluator.MinimumMatchLength)
            {
                nextCharIndex = value.Length;
                return false;
            }

            // 以降の評価器が固定長マッチ
            if (_followingEvaluator.HasFixedMatchLength)
            {
                var endIndex = value.Length - _followingEvaluator.MinimumMatchLength;
                for (var i = charIndex; i < endIndex; ++i)
                {
                    if (value[i].IsDirectorySeparator())
                    {
                        nextCharIndex = i;
                        return false;
                    }   
                }
                charIndex = endIndex;

                return _followingEvaluator.IsMatch(value, charIndex, out nextCharIndex);
            }

            // 以降の評価器が可変長マッチ
            for (var i = charIndex; i < value.Length; ++i)
            {
                if (_followingEvaluator.IsMatch(value, i, out nextCharIndex))
                {
                    return true;
                }

                if (value[i].IsDirectorySeparator())
                {
                    nextCharIndex = i;
                    return false;
                }
            }

            nextCharIndex = value.Length;
            return false;
        }

        public override string ToString()
        {
            return nameof(WildcardEvaluator);
        }

        private Token _token;
        private CompositeEvaluator _followingEvaluator;
    }
}
