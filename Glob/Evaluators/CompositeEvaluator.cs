using Glob.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glob.Evaluators
{
    class CompositeEvaluator : IEvaluator
    {
        public int SubEvaluatorsCount { get; }
        public int MinimumMatchLength { get; private set; }
        public bool HasFixedMatchLength { get; private set; }

        public CompositeEvaluator(IReadOnlyList<Token> tokens, int startTokenIndex, EvaluatorFactory factory, out int nextTokenIndex)
        {
            MinimumMatchLength = 0;
            HasFixedMatchLength = true;

            var evaluators = new List<IEvaluator>();

            nextTokenIndex = startTokenIndex;
            while (nextTokenIndex < tokens.Count)
            {
                var evaluator = factory.CreateEvaluator(tokens, nextTokenIndex, out nextTokenIndex);
                evaluators.Add(evaluator);

                MinimumMatchLength += evaluator.MinimumMatchLength;
                HasFixedMatchLength &= evaluator.HasFixedMatchLength;
            }
            _evaluators = evaluators.AsReadOnly();
            SubEvaluatorsCount = _evaluators.Count;

            nextTokenIndex = tokens.Count;
        }

        public bool IsMatch(string value, int charIndex, out int nextCharIndex)
        {
            nextCharIndex = charIndex;

            foreach (var evaluator in _evaluators)
            {
                if (!evaluator.IsMatch(value, charIndex, out nextCharIndex))
                {
                    //Debug.WriteLine($" - {evaluator.GetType()}");
                    return false;
                }
                //Debug.WriteLine($" + {evaluator.GetType()}");
                charIndex = nextCharIndex;
            }

            return true;
        }

        private IReadOnlyList<IEvaluator> _evaluators;
    }
}
