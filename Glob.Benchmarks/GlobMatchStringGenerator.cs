using Glob.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glob.Benchmarks
{
    class GlobMatchStringGenerator
    {
        public GlobMatchStringGenerator(IReadOnlyList<Token> tokens)
        {
            _tokens = tokens;
        }

        public string GenerateRandomMatch()
        {
            var rand = new Random();

            var builder = new StringBuilder();

            for (var i = 0; i < _tokens.Count; ++i)
            {
                var token = _tokens[i];

                switch (token.Type)
                {
                    case TokenType.AnyCharacter:
                        builder.Append(RandGen.Character());
                        break;

                    case TokenType.BeginRange:
                        var literal = _tokens[i + 1].Value;
                        if (literal.Contains('-'))
                        {
                            if (literal.StartsWith("!"))
                            {
                                builder.Append(RandGen.CharacterOtherThan(literal[1], literal[3]));
                            }
                            else
                            {
                                builder.Append(RandGen.CharRange(literal[0], literal[2]));
                            }
                        }
                        else
                        {
                            if (literal.StartsWith("!"))
                            {
                                builder.Append(RandGen.CharacterOtherThan(literal.Substring(1)));
                            }
                            else
                            {
                                builder.Append(literal[rand.Next(literal.Length)]);
                            }
                        }
                        i += 2;
                        break;

                    case TokenType.DirectorySeparator:
                        builder.Append('/');
                        break;

                    case TokenType.DirectoryWildcard:
                        builder.Append(RandGen.Character());
                        builder.Append('/');
                        break;

                    case TokenType.Literal:
                        builder.Append(token.Value);
                        break;

                    case TokenType.Wildcard:
                        for (var j = 0; j < 3; ++j)
                        {
                            builder.Append(RandGen.Character());
                        }
                        break;

                    default:
                        throw new ApplicationException();
                }
            }

            return builder.ToString();
        }

        public string GenerateRandomNonMatch()
        {
            var builder = new StringBuilder();
            return builder.ToString();
        }

        private IReadOnlyList<Token> _tokens;
    }
}
