using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace utests
{
    public class Calculator
    {
        public decimal result { get; set; }
        public bool useMemory { get; set; }
        Calculator calcul { get; set; }
        Calculator tempcalcul { get; set; }
        public static List<decimal> lastOperationResult { get; set; } = new List<decimal>();

        public Calculator()
        {
            result = 0;
            useMemory = true;
        }

        public decimal Sum(params decimal[] numbers)
        {
            for(int i=0; i < numbers.Length; i++)
            {
                result += numbers[i];
            }
            return result;
        }

        public decimal Minus(params decimal[] numbers)
        {
            if (!useMemory)
            {
                result = numbers[0];
                if(numbers.Length > 1)
                {
                    for (int i = 1; i < numbers.Length; i++)
                    {
                        result -= numbers[i];
                    }
                }
            }
            else
            {
                for (int i = 0; i < numbers.Length; i++)
                {
                        result -= numbers[i];
                }
            }
            return result;
        }

        public decimal Multiply(params decimal[] numbers)
        {
            if(result==0)
            {
                result = 1;
            }

            for (int i = 0; i < numbers.Length; i++)
            {
                result *= numbers[i];
            }
            return result;
        }

        public decimal Divide(params decimal[] numbers)
        {
            if (result == 0)
            {
                result = numbers[0];
                return result;
            }
            for (int i = 0; i < numbers.Length; i++)
            {
                try
                {
                    result /= numbers[i];
                }
                catch(DivideByZeroException)
                {
                    Console.WriteLine("Can't divide By {0}", numbers[i]);
                }
            }
            return result;
        }

        public void setMemory(bool use)
        {
            useMemory = use;
        }

        public void initCalcul()
        {
            calcul = new Calculator();
            tempcalcul = new Calculator();
        }

        public Calculator getCalcul()
        {
            return calcul;
        }

        public Calculator getTempcalcul()
        {
            return tempcalcul;
        }

        public void Reset()
        {
            result = 0;
        }

        public static void GenerateCalculator()
        {
            //5+3-(2*4+5)+63-(4/2+1)+9
            Calculator calc = new Calculator();
            string result;
            string tryagain;
            lastOperationResult.Add(0);

            do
            {
                calc.initCalcul();
                Calculator calcul = calc.getCalcul();
                Calculator tempcalcul = calc.getTempcalcul();
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
                int lastIndex = lastOperationResult.Count;
                if(lastIndex > 0)
                {
                    lastIndex = lastIndex - 1;
                }
                Console.WriteLine("Last index: " + Convert.ToString(lastIndex));
                decimal resultM = Convert.ToDecimal(result) + lastOperationResult[lastIndex];

                Console.WriteLine("Actual total:");
                Console.WriteLine(resultM);

                Console.WriteLine("Continue with other operation? C");
                Console.WriteLine("Make a new operation? M");
                if(lastOperationResult.Count > 1)
                {
                    Console.WriteLine("Show operations result history? H");
                }
                Console.WriteLine("Terminate? T");
                input = Console.ReadLine();
                tryagain = input.ToString().ToLower();
                lastOperationResult.Add(resultM);
                if (tryagain == "m")
                {
                    lastOperationResult = new List<decimal>();
                    lastOperationResult.Add(0);
                }
                else if(tryagain == "h")
                {
                    Console.WriteLine("Operations History");
                    for (int ind = 0; ind < lastOperationResult.Count; ind++)
                    {
                        Console.WriteLine("Operation result " + ind + " is " + lastOperationResult[ind]);
                    }
                }
                else if(tryagain != "t")
                {
                    Console.WriteLine("Continue with other operation? C");
                    Console.WriteLine("Make a new operation? M");
                    if (lastOperationResult.Count > 1)
                    {
                        Console.WriteLine("Show operations result history? H");
                    }
                    Console.WriteLine("Terminate? T");
                }
            }
            while (tryagain != "t");
        }

        protected static string doOperations(Calculator calcul, Calculator tempcalcul, string expressionString)
        {
            Operate(ref calcul, ref tempcalcul, ref expressionString, "*");
            Operate(ref calcul, ref tempcalcul, ref expressionString, "/");
            Operate(ref calcul, ref tempcalcul, ref expressionString, "-");
            Operate(ref calcul, ref tempcalcul, ref expressionString, "+");
            Operate(ref calcul, ref tempcalcul, ref expressionString, "+");

            return expressionString;
        }

        protected static void Operate(ref Calculator calcul, ref Calculator tempcalcul, ref string expressionString, string sign)
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
