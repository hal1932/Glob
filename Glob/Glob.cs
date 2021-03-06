﻿using Glob.Evaluators;
using Glob.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glob
{
    public class Glob
    {
        public IReadOnlyList<Token> Tokens { get; }

        public static Glob Parse(string pattern)
        {
            var tokens = new List<Token>();
            using (var reader = new PatternReader(pattern))
            {
                while (reader.ReadNext())
                {
                    tokens.Add(reader.CurrentToken);
                }
            }

            //foreach (var token in tokens)
            //{
            //    Debug.WriteLine($"{token.Type} {token.Value}");
            //}

            var evaluators = new List<IEvaluator>();
            using (var reader = new TokenReader(tokens))
            {
                while (reader.ReadNext())
                {
                    evaluators.Add(reader.CurrentEvaluator);
                }
            }

            //foreach (var evaluator in evaluators)
            //{
            //    Debug.WriteLine(evaluator);
            //}

            return new Glob(pattern, tokens.AsReadOnly(), evaluators.ToArray());
        }

        private Glob(string pattern, IReadOnlyList<Token> tokens, IEvaluator[] evaluators)
        {
            Tokens = tokens;
            _pattern = pattern;
            _evaluators = evaluators;
        }

        public bool IsMatch(string value)
        {
            var charIndex = 0;
            //foreach (var evaluator in _evaluators)
            for (int i = 0, count = _evaluators.Length; i < count; ++i)
            {
                if (!_evaluators[i].IsMatch(value, charIndex, out charIndex))
                {
                    //Debug.WriteLine($" - {evaluator.GetType()}");
                    return false;
                }
                //Debug.WriteLine($" + {evaluator.GetType()}");
            }
            return charIndex == value.Length;
        }

        public override string ToString()
        {
            return _pattern;
        }

        private string _pattern;
        private IEvaluator[] _evaluators;
    }
}
