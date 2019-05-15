using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace utests
{
    public class Calculator
    {
        decimal result { get; set; }
        bool useMemory { get; set; }
        Calculator calcul { get; set; }
        Calculator tempcalcul { get; set; }

        public Calculator()
        {
            this.result = 0;
            this.useMemory = true;
        }

        public decimal Sum(params decimal[] numbers)
        {
            for(int i=0; i < numbers.Length; i++)
            {
                this.result += numbers[i];
            }
            return this.result;
        }

        public decimal Minus(params decimal[] numbers)
        {
            if (!this.useMemory)
            {
                this.result = numbers[0];
                if(numbers.Length > 1)
                {
                    for (int i = 1; i < numbers.Length; i++)
                    {
                        this.result -= numbers[i];
                    }
                }
            }
            else
            {
                for (int i = 0; i < numbers.Length; i++)
                {
                        this.result -= numbers[i];
                }
            }
            return this.result;
        }

        public decimal Multiply(params decimal[] numbers)
        {
            if(this.result==0)
            {
                this.result = 1;
            }

            for (int i = 0; i < numbers.Length; i++)
            {
                this.result *= numbers[i];
            }
            return this.result;
        }

        public decimal Divide(params decimal[] numbers)
        {
            if (this.result == 0)
            {
                this.result = numbers[0];
                return this.result;
            }
            for (int i = 0; i < numbers.Length; i++)
            {
                this.result /= numbers[i];
            }
            return this.result;
        }

        public void setMemory(bool use)
        {
            this.useMemory = use;
        }

        public void initCalcul()
        {
            this.calcul = new Calculator();
            this.tempcalcul = new Calculator();
        }

        public Calculator getCalcul()
        {
            return this.calcul;
        }

        public Calculator getTempcalcul()
        {
            return this.tempcalcul;
        }

        public void Reset()
        {
            this.result = 0;
        }

        public static Calculator GenerateCalculator()
        {
            //5+3-(2*4+5)+63-(4/2+1)+9
            Calculator calc = new Calculator();
            calc.initCalcul();
            Calculator calcul = calc.getCalcul();
            Calculator tempcalcul = calc.getTempcalcul();
            string result;
            string tryagain;

            do
            {
                Console.WriteLine("Begin Calculation");
                var input = Console.ReadLine();
                string str = input.ToString();
                foreach (Match expression in Regex.Matches(str, @"\((\-?\d+.?)+\)", RegexOptions.IgnoreCase))
                {
                    result = doOperations(calcul, tempcalcul, expression.ToString());
                    str = str.Replace(expression.ToString(), result);
                }
                str = str.Replace("(", "");
                str = str.Replace(")", "");

                result = doOperations(calcul, tempcalcul, str);

                Console.WriteLine("Actual total:");
                Console.WriteLine(result);

                Console.WriteLine("Try again? Y/N");
                input = Console.ReadLine();
                tryagain = input.ToString().ToLower();
            }
            while (tryagain == "y");
            return calcul;
        }

        private static string doOperations(Calculator calcul, Calculator tempcalcul, string expressionString)
        {
            Operate(ref calcul, ref tempcalcul, ref expressionString, "*");
            Operate(ref calcul, ref tempcalcul, ref expressionString, "/");
            Operate(ref calcul, ref tempcalcul, ref expressionString, "-");
            Operate(ref calcul, ref tempcalcul, ref expressionString, "+");
            Operate(ref calcul, ref tempcalcul, ref expressionString, "+");

            return expressionString;
        }

        private static void Operate(ref Calculator calcul, ref Calculator tempcalcul, ref string expressionString, string sign)
        {
            
            string Rstring = @"\-?\d+\+\-?\d+";
            switch (sign)
            {
                case "-":
                    Rstring = @"\-?\d+\-\-?\d+";
                    break;
                case "*":
                    Rstring = @"\-?\d+\*\-?\d+";
                    break;
                case "/":
                    Rstring = @"\-?\d+\/\-?\d+";
                    break;
            }

            foreach (Match multiply in Regex.Matches(expressionString, Rstring, RegexOptions.IgnoreCase))
            {
                string multiplyString = multiply.ToString();
                int index = multiplyString.IndexOf(sign, 1);
                string str2 = multiplyString.Substring(index);
                string str1 = multiplyString.Replace(str2, "");

                tempcalcul.Reset();
                if (sign == "-") {
                    str2 = str2.IndexOf("-", 1) > 0 ? "-" + str2.Replace("-", "") : str2.Replace("-", "");
                    tempcalcul.setMemory(false);
                }
                else { str2 = str2.Replace(sign, ""); }

                Console.WriteLine("Calcul: " + str1+" "+sign+" "+ str2);
                decimal str1Decimal = Convert.ToDecimal(str1);
                doCalcul(str1Decimal,ref tempcalcul, sign);
                if (sign == "-") { tempcalcul.setMemory(true); }

                decimal str1Decima2 = Convert.ToDecimal(str2);
                doCalcul(str1Decima2, ref tempcalcul, sign);
                
                expressionString = expressionString.Replace(multiply.ToString(), tempcalcul.result.ToString());
                Console.WriteLine("Current Result: " + expressionString);
            }
        }

        public static void doCalcul(decimal dec, ref Calculator tempcalcul, string sign)
        {
            switch (sign)
            {
                case "-":
                    tempcalcul.Minus(dec);
                    break;
                case "*":
                    tempcalcul.Multiply(dec);
                    break;
                case "/":
                    tempcalcul.Divide(dec);
                    break;
                default:
                    tempcalcul.Sum(dec);
                    break;
            }
        }

        static void Main(string[] args)
        {
            GenerateCalculator();
        }
    }
}
