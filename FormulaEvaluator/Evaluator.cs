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

            foreach (string t in substrings)
            {
                //Remove all whitespace
                string token = Regex.Replace(t, @"\s+", "");

                //If token is a variable convert it to a number
                if (Regex.IsMatch(t, @"[A-Z]+[0-9]+"))
                {
                    try
                    {
                        token = variableEvaluator(token).ToString();
                    } catch(Exception ex)
                    {
                        throw new ArgumentException();
                    }
                }

                //Check if token is a number
                bool isNumeric = int.TryParse(token, out int n);
                if (isNumeric)
                {
                    // If operators has a * or / do the operation
                    if (operators.TryPeek(out string result)
                        && (result.Equals("*") || result.Equals("/")))
                    {
                        if (!integers.TryPop(out int poppedNum))
                            throw new ArgumentException();
                        integers.Push(Operate(poppedNum, n, operators.Pop()));
                    }
                    //If not just push n
                    else
                    {
                        integers.Push(n);
                    }
                    continue;
                }

                //Check if token is + or -
                if (token.Equals("+") || token.Equals("-"))
                {
                    if (operators.TryPeek(out string peekOp)
                        && (peekOp.Equals("+") || peekOp.Equals("-")))
                    {
                        if (integers.Count < 2)
                            throw new ArgumentException();
                        integers.Push(Operate(integers.Pop(), integers.Pop(), operators.Pop()));
                    }

                    operators.Push(token);
                    continue;
                }

                //Check if token is * or /
                if (token.Equals("*") || token.Equals("/"))
                {
                    operators.Push(token);
                }
            }
            if(operators.Count == 0)
            {
                if(integers.Count != 1)
                    throw new ArgumentException();
            }
            else
            {
                if(integers.Count != 2 && operators.Count != 0)
                {
                    throw new ArgumentException();
                }
                else
                {
                    integers.Push(Operate(integers.Pop(), integers.Pop(), operators.Pop()));
                }
            }
            return integers.Pop();
        }
        public static int Operate(int num1, int num2, string operation)
        {
            //Operation is garunteed to be + - * /
            switch (operation)
            {
                case "/":
                    if (num2 == 0)
                        throw new ArgumentException();
                    return num1 / num2;
                case "*":
                    return num1 * num2;
                case "+":
                    return num1 + num2;
                case "-":
                    return num1 - num2;
                default:
                    //this path will never be reached
                    return -1;
            }
        }
    }
}   