using SpreadsheetUtilities;
namespace FormulaTests
{
    [TestClass]
    public class FormulaTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            Formula f = new Formula("1 + 1");
            
            Assert.AreEqual(2, (double)f.Evaluate(s => 1));
        }

        [TestMethod]
        public void ToStringBasic()
        {
            Formula f = new Formula("1 + 1 - x3", s => s.ToUpper(), s => true );
            Assert.AreEqual("1+1-X3", f.ToString());
        }

        [TestMethod]
        public void EqualsSpaces()
        {
            Formula f = new Formula("1 + 1 - x3", s => s.ToUpper(), s => true);
            Formula f2 = new Formula("  1 +     1 - X3", s => s.ToUpper(), s => true);
            Assert.IsTrue(f.Equals(f2));
        }

        [TestMethod]
        public void EqualsHashCode()
        {
            Formula f = new Formula("1 + 1 - x3", s => s.ToUpper(), s => true);
            Formula f2 = new Formula("  1 +     1 - X3", s => s.ToUpper(), s => true);
            Assert.AreEqual(f.GetHashCode(), f2.GetHashCode());
        }

        [TestMethod]
        public void DoesNotEqualHashCode()
        {
            Formula f = new Formula("x1+x2");
            Formula f2 = new Formula("y2+x1");
            Assert.AreNotEqual(f.GetHashCode, f2.GetHashCode);
        }

        [TestMethod]
        public void EqualsDifferentNorm()
        {
            Formula f = new Formula("1+1-x3", s => s, s => true);
            Formula f2 = new Formula("1+1-x3", s => s.ToUpper(), s => true);
            Assert.IsFalse(f.Equals(f2));
        }

        [TestMethod]
        public void EqualsSwap()
        {
            Formula f = new Formula("x1+x2");
            Formula f2 = new Formula("y2+x1");
            Assert.IsFalse(f.Equals(f2));
        }

        [TestMethod]
        public void EqualsDoubles()
        {
            Formula f = new Formula("2.0 + x7");
            Formula f2 = new Formula("2.000 + x7");
            Assert.IsTrue(f.Equals(f2));
        }

        [TestMethod]
        public void EqualsOverload()
        {
            Formula f = new Formula("2.0 + x7");
            Formula f2 = new Formula("2.000 + x7");
            Assert.IsTrue(f == f2);
        }

        [TestMethod]
        public void DoesNotEqualsOverload()
        {
            Formula f = new Formula("2.0 + x7");
            Formula f2 = new Formula("2.000 + x7");
            Assert.IsFalse(f != f2);
        }

        [TestMethod]
        public void ScientificNotationPositive()
        {
            Formula f = new Formula("5e-5 * 100000");
            Assert.AreEqual(f.Evaluate(s => 0), 5.0);
        }

        [TestMethod()]
        public void TestComplexNestedParensRight()
        {
            Formula f = new Formula("x1+(x2+(x3+(x4+(x5+x6))))");
            Assert.AreEqual(6.0, f.Evaluate(s => 1));
        }

        [TestMethod()]
        public void TestComplexNestedParensLeft()
        {
            Formula f = new Formula("((((x1+x2)+x3)+x4)+x5)+x6");
            Assert.AreEqual(12.0, f.Evaluate(s => 2));
        }

        [TestMethod()]
        public void TestRepeatedVar()
        {
            Formula f = new Formula("a4-a4*a4/a4");
            Assert.AreEqual(0.0, f.Evaluate(s => 3));
        }

        [TestMethod()]
        public void TestComplexAndParentheses()
        {
            Formula f = new Formula("2+3*5+(3+4*8)*5+2");
            Assert.AreEqual(194.0, f.Evaluate(s => 0));
        }

        [TestMethod()]
        public void TestWrongFormat()
        {
            Formula f = new Formula("2*((3+5)");
            Assert.IsTrue(f.Evaluate(s => 0) is FormulaError);
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestWrongFormatNormalizer()
        {
            Formula f = new Formula("2 + _x", s => s, s => false);
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestInvalidVariable()
        {
            Formula f = new Formula("2 + ?");
        }

        [TestMethod()]
        public void TestVariableDoesNotExist()
        {
            Formula f = new Formula("2 + _x", s => s, s => true);
            Assert.IsTrue(f.Evaluate(s => { throw new ArgumentException(); }) is FormulaError);
        }

        [TestMethod()]
        public void TestComplexMultiVar()
        {
            Formula f = new Formula("y1*3-8/2+4*(8-9*2)/14*x7");
            Assert.AreEqual(5.142857142857142, f.Evaluate(s => (s == "x7") ? 1 : 4));
        }

        [TestMethod()]
        public void TestComplexNoVar()
        {
            Formula f = new Formula("5*6+(18+11/(6+8/(5)))");
            Assert.AreEqual(49.44736842105263, f.Evaluate(s => 5));
        }

        [TestMethod()]
        public void TestGetVariables()
        {
            Formula f = new Formula("x2 + 6 + xx / (_x + 5)");
            List<string> variables = f.GetVariables().ToList<string>();
            Assert.IsTrue(variables.Contains("x2"));
            Assert.IsTrue(variables.Contains("xx"));
            Assert.IsTrue(variables.Contains("_x"));
        }
    }
}