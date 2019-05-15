using System;
using utests;
using Xunit;

namespace XUnitTestProject1
{
    public class UnitTest1
    {
        [Fact]
        public void Test_Sum_one_number()
        {
            decimal result = 0;
            var c = new Calculator();

            result = c.Sum(5);

            Assert.Equal(5, result);
        }

        [Fact]
        public void Test_Sum_two_numbers()
        {
            decimal result = 0;
            var c = new Calculator();

            result = c.Sum(5, 10);

            Assert.Equal(15, result);
        }

        [Fact]
        public void Test_Sum_multiple_numbers()
        {
            decimal result = 0;
            var c = new Calculator();

            result = c.Sum(5, 10, -3.5M, -2, 5, -0.5M);

            Assert.Equal(14, result);
            Assert.IsType<decimal>(result);
        }
    }
}
