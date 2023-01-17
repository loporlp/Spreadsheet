using System.Text.RegularExpressions;

namespace FormulaEvaluator
{
    /// <summary>
    /// Author: Mason Sansom
    /// Partner: -none-
    /// Date: 15-Jan-2023
    /// Course:    CS 3500, University of Utah, School of Computing
    /// Copyright: CS 3500 and Mason Sansom - This work may not 
    ///            be copied for use in Academic Coursework.
    ///
    /// I, Mason Sansom, certify that I wrote this code from scratch and
    /// did not copy it in part or whole from another source.  All 
    /// references used in the completion of the assignments are cited 
    /// in my README file.
    ///
    /// File Contents
    /// 
    /// This class Evaluates Infix String inputs and generates an 
    /// answer
    /// </summary>
    public class Evaluator
    {

        public delegate int Lookup(string variableName);

        /// <summary>
        ///     This function takes in a String expression 
        ///     and evalutates and returns an answer
        /// </summary>
        /// <param name="expression"> expression to be evalutated</param>
        /// <param name="variableEvaluator"> Variables to be evaluated</param>
        /// <returns> int solution to the expression </returns>
        public static int Evaluate(String expression, Lookup variableEvaluator)
        {
            Stack<int> integers = new Stack<int>();
            Stack<string> operators = new Stack<string>();

            string[] substrings = Regex.Split(expression, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            foreach(string t in substrings)
            {
                Regex.Replace(t, @"\s+", "");
                //Check if t is a number
                bool isNumeric = int.TryParse(t, out int n);
                if (isNumeric || Regex.IsMatch(t, @"[A-Z]+[0-9]+"))
                {
                    // If operators has a * or / do the operation
                    if (operators.TryPeek(out string result) 
                        && (result.Equals("*") || result.Equals("/")))
                    {
                        integers.Push(Operate(integers.Pop(), n, operators.Pop()));

                    }
                    //If not just push N
                    else
                    {
                        integers.Push(n);
                    }
                }


            }

            return 4;
        }

        private static int Operate(int num1, int num2, string operation)
        {
            return 3;
        }
    }
}