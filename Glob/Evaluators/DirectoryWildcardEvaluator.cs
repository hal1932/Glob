using Glob.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glob.Evaluators
{
    class DirectoryWildcardEvaluator : IEvaluator
    {
        public int MinimumMatchLength => 0;
        public bool HasFixedMatchLength => false;

        public DirectoryWildcardEvaluator(IReadOnlyList<Token> tokens, int tokenIndex, EvaluatorFactory factory, out int nextTokenIndex)
        {
            _token = tokens[tokenIndex];
            _followingEvaluator = new CompositeEvaluator(tokens, tokenIndex + 1, factory, out nextTokenIndex);
            _containsFollowingSeparator = _token.Value.EndsWith("/");
        }

        public bool IsMatch(string value, int charIndex, out int nextCharIndex)
        {
            // この評価器が最後なら、value の末尾まですべてにマッチ
            if (_followingEvaluator.SubEvaluatorsCount == 0)
            {
                nextCharIndex = value.Length;
                if (_containsFollowingSeparator)
                {
                    return value[value.Length - 1] == '/';
                }
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
                charIndex = value.Length - _followingEvaluator.MinimumMatchLength;
                if (charIndex > 0 && value[charIndex - 1] != '/')
                {
                    nextCharIndex = charIndex;
                    return false;
                }
                return _followingEvaluator.IsMatch(value, charIndex, out nextCharIndex);
            }

            // 以降の評価器が可変長マッチ
            while (charIndex < value.Length)
            {
                if (_followingEvaluator.IsMatch(value, charIndex, out nextCharIndex))
                {
                    return true;
                }

                for (var i = charIndex; i < value.Length; ++i)
                {
                    if (value[i] == '/')
                    {
                        break;
                    }
                }
                //charIndex = value.IndexOf('/', charIndex);
                //if (charIndex < 0)
                //{
                //    break;
                //}
                ++charIndex;
            }

            nextCharIndex = charIndex;
            return false;
        }

        public override string ToString()
        {
            return nameof(DirectoryWildcardEvaluator);
        }

        private Token _token;
        private CompositeEvaluator _followingEvaluator;
        private bool _containsFollowingSeparator;
    }
}
