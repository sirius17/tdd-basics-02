using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleCalculator
{
    public class Calculator
    {
        private string _digits = string.Empty;
        private static readonly HashSet<char> _supportedKeys = new HashSet<char>
        {
            '0', '1', '2', '3', '4',
            '5', '6', '7', '8', '9',
            '+', '-', 'x', '/', '=',
            '.', 'c', 's', 'C', 'S'
        };
        private int _accumulator = 0;


        public string SendKeyPress(char key)
        {

            var isSupported = _supportedKeys.Contains(key);
            if (isSupported == false) 
                return _digits;

            if (key == '+')
            {
                _accumulator = int.Parse(_digits);
                _digits = string.Empty;
            }
            else if (key == '=')
            {
                _accumulator = int.Parse(_digits) + _accumulator;
                _digits = _accumulator.ToString();
            }
            else
            {
                _digits = _digits + key;
            }
            return _digits;
        }
    }
}
