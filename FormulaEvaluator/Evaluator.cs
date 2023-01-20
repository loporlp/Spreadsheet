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
        /// <summary>
        /// looks up given variable and return the number it represents
        /// </summary>
        /// <param name="variableName"> variable to be looked up</param>
        /// <returns></returns>
        public delegate int Lookup(string variableName);

        /// <summary>
        ///     This function takes in a String Infix expression and variable
        ///     lookup function and evalutates and returns an answer
        /// </summary>
        /// <param name="expression"> expression to be evalutated</param>
        /// <param name="variableEvaluator"> Variables to be evaluated</param>
        /// <returns> int solution to the expression </returns>
        /// <exception cref="ArgumentException"> thows if invalid expression is given
        /// like divide by 0 or invalid Infix notation is also thrown if a variable is
        /// unable to be looked up using the Lookup function</exception>
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
                if (Regex.IsMatch(token, @"^[a-zA-Z]+[0-9]+$"))
                {
                    try
                    {
                        token = variableEvaluator(token).ToString();
                    } catch
                    {
                        throw new ArgumentException();
                    }
                }

                //Check if token is a number
                bool isNumeric = int.TryParse(token, out int n);
                if (isNumeric)
                {
                    // If operators has a * or / do the operation
                    if (operators.TryPeek(out string? result)
                        && (result == "*" || result == "/"))
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
                if (token == "+" || token == "-")
                {
                    if (operators.TryPeek(out string? peekOp)
                        && (peekOp == "+" || peekOp == "-"))
                    {
                        if (integers.Count < 2)
                            throw new ArgumentException();
                        integers.Push(Operate(integers.Pop(), integers.Pop(), operators.Pop()));
                    }

                    operators.Push(token);
                    continue;
                }

                //Check if token is * or /
                if (token == "*" || token == "/")
                {
                    operators.Push(token);
                    continue;
                }

                //Check if token is "(" or ")"
                if (token == "(")
                {
                    operators.Push(token);
                    continue;
                }

                if (token == ")")
                {
                    //Step 1 from algorithm
                    if (operators.TryPeek(out string? peekOp))
                    {
                        if(peekOp == "+" || peekOp == "-")
                        {
                            if (integers.Count < 2)
                            {
                                throw new ArgumentException();
                            }
                            else
                            {
                                integers.Push(Operate(integers.Pop(), integers.Pop(), operators.Pop()));
                            }
                        }
                        
                    }
                    else
                    {
                        throw new ArgumentException();
                    }

                    //Step 2 from algorithm
                    if (!operators.TryPop(out string? popOp) || popOp != "(")
                    {
                        throw new ArgumentException();
                    }   

                    //Step 3 from algorithm
                    if (operators.TryPeek(out peekOp)
                        && (peekOp == "*" || peekOp == "/"))
                    {
                        if (integers.Count < 2)
                        {
                            throw new ArgumentException();
                        }
                        else
                        {
                            integers.Push(Operate(integers.Pop(), integers.Pop(), operators.Pop()));
                        }
                    }

                    continue;
                }

                if (token == "")
                    continue;

                //Invalid token
                throw new ArgumentException();
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
                    //This time subtraction needs to be done the other way round
                    int num1 = integers.Pop();
                    integers.Push(Operate(integers.Pop(), num1, operators.Pop()));
                }
            }
            return integers.Pop();
        }
        /// <summary>
        /// Takes in 2 numbers and one of the following
        /// operations [+, -, *, /]
        /// and returns the result of num1 operation num2
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <param name="operation"></param>
        /// <returns>result of num1 operation num2</returns>
        /// <exception cref="ArgumentException"> throws if trying to divide by 0</exception>
        private static int Operate(int num1, int num2, string operation)
        {
            //Operation should be + - * /
            switch (operation)
            {
                case "/":
                    if (num2 == 0)
                        //divide by 0 error
                        throw new ArgumentException();
                    return num1 / num2;
                case "*":
                    return num1 * num2;
                case "+":
                    return num1 + num2;
                case "-":
                    return num1 - num2;
                default:
                    //if incorrect math operation is sent
                    throw new ArgumentException();
            }
        }
    }
}   