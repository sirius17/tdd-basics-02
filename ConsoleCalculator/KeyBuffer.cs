using System;
using System.Linq;

namespace ConsoleCalculator
{
    public class KeyBuffer
    {
        private readonly char[] _buffer;
        private readonly Memory<char> _vector;
        private int _pos = -1;

        public KeyBuffer(int size)
        {
            _buffer = new char[size];
            IsNegative = false;
            _vector = new Memory<char>(_buffer);
        }

        public void Append(char digit)
        {
            _buffer[++_pos] = digit;
        }

        public float GetValue()
        {
            if (IsEmpty == true)
                return 0f;
            else 
                return float.Parse(GetStringValue());
        }

        public void Clear()
        {
            for (int i = 0; i < _buffer.Length; i++)
            {
                _buffer[i] = '*';
            }
            _pos = -1;
            IsNegative = false;
        }

        public void ToggleSign()
        {
            if (IsEmpty == false && GetValue() != 0)
                IsNegative = !IsNegative;
        }

        public string GetStringValue()
        {
            if (IsEmpty == true)
                return "0";
            var str = new string(_vector.Slice(0, _pos + 1).ToArray());
            return IsNegative ? "-" + str : str;
        }

        public bool IsEmpty => _pos == -1;

        public bool IsNegative { get; private set; }

        public bool Contains(char c)
        {
            return _buffer.Contains(c);
        }
    }

}
