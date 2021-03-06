﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glob.Tokens
{
    class PatternReader : IDisposable
    {
        public Token CurrentToken { get; private set; }

        public PatternReader(string pattern)
        {
            _pattern = pattern;
            CurrentToken = Token.Default;
        }

        public bool ReadNext()
        {
            while (_currentCharIndex < _pattern.Length)
            {
                var charType = PeekChar(_currentCharIndex);
                var value = default(string);

                switch (charType)
                {
                    case TokenType.DirectoryWildcard:
                        _currentCharIndex += 2;
                        if (PeekChar(_currentCharIndex) == TokenType.DirectorySeparator)
                        {
                            ++_currentCharIndex;
                            value = "**/";
                        }
                        else
                        {
                            value = "**";
                        }
                        break;

                    case TokenType.Literal:
                        var nextPosition = _currentCharIndex + 1;
                        while (nextPosition < _pattern.Length && PeekChar(nextPosition) == TokenType.Literal)
                        {
                            ++nextPosition;
                        }
                        value = _pattern.Substring(_currentCharIndex, nextPosition - _currentCharIndex);
                        _currentCharIndex = nextPosition;
                        break;

                    default:
                        value = _pattern[_currentCharIndex].ToString();
                        ++_currentCharIndex;
                        break;
                }

                CurrentToken = new Token() { Type = charType, Value = value };
                return true;
            }

            CurrentToken = Token.Default;
            return false;
        }

        private TokenType PeekChar(int position)
        {
            if (position >= _pattern.Length)
            {
                return TokenType.None;
            }

            switch (_pattern[position])
            {
                case '[':
                    return TokenType.BeginRange;

                case ']':
                    return TokenType.EndRange;

                case '/':
                    return TokenType.DirectorySeparator;

                case '*':
                    if (position + 1 < _pattern.Length && _pattern[position + 1] == '*')
                    {
                        return TokenType.DirectoryWildcard;
                    }
                    return TokenType.Wildcard;

                case '?':
                    return TokenType.AnyCharacter;

                default:
                    return TokenType.Literal;
            }
        }

        public void Dispose() { }

        private string _pattern;
        private int _currentCharIndex;
    }
}
