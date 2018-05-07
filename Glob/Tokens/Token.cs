using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glob.Tokens
{
    public struct Token
    {
        public static Token Default = new Token() { Type = TokenType.None };

        public TokenType Type;
        public string Value;
    }
}
