using System;
using System.Collections.Generic;

namespace ConsoleCalculator
{
    public class Calculator
    {
        public readonly List<char> _digits = new List<char>();

        public string SendKeyPress(char key)
        {
            // Add your implementation here.
            _digits.Add(key);
            return new string(_digits.ToArray());
        }
    }
}
