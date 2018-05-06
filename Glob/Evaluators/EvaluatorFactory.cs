using Glob.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glob.Evaluators
{
    class EvaluatorFactory
    {
        public IEvaluator CreateEvaluator(IReadOnlyList<Token> tokens, int tokenIndex, out int nextTokenIndex)
        {
            IEvaluator evaluator = null;

            switch (tokens[tokenIndex].Type)
            {
                case TokenType.AnyCharacter:
                    if (!_cache.TryGetValue(TokenType.AnyCharacter, out evaluator))
                    {
                        evaluator = new AnyCharacterEvaluator();
                        _cache[TokenType.AnyCharacter] = evaluator;
                    }
                    nextTokenIndex = tokenIndex + 1;
                    break;

                case TokenType.DirectorySeparator:
                    if (!_cache.TryGetValue(TokenType.DirectorySeparator, out evaluator))
                    {
                        evaluator = new DirectorySeparatorEvaluator();
                        _cache[TokenType.DirectorySeparator] = evaluator;
                    }
                    nextTokenIndex = tokenIndex + 1;
                    break;

                case TokenType.Wildcard:
                    evaluator = new WildcardEvaluator(tokens, tokenIndex, this, out nextTokenIndex);
                    break;

                case TokenType.DirectoryWildcard:
                    evaluator = new DirectoryWildcardEvaluator(tokens, tokenIndex, this, out nextTokenIndex);
                    break;

                case TokenType.Literal:
                    evaluator = new LiteralEvaluator(tokens[tokenIndex]);
                    nextTokenIndex = tokenIndex + 1;
                    break;

                case TokenType.BeginRange:
                    nextTokenIndex = tokenIndex + 1;
                    var literal = tokens[nextTokenIndex];
                    if (literal.Type != TokenType.Literal || tokens[nextTokenIndex + 1].Type != TokenType.EndRange)
                    {
                        throw new InvalidOperationException("Invalid pattern");
                    }

                    if (literal.Value.Contains('-'))
                    {
                        evaluator = new CharRangeEvaluator(literal);
                    }
                    else
                    {
                        evaluator = new CharListEvaluator(literal);
                    }
                    nextTokenIndex += 2;
                    break;

                default:
                    throw new ArgumentException($"invalid token type: {tokens[tokenIndex].Type}");
            }

            return evaluator;
        }

        private Dictionary<TokenType, IEvaluator> _cache = new Dictionary<TokenType, IEvaluator>();
    }
}
