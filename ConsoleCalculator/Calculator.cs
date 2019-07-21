using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleCalculator
{
    public class Calculator
    {
        private string _digits = string.Empty;

        public string SendKeyPress(char key)
        {
            _digits = _digits + key;
            return _digits;
        }
    }
}
