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

        [Fact]
        public void Multiple_digit_number_entry_test()
        {
            var calc = new Calculator();
            calc.SendKeyPress('1').Should().Be("1");
            calc.SendKeyPress('2').Should().Be("12");
            calc.SendKeyPress('7').Should().Be("127");
        }

        [Fact]
        public void Ignore_unsupported_keys_test()
        {
            var calc = new Calculator();
            calc.SendKeyPress('a').Should().BeEmpty();
            calc.SendKeyPress('1').Should().Be("1");
            calc.SendKeyPress('a').Should().Be("1");        // Ideally this next key press should be ignored.
            calc.SendKeyPress('4').Should().Be("14");       // Valid digit, so this should work
        }

        [Fact]
        public void Simple_addition_test()
        {
            var calc = new Calculator();
            calc.SendKeyPress('1');
            calc.SendKeyPress('+');
            calc.SendKeyPress('2');
            calc.SendKeyPress('=').Should().Be("3");
        }

        [Fact]
        public void Chained_addition_test()
        {
            var calc = new Calculator();
            calc.SendKeyPress('1');
            calc.SendKeyPress('+');
            calc.SendKeyPress('2');
            calc.SendKeyPress('+').Should().Be("3");
            calc.SendKeyPress('3');
            calc.SendKeyPress('=').Should().Be("6");
        }

        [Fact]
        public void Simple_subtraction_test()
        {
            var calc = new Calculator();
            calc.SendKeyPress('2');
            calc.SendKeyPress('-');
            calc.SendKeyPress('2');
            calc.SendKeyPress('=').Should().Be("0");
        }

        [Fact]
        public void Chained_subtraction_test()
        {
            var calc = new Calculator();
            calc.SendKeyPress('3');
            calc.SendKeyPress('-');
            calc.SendKeyPress('1');
            calc.SendKeyPress('-').Should().Be("2");
            calc.SendKeyPress('2');
            calc.SendKeyPress('=').Should().Be("0");
        }

        [Fact]
        public void Default_first_operand_value_is_0_test()
        {
            var calc = new Calculator();
            calc.SendKeyPress('=').Should().Be("0");
        }
    }
}
