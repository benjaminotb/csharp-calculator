using System;
using utests;
using Xunit;

namespace XUnitTestProject1
{
    public class MultiplyDivideTests
    {
        [Fact]
        public void Test_Multiply_one_number()
        {
            decimal result = 0;
            var c = new Calculator();

            result = c.Multiply(5);

            Assert.Equal(5, result);
        }

        [Fact]
        public void Test_Multiply_two_numbers()
        {
            decimal result = 0;
            var c = new Calculator();

            result = c.Multiply(5, 10);

            Assert.Equal(50, result);
        }

        [Fact]
        public void Test_Multiply_multiple_numbers()
        {
            decimal result = 0;
            var c = new Calculator();

            result = c.Multiply(5, 10, -3.5M, -2, 5, -0.5M);

            Assert.Equal(-875, result);
            Assert.IsType<decimal>(result);
        }
    }
}
