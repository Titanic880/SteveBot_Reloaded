using System.Collections.Generic;
using System.Linq;
using System;

namespace SB_Content
{
    public static class Calculator
    {
        private static readonly char[] ops = { '^', '*', '/', '-', '+' };
        /// <summary>
        /// Takes a full equation and calculates it
        /// </summary>
        /// <param name="_Input"></param>
        /// <returns></returns>
        public static double Complex_Equation(string input)
        {
            //Catch Empty input
            if (input == "" || input.Length < 2)
                return double.NaN;
            //Break and check for math information
            List<string> tmp = Complex_Organize(input);

            while (tmp.Count > 1)
            {
                //Pemdas order (minus the Brackets)
                foreach (char op in ops)
                {
                    int index = tmp.IndexOf(op.ToString());
                    if (index < 0) continue;
                    //Pass through Value 2 First due to Math operations
                    tmp[index] = Calcsimple(tmp[index - 1], tmp[index + 1], op);
                    //Clean up Left over values
                    tmp.RemoveAt(index + 1);
                    tmp.RemoveAt(index - 1);
                }
            }
            return Convert.ToDouble(tmp[0]);
        }
        private static string Calcsimple(string value2, string value1, char op)
        {
            return op switch
            {
                '+' => (Convert.ToDouble(value1) + Convert.ToDouble(value2)).ToString(),
                '-' => (Convert.ToDouble(value2) - Convert.ToDouble(value1)).ToString(),
                '*' => (Convert.ToDouble(value1) * Convert.ToDouble(value2)).ToString(),
                '/' => (Convert.ToDouble(value2) / Convert.ToDouble(value1)).ToString(),
                '^' => Math.Pow(Convert.ToDouble(value2), Convert.ToDouble(value1)).ToString(),
                _ => "0",
            };
        }
        private static List<string> Complex_Organize(string input)
        {
            List<string> tmp = new();
            string bracket = "";
            bool brack = false;
            string placeholder = "";
            foreach (char a in input)
            {
                if (a == '(')
                {
                    brack = true;
                    //x() auto complete
                    if (placeholder != "")
                    {
                        tmp.Add(placeholder);
                        tmp.Add("*");
                        placeholder = "";
                    }
                    continue;
                }
                if (brack || a == ')')
                {
                    if (a == ')')
                    {
                        brack = false;
                        //Recursively solve the equation
                        tmp.Add(Complex_Equation(bracket).ToString());
                        continue;
                    }
                    else
                    {
                        bracket += a;
                        continue;
                    }
                }
                //Check for operator (allows bigger than 0-9 operations)
                if (ops.Contains(a))
                {
                    if (placeholder != "")
                    {
                        tmp.Add(placeholder);
                        placeholder = "";
                    }
                    tmp.Add(a.ToString());
                    continue;
                }
                placeholder += a;
            }
            //Catch Missing Values (Better option?)
            if (placeholder != "")
                tmp.Add(placeholder);
            return tmp;
        }

        #region Conversions
        /// <summary>
        /// Takes a number and converts it to Hexcidecimal
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static string Dec2hex(int Input)
        {
            string tmp = Input.ToString("X");
            return tmp;
        }
        /// <summary>
        /// Takes a string and returns a decimal type
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static int Hex2dec(string Input)
        {
            int tmp = Convert.ToInt32(Input, 16);
            return tmp;
        }
        /// <summary>
        /// Returns the Factorial of Input
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static string Factorial(int Input)
        {
            if (Input < 0)
                return "Cannot Factorial a Negative!";
            ulong total = 1;
            for (ulong i = 1; i <= Convert.ToUInt64(Input); i++)
                total *= i;
            return total.ToString();
        }
        #endregion Conversions
    }
}