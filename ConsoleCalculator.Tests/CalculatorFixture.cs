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
            calc.SendKeyPress('a').Should().Be("0");
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

        [Fact]
        public void Chained_operation_with_default_first_operand_test()
        {
            var calc = new Calculator();
            calc.SendKeyPress('-');
            calc.SendKeyPress('2');
            calc.SendKeyPress('-');
            calc.SendKeyPress('3');
            calc.SendKeyPress('=').Should().Be("-5");
        }

        [Fact]
        public void Simple_multiplication_test()
        {
            var calc = new Calculator();
            calc.SendKeyPress('2');
            calc.SendKeyPress('x');
            calc.SendKeyPress('2');
            calc.SendKeyPress('=').Should().Be("4");
        }

        [Fact]
        public void Chained_multiplication_test()
        {
            var calc = new Calculator();
            calc.SendKeyPress('2');
            calc.SendKeyPress('x');
            calc.SendKeyPress('3');
            calc.SendKeyPress('x').Should().Be("6");
            calc.SendKeyPress('4');
            calc.SendKeyPress('=').Should().Be("24");
        }

        [Fact]
        public void Simple_division_test()
        {
            var calc = new Calculator();
            calc.SendKeyPress('8');
            calc.SendKeyPress('/');
            calc.SendKeyPress('2');
            calc.SendKeyPress('=').Should().Be("4");
        }

        [Fact]
        public void Chained_division_test()
        {
            var calc = new Calculator();
            calc.SendKeyPress('8');
            calc.SendKeyPress('/');
            calc.SendKeyPress('4');
            calc.SendKeyPress('/').Should().Be("2");
            calc.SendKeyPress('2');
            calc.SendKeyPress('=').Should().Be("1");
        }

        [Fact]
        public void Ignore_initial_zeros_for_integers_test()
        {
            var calc = new Calculator();
            calc.SendKeyPress('0').Should().Be("0");
            calc.SendKeyPress('0').Should().Be("0");
            calc.SendKeyPress('0').Should().Be("0");
            calc.SendKeyPress('2').Should().Be("2");
            calc.SendKeyPress('0').Should().Be("20");
            calc.SendKeyPress('0').Should().Be("200");
        }


        [Fact]
        public void Zero_should_ignore_sign_test()
        {
            var calc = new Calculator();
            calc.SendKeyPress('s').Should().Be("0");
            calc.SendKeyPress('0').Should().Be("0");
            calc.SendKeyPress('s').Should().Be("0");
        }

        [Fact]
        public void Non_zero_operand_should_honor_sign_test()
        {
            var calc = new Calculator();
            calc.SendKeyPress('1').Should().Be("1");
            calc.SendKeyPress('s').Should().Be("-1");
            calc.SendKeyPress('s').Should().Be("1");
        }


        [Theory]
        [InlineData("12s + 10", "-2")]
        [InlineData("12s - 10", "-22")]
        [InlineData("12s x 10", "-120")]
        [InlineData("120s / 10", "-12")]
        public void Operations_with_negative_numbers_test(string seq, string expected )
        {
            var calc = new Calculator();
            calc.SendKeySequence(seq);
            calc.SendKeyPress('=').Should().Be(expected);
        }

        [Fact]
        public void Consequtive_equals_should_duplicate_last_operation_test()
        {
            var calc = new Calculator();
            calc.SendKeySequence("1+2===").Should().Be("7");
        }

        [Fact]
        public void Operation_with_single_operand_should_copy_to_second_operand_test()
        {
            var calc = new Calculator();
            calc.SendKeySequence("1+=").Should().Be("2");
        }

        [Fact]
        public void Equal_without_operation_should_treat_operand_as_result_test()
        {
            var calc = new Calculator();
            calc.SendKeySequence("1=").Should().Be("1");
            calc.SendKeySequence("2+=").Should().Be("4");
        }


        [Fact]
        public void Clear_should_reset_calculator_test()
        {
            var calc = new Calculator();
            calc.SendKeySequence("1+2=").Should().Be("3");
            calc.SendKeyPress('c').Should().Be("0");
            calc.SendKeySequence("1+2=").Should().Be("3");
            calc.SendKeyPress('C').Should().Be("0");
        }

        [Fact]
        public void Errors_should_display_as_error_test()
        {
            var calc = new Calculator();
            calc.SendKeySequence("3/0=").Should().Be("-E-");
        }


        [Fact]
        public void Simple_decimal_number_display_test()
        {
            var calc = new Calculator();
            calc.SendKeySequence("3.142").Should().Be("3.142");
        }

        [Fact]
        public void Ignore_multiple_decimal_point_input_test()
        {
            var calc = new Calculator();
            calc.SendKeySequence("3.1..4...2..").Should().Be("3.142");
        }
    }

    internal static class CalculatorExtensions
    {
        public static string SendKeySequence(this Calculator calc, string keys)
        {
            var result = string.Empty;
            foreach (var key in keys)
                result = calc.SendKeyPress(key);
            return result;
        }
    }
}
