using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleCalculator
{
    public class Calculator
    {
        private readonly KeyBuffer _input = new KeyBuffer(15);
        private string _display = string.Empty;

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

        private int? _operand = null;
        private IBinaryOp _op = null;

        public string SendKeyPress(char key)
        {
            var isSupported = _supportedKeys.Contains(key);
            if (isSupported == false) 
                return _display;

            if (IsOperator(key) == true)
                HandleOperator(key);
            else if (key == '=')
                HandleEquals();
            else if (IsDigit(key) == true)
                HandleDigit(key);
            else if (key == 's')
                HandleSign();
            return Display;
        }

        private void HandleSign()
        {
            _input.ToggleSign();
            _display = _input.GetStringValue();
        }

        private void HandleDigit(char key)
        {
            var isPrefixedZero = _input.IsEmpty == true && key == '0';
            if (isPrefixedZero == false)
            {
                _input.Append(key);
                _display = _input.GetStringValue();
            }
        }

        private void HandleEquals()
        {
            bool isAccumulatorInnitialized = _operand != null;
            if (_op != null)
            {
                var opA = isAccumulatorInnitialized ? _operand.Value : 0;
                var opB = _input.GetValue();
                _operand = _op.Apply(opA, opB);
            }
            _display = _operand?.ToString();

        }

        private void HandleOperator(char key)
        {
            bool isAccumulatorInnitialized = _operand != null;
            _op = SupportedOperations[key];
            _operand = isAccumulatorInnitialized ? _op.Apply(_operand.Value, _input.GetValue()) : _input.GetValue();
            _display = _operand.ToString();
            _input.Clear();
        }

        private bool IsDigit(char key) => Digits.Contains(key);

        private bool IsOperator(char key) => SupportedOperations.Keys.Contains(key);

        private string Display => string.IsNullOrWhiteSpace(_display) ? "0" : _display;


    }

}
