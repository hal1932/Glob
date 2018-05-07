using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glob.Benchmarks
{
    public static class RandGen
    {
        public static char CharRange(char start, char end) => (char)_rand.Next(start, end);

        public static char UpperAscii() => CharRange('A', 'Z');
        public static char LowerAscii() => CharRange('a', 'z');
        public static char Number() => CharRange('0', '9');
        public static char Symbol() => CharRange('-', '.');

        public static char Character()
        {
            switch (_rand.Next(0, 3))
            {
                case 0: return UpperAscii();
                case 1: return LowerAscii();
                case 2: return Number();
                case 3: return Symbol();
                default:
                    throw new ApplicationException();
            }
        }

        public static char CharacterOrDirectorySeparator()
        {
            switch (_rand.Next(0, 4))
            {
                case 0: return UpperAscii();
                case 1: return LowerAscii();
                case 2: return Number();
                case 3: return Symbol();
                case 4: return '/';
                default:
                    throw new ApplicationException();
            }
        }

        public static char CharacterOtherThan(char start, char end)
        {
            while (true)
            {
                var c = Character();
                if (c < start || end < c)
                {
                    return c;
                }
            }
        }

        public static char CharacterOtherThan(string chars)
        {
            while (true)
            {
                var c = Character();
                if (!chars.Contains(c))
                {
                    return c;
                }
            }
        }

        private static Random _rand = new Random();
    }
}
