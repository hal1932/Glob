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
            var valueLength = value.Length;

            // この評価器が最後なら、value の末尾まで '/' 以外のすべてにマッチ
            if (_followingEvaluator.SubEvaluatorsCount == 0)
            {
                for (int i = charIndex; i < valueLength; ++i)
                {
                    if (value[i].IsDirectorySeparator())
                    {
                        nextCharIndex = charIndex + i;
                        return false;
                    }
                }

                nextCharIndex = valueLength;
                return true;
            }

            // 以降の評価器の最低文字数に足りなければマッチしない
            if (valueLength - charIndex < _followingEvaluator.MinimumMatchLength)
            {
                nextCharIndex = valueLength;
                return false;
            }

            // 以降の評価器が固定長マッチ
            if (_followingEvaluator.HasFixedMatchLength)
            {
                var endIndex = valueLength - _followingEvaluator.MinimumMatchLength;
                for (int i = charIndex, count = endIndex; i < count; ++i)
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
            for (int i = charIndex; i < valueLength; ++i)
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

            nextCharIndex = valueLength;
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
