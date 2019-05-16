using System;
using utests;
using Xunit;
using System.Text.RegularExpressions;

namespace XUnitTestProject1
{
    public class TestOperations : Calculator
    {
        [Fact]
        public void Test_SetMemory()
        {
            Calculator calcul = new Calculator();
            calcul.setMemory(true);
            Assert.True(calcul.useMemory);

            calcul.setMemory(false);
            Assert.False(calcul.useMemory);
        }

        [Fact]
        public void Test_getCalcul()
        {
            Calculator calcul = new Calculator();
            calcul.initCalcul();
            Calculator get = calcul.getCalcul();

            try
            {
                Assert.IsType<Calculator>(get);
            }
            catch(NullReferenceException)
            {
                Assert.Throws<NullReferenceException>(() =>
                {
                    Assert.IsType<Calculator>(get);
                });
            }
        }

        [Fact]
        public void Test_getTempCalcul()
        {
            Calculator calcul = new Calculator();
            calcul.initCalcul();
            Calculator get = calcul.getTempcalcul();

            Assert.IsType<Calculator>(get);
        }

        [Fact]
        public void Test_Reset()
        {
            Calculator calcul = new Calculator();
            calcul.Reset();

            Assert.Equal(0, calcul.result);
        }

        [Fact]
        public void Test_GenerateCalculator()
        {
            Calculator calc = new Calculator();
            calc.initCalcul();
            Calculator calcul = calc.getCalcul();
            Calculator tempcalcul = calc.getTempcalcul();
            string result;

            string str = "5+3-(2*4+5)+63-(4/2+1)+9";
            foreach (Match expression in Regex.Matches(str, @"\((\-?\d+.?)+\)", RegexOptions.IgnoreCase))
            {
                result = doOperations(calcul, tempcalcul, expression.ToString());
                str = str.Replace(expression.ToString(), result);
            }
            str = str.Replace("(", "");
            str = str.Replace(")", "");

            result = doOperations(calcul, tempcalcul, str);
            
            Assert.Equal("64",result);
        }

        [Fact]
        public void Test_DoOperations()
        {
            Calculator calcul = new Calculator();
            Calculator tempcalcul = new Calculator();

            string expression = "(2*4+5)";
            Operate(ref calcul, ref tempcalcul, ref expression,"*");
            Assert.Equal("(8+5)", expression);

            expression = "(2*4+5)";
            Operate(ref calcul, ref tempcalcul, ref expression, "+");
            Assert.Equal("(2*9)", expression);

            expression = "(2*4--5)";
            Operate(ref calcul, ref tempcalcul, ref expression, "-");
            Assert.Equal("(2*9)", expression);

            expression = "(2*4-5)";
            Operate(ref calcul, ref tempcalcul, ref expression, "-");
            Assert.Equal("(2*-1)", expression);

            expression = "(2*-5)";
            Operate(ref calcul, ref tempcalcul, ref expression, "*");
            Assert.Equal("(-10)", expression);

            expression = "(4/2+3)";
            Operate(ref calcul, ref tempcalcul, ref expression, "/");
            Assert.Equal("(2+3)", expression);
            Operate(ref calcul, ref tempcalcul, ref expression, "+");
            Assert.Equal("(5)", expression);
        }

        [Fact]
        public void Test_Operate()
        {

        }
    }
}
