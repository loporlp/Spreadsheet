// See https://aka.ms/new-console-template for more information
using FormulaEvaluator;
using System.Text.RegularExpressions;

Console.WriteLine(Evaluator.Evaluate("1", null) == 1);
Console.WriteLine(Evaluator.Evaluate("2*8", null) == 16);
Console.WriteLine(Evaluator.Evaluate("10+8", null) == 18);
Console.WriteLine(Evaluator.Evaluate("8/4", null) == 2);
Console.WriteLine(Evaluator.Evaluate("4 + 8/4", null) == 6);
Console.WriteLine(Evaluator.Evaluate("2*     8", null) == 16);
Console.WriteLine(Evaluator.Evaluate("2*8+16/4+18", null) == 38);
