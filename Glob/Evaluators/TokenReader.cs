using Glob.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glob.Evaluators
{
    class TokenReader : IDisposable
    {
        public IEvaluator CurrentEvaluator { get; private set; }

        public TokenReader(IReadOnlyList<Token> tokens)
        {
            _tokens = tokens;
            _factory = new EvaluatorFactory();
        }

        public void Dispose() { }

        public bool ReadNext()
        {
            while (_currentTokenIndex < _tokens.Count)
            {
                CurrentEvaluator = _factory.CreateEvaluator(_tokens, _currentTokenIndex, out _currentTokenIndex);
                return true;
            }

            CurrentEvaluator = null;
            return false;
        }

        private IReadOnlyList<Token> _tokens;
        private EvaluatorFactory _factory;
        private int _currentTokenIndex;
    }
}
