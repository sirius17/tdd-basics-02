using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleCalculator
{
    public class Calculator
    {
        private string _display = string.Empty;
        private string _digits = string.Empty;
        private static readonly HashSet<char> _supportedKeys = new HashSet<char>
        {
            '0', '1', '2', '3', '4',
            '5', '6', '7', '8', '9',
            '+', '-', 'x', '/', '=',
            '.', 'c', 's', 'C', 'S'
        };
        private static readonly HashSet<char> Digits = new HashSet<char> { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
        private static readonly Dictionary<char, IBinaryOp> SupportedOperations = new Dictionary<char, IBinaryOp>
        {
            {'+', new Add()},
            {'-', new Subtract()},
            {'x', new Multiply() },
            {'/', new Divide() }
        };

        private int? _accumulator = null;
        private IBinaryOp _op = null;

        public string SendKeyPress(char key)
        {
            var isSupported = _supportedKeys.Contains(key);
            if (isSupported == false) 
                return _display;
            bool isAccumulatorInnitialized = _accumulator != null;
            if (IsOperator(key) == true )
            {
                _op = SupportedOperations[key];
                _accumulator = isAccumulatorInnitialized ? _op.Apply(_accumulator.Value, CurrentOperand) : CurrentOperand;
                _display = _accumulator.ToString();
                _digits = string.Empty;
            }
            else if (key == '=')
            {
                if (_op != null)
                {
                    var opA = isAccumulatorInnitialized ? _accumulator.Value : 0; 
                    var opB = int.Parse(_digits);
                    _accumulator = _op.Apply(opA, opB);
                }
                _display = _accumulator?.ToString();
                _digits = string.Empty;
            }
            else if(IsDigit(key) == true)
            {
                _digits = _digits + key;
                _display = _digits;
            }
            return Display;
        }

        private int CurrentOperand => string.IsNullOrWhiteSpace(_digits) == true ? 0 : int.Parse(_digits);

        private bool IsDigit(char key) => Digits.Contains(key);

        private bool IsOperator(char key) => SupportedOperations.Keys.Contains(key);

        private string Display => string.IsNullOrWhiteSpace(_display) ? "0" : _display;
    }

    public interface IBinaryOp
    {
        int Apply(int opA, int opB);
    }

    public class Add  : IBinaryOp
    {
        public int Apply(int opA, int opB) => opA + opB;
    }

    public class Subtract : IBinaryOp
    {
        public int Apply(int opA, int opB) => opA - opB;
    }

    public class Multiply : IBinaryOp
    {
        public int Apply(int opA, int opB) => opA * opB;
    }

    public class Divide : IBinaryOp
    {
        public int Apply(int opA, int opB) => opA / opB;
    }
}
