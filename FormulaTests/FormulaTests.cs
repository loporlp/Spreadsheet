using SpreadsheetUtilities;
namespace FormulaTests
{
    [TestClass]
    public class FormulaTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            Formula f = new Formula("R2D2" + 5);
        }
    }
}