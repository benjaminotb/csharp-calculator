using System;
using utests;
using Xunit;

namespace XUnitTestProject1
{
    public class DivideTests
    {
        [Fact]
        public void Test_Divide_one_number()
        {
            var calculator = new Calculator();

            var result = calculator.Divide(5);
            Assert.IsType<decimal>(result);

            try
            {
                result = calculator.Divide(0);
                Assert.IsType<decimal>(result);
            }
            catch
            {
                Assert.Throws<DivideByZeroException>(() => {
                    result = calculator.Divide(0);
                    Assert.IsType<decimal>(result);
                });
            }
        }

        [Fact]
        public void Test_Divide_two_numbers()
        {
            var calculator = new Calculator();

            var result = calculator.Divide(5, 10);
            Assert.IsType<decimal>(result);

            try
            {
                result = calculator.Divide(5, 0, 10);
                Assert.IsType<decimal>(result);
            }
            catch
            {
                Assert.Throws<DivideByZeroException>(() => {
                    result = calculator.Divide(5, 0, 10);
                    Assert.IsType<decimal>(result);
                });
            }
        }

        [Fact]
        public void Test_Divide_multiple_numbers()
        {
            var calculator = new Calculator();

            var result = calculator.Divide(5, 10, -3.5M, -2, 5, -0.5M);
            Assert.IsType<decimal>(result);

            try
            {
                result = calculator.Divide(5, 10, -3.5M, 0, -2, 5, -0.5M);
                Assert.IsType<decimal>(result);
            }
            catch
            {
                Assert.Throws<DivideByZeroException>(() => {
                    result = calculator.Divide(5, 10, -3.5M, 0, -2, 5, -0.5M);
                    Assert.IsType<decimal>(result);
                });
            }
        }
    }
}
