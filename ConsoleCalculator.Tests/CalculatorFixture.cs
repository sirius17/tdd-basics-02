using System;
using FluentAssertions;
using Xunit;

namespace ConsoleCalculator.Tests
{
    public class CalculatorFixture
    {
        [Fact]
        public void DummyTest()
        {
            return;
        }

        [Theory]
        [InlineData('1', "1")]
        [InlineData('2', "2")]
        [InlineData('3', "3")]
        [InlineData('4', "4")]
        [InlineData('5', "5")]
        [InlineData('6', "6")]
        [InlineData('7', "7")]
        [InlineData('8', "8")]
        [InlineData('9', "9")]
        [InlineData('0', "0")]
        public void Single_digit_number_entry_test(char key, string expected)
        {
            Calculator calc = new Calculator();
            calc.SendKeyPress(key).Should().Be(expected);
        }
    }
}
