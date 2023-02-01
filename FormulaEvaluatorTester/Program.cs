using FormulaEvaluator;
using System.Text.RegularExpressions;
///<summary> 
///This class runts tests 
///to test the Evaluator class
///</summary>

//Variable tests are conducted throughout the class

//Testing Basic Addition
Console.WriteLine("Addition Tests");
Console.WriteLine(Evaluator.Evaluate("1+1", null) == 2);
Console.WriteLine(Evaluator.Evaluate("10+8", null) == 18);
Console.WriteLine(Evaluator.Evaluate("Z23+A1", (x) => 5) == 10);
Console.WriteLine(Evaluator.Evaluate("2+A1", (x) => 5) == 7);

//Testing Basic Multiplication
Console.WriteLine("Multiplication Test");
Console.WriteLine(Evaluator.Evaluate("2*8", null) == 16);
Console.WriteLine(Evaluator.Evaluate("2*     8", null) == 16);
Console.WriteLine(Evaluator.Evaluate("20 * AB132", (x) => 10) == 200);
Console.WriteLine(Evaluator.Evaluate("(((7 + 8) * (16 / 4) * 4 )+ 13) + 100/10 + 5/5*10", null) == 273 );
Console.WriteLine("FAILED TEST");
Console.WriteLine(Evaluator.Evaluate("y1*3-8/2+4*(8-9*2)/14*x7", s => (s == "x7") ? 1 : 4));

//Testing Basic Division
Console.WriteLine("Division Tests");
Console.WriteLine(Evaluator.Evaluate("8/4", null) == 2);
Console.WriteLine(Evaluator.Evaluate("20/B7", (x) => 5) == 4);
Console.WriteLine(Evaluator.Evaluate("(8/4)", null) == 2);
Console.WriteLine(Evaluator.Evaluate("(16/4) / 2", null) == 2);
Console.WriteLine(Evaluator.Evaluate("16 / (8/4) / 2", null) == 4);
try
{
    Evaluator.Evaluate("20/0", null);
} catch(ArgumentException)
{
    Console.WriteLine("True");
}

//Testing Complex Equations
Console.WriteLine("Division Tests");
Console.WriteLine(Evaluator.Evaluate("4 + 8/4", null) == 6);
Console.WriteLine(Evaluator.Evaluate("2*8+16/4+18", null) == 38);
Console.WriteLine(Evaluator.Evaluate("(5 + 19)", (x) => 5) == 24);
Console.WriteLine(Evaluator.Evaluate("2*8+16/4+18", null) == 38);
Console.WriteLine(Evaluator.Evaluate("5 + (8 * (10 + 10)) - 5", null) == 160);
Console.WriteLine(Evaluator.Evaluate("500 - ( 15 * 20 + (75 + 5) + (18 + 2)) / 5", null) == 420);


//Testing Misc Input
Console.WriteLine("Misc Input Tests");
Console.WriteLine(Evaluator.Evaluate("1", null) == 1);
try
{
    Evaluator.Evaluate("asdkfjq908e98 S(* s (Df9U* Su98 S", null);
}
catch (ArgumentException)
{
    Console.WriteLine("True");
}
try
{
    Evaluator.Evaluate(" -A- ", null);
}
catch (ArgumentException)
{
    Console.WriteLine("True");
}


Console.WriteLine("REGEX TEST");
Console.WriteLine(Regex.IsMatch("R2D2", @"[a-zA-Z_][a-zA-Z0-9_]*$"));

