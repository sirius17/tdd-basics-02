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
        private int? _accumulator = null;

        public string SendKeyPress(char key)
        {
            var isSupported = _supportedKeys.Contains(key);
            if (isSupported == false) 
                return _display;

            if (key == '+')
            {
                _accumulator = (_accumulator ?? 0) + int.Parse(_digits);
                _display = _accumulator.ToString();
                _digits = string.Empty;
            }
            else if (key == '=')
            {
                _accumulator = int.Parse(_digits) + _accumulator;
                _display = _accumulator.ToString();
                _digits = string.Empty;
            }
            else
            {
                _digits = _digits + key;
                _display = _digits;
            }
            return _display;
        }
    }
}
