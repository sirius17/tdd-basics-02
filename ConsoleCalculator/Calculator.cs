using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleCalculator
{
    public class Calculator
    {
        private KeyBuffer _inputBuffer = new KeyBuffer(15);
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
            if (_digits.Length != 0)
            {
                if (_digits.StartsWith("-", StringComparison.Ordinal) == false)
                    _digits = "-" + _digits;
                else
                    _digits = _digits.Substring(1);
            }
            _display = _digits;
        }

        private void HandleDigit(char key)
        {
            var isPrefixedZero = string.IsNullOrWhiteSpace(_digits) == true && key == '0';
            if (isPrefixedZero == false)
            {
                _digits = _digits + key;
                _display = _digits;
            }
        }

        private void HandleEquals()
        {
            bool isAccumulatorInnitialized = _accumulator != null;
            if (_op != null)
            {
                var opA = isAccumulatorInnitialized ? _accumulator.Value : 0;
                var opB = int.Parse(_digits);
                _accumulator = _op.Apply(opA, opB);
            }
            _display = _accumulator?.ToString();

        }

        private void HandleOperator(char key)
        {
            bool isAccumulatorInnitialized = _accumulator != null;
            _op = SupportedOperations[key];
            _accumulator = isAccumulatorInnitialized ? _op.Apply(_accumulator.Value, CurrentOperand) : CurrentOperand;
            _display = _accumulator.ToString();
            _digits = string.Empty;
        }

        private int CurrentOperand => string.IsNullOrWhiteSpace(_digits) == true ? 0 : int.Parse(_digits);

        private bool IsDigit(char key) => Digits.Contains(key);

        private bool IsOperator(char key) => SupportedOperations.Keys.Contains(key);

        private string Display => string.IsNullOrWhiteSpace(_display) ? "0" : _display;
    }


    public class KeyBuffer
    {
        private readonly char[] _buffer;
        private readonly Memory<char> _vector;
        private int _pos = -1;

        public KeyBuffer(int size)
        {
            _buffer = new char[size];
            _vector = new Memory<char>(_buffer);
        }

        public void Append(char digit)
        {
            _buffer[++_pos] = digit;
        }

        public int GetValue()
        {
            if (IsEmpty == true)
                return 0;
            var str = new string(_vector.Slice(0, _pos+1).ToArray());
            return IsNegative ? -int.Parse(str) : int.Parse(str);
        }

        public void Clear()
        {
            for (int i = 0; i < _buffer.Length; i++)
            {
                _buffer[i] = '*';
            }
            _pos = -1;
        }

        public void ToggleSign()
        {
            if(IsEmpty == false )
                IsNegative = !IsNegative;
        }

        public bool IsEmpty => _pos == -1;

        public bool IsNegative { get; private set; }
    }

}
