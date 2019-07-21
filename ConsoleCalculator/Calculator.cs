using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleCalculator
{
    public class Calculator
    {
        private readonly KeyBuffer _input = new KeyBuffer(15);

        private static readonly HashSet<char> _supportedKeys = new HashSet<char>
        {
            '0', '1', '2', '3', '4',
            '5', '6', '7', '8', '9',
            '+', '-', 'x', '/', '=',
            '.', 'c', 's', 'C', 'S'
        };
        private static readonly HashSet<char> Digits = new HashSet<char> { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '.' };
        private static readonly Dictionary<char, IBinaryOp> SupportedOperations = new Dictionary<char, IBinaryOp>
        {
            {'+', new Add()},
            {'-', new Subtract()},
            {'x', new Multiply() },
            {'/', new Divide() }
        };


        private float? _result = null;
        private float? _lastOperand = null;
        private IBinaryOp _op = null;

        public string SendKeyPress(char key)
        {
            try
            {
                var isSupported = _supportedKeys.Contains(key);
                if (isSupported == true)
                {
                    if (IsOperator(key) == true)
                        HandleOperator(key);
                    else if (key == '=')
                        HandleEquals();
                    else if (IsDigit(key) == true)
                        HandleDigit(key);
                    else if (key == 's' || key == 'S')
                        HandleSign();
                    else if (key == 'c' || key == 'C')
                        ResetCalculator();
                }
            }
            catch
            {
                HandleFault();
                return "-E-";
            }

            return GetDisplayValue();
        }

        private void HandleFault()
        {
            ResetCalculator();
        }

        private void ResetCalculator()
        {
            _input.Clear();
            _lastOperand = null;
            _result = null;
            _op = null;
        }

        private void HandleSign()
        {
            _input.ToggleSign();
        }

        private void HandleDigit(char key)
        {
            // Ignore multiple decimal points.
            if (_input.Contains('.') && key == '.')
                return;
            // Ignore multiple preceeding zeros.
#pragma warning disable RECS0018 // Comparison of floating point numbers with equality operator
            if (_input.IsEmpty == false && _input.GetStringValue() == "0" && key != '.')
#pragma warning restore RECS0018 // Comparison of floating point numbers with equality operator
                _input.Clear();
            _input.Append(key);
        }

        private void HandleEquals()
        {
            /// Specification
            /// 1. If = is pressed without an operation, then current input is the last operand
            /// 2. In all other cases = performs the operation. The only thing that matters is what are the values of opA and opB.
            /// opA = result or lastOperand
            /// opB = currentInput or lastOperand (if current input is null)
            bool hasOperation = _op != null;
            if( hasOperation == false )
            {
                _result = _input.GetValue();
                _input.Clear();
                return;
            }

            var resultExists = _result != null;
            var opA = resultExists ? _result.Value : _lastOperand.Value;
            var opB = _input.IsEmpty == false ? _input.GetValue() : _lastOperand.Value;
            _result = _op.Apply(opA, opB);
            _lastOperand = opB;
            _input.Clear();
        }

        private void HandleOperator(char key)
        {
            /// Specification
            /// When an operator is pressed, if two operands are present, then the operation is applied to the result.
            /// Two operators are persent when one of the  the following occur -
            /// 1. Either lastOperand exists alone
            /// 2. Both result and lastOperand exist.
            /// Else the operand and operation are recorded and result is reset.
            /// In all cases we will record the operation selected and the current operand.
            _op = SupportedOperations[key];
            var currentOp = _input.GetValue();
            var twoOperandsPresent = (_result != null && _lastOperand != null) || (_result == null && _lastOperand != null);
            if (twoOperandsPresent == true)
            {
                var opA = _result != null ? _result.Value : _lastOperand.Value;
                _result = _op.Apply(opA, currentOp);
            }
            else
            {
                // We are reseting result since results without 2 operands cannot be calculated.
                _result = null;
            }
            _lastOperand = currentOp;
            _input.Clear();
        }

        private bool IsDigit(char key) => Digits.Contains(key);

        private bool IsOperator(char key) => SupportedOperations.Keys.Contains(key);

        private string GetDisplayValue()
        {
            /// Spec:
            /// If key buffer is not empty then display the buffer.
            /// If it is empty then display the result in memory
            /// Else display the operand
            if(_input.IsEmpty == false)
                return _input.GetStringValue();
            if (_result != null)
                return _result.ToString();
            if (_lastOperand != null)
                return _lastOperand.ToString();
            return "0";
        }
    }

}
