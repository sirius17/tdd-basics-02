using System;
using FluentAssertions;
using Xunit;

namespace ConsoleCalculator.Tests
{
    public class KeyBufferFixture
    {
        /// Spefication
        /// Represennts array of characters pressed.
        /// Supports 
        /// 1. Signage
        /// 2. Numeric state
        /// 3. Appending digits
        /// 4. Clearing

        [Fact]
        public void Key_buffer_has_a_preset_size_test()
        {
            var buffer = new KeyBuffer(size:15);
            buffer.Should().NotBeNull();
        }

        [Fact]
        public void Digits_can_be_appended_to_the_buffer_test()
        {
            var buffer = new KeyBuffer(size: 15);
            buffer.GetValue().Should().Be(0);
            buffer.Append('1');
            buffer.GetValue().Should().Be(1);
            buffer.Append('2');
            buffer.GetValue().Should().Be(12);
            buffer.Should().NotBeNull();
        }

        [Fact]
        public void Buffer_can_be_cleared_test()
        {
            var buffer = new KeyBuffer(size: 15);
            buffer.IsEmpty.Should().BeTrue();
            buffer.Append('1');
            buffer.GetValue().Should().Be(1);
            buffer.Clear();
            buffer.IsEmpty.Should().BeTrue();
            buffer.GetValue().Should().Be(0);
        }

        [Fact]
        public void Buffer_sign_is_positive_by_default_test()
        {
            var buffer = new KeyBuffer(size: 15);
            buffer.IsNegative.Should().BeFalse();
        }

        [Fact]
        public void Buffer_sign_can_be_toggled_test()
        {
            var buffer = new KeyBuffer(size: 15);
            buffer.Append('1');
            buffer.IsNegative.Should().BeFalse();
            buffer.ToggleSign();
            buffer.IsNegative.Should().BeTrue();
            buffer.GetValue().Should().Be(-1);

        }

        [Fact]
        public void Sign_toggle_is_ignored_if_buffer_is_empty_test()
        {
            var buffer = new KeyBuffer(size: 15);
            buffer.IsNegative.Should().BeFalse();
            buffer.ToggleSign();
            buffer.IsNegative.Should().BeFalse();
        }
    }
}
