using FormulaEvaluator;
using System.Text.RegularExpressions;

Console.WriteLine(Evaluator.Evaluate("1+1", null) == 2);
Console.WriteLine(Evaluator.Evaluate("1", null) == 1);
Console.WriteLine(Evaluator.Evaluate("2*8", null) == 16);
Console.WriteLine(Evaluator.Evaluate("10+8", null) == 18);
Console.WriteLine(Evaluator.Evaluate("8/4", null) == 2);
Console.WriteLine(Evaluator.Evaluate("4 + 8/4", null) == 6);
Console.WriteLine(Evaluator.Evaluate("2*     8", null) == 16);
Console.WriteLine(Evaluator.Evaluate("2*8+16/4+18", null) == 38);

Console.WriteLine("REGEX TESTS");
Console.WriteLine(Regex.IsMatch("AAaaAA10", @"[a-zA-Z]+[0-9]+"));
Console.WriteLine(Regex.IsMatch("10", @"\w+[0-9]+"));
Console.WriteLine(Regex.IsMatch("10", @"^\w+\d+"));

string? test;
test = null;
Console.WriteLine(test);

