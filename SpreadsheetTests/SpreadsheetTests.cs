using SpreadsheetUtilities;
using SS;

namespace SpreadsheetTests
{
    [TestClass]
    public class SpreadsheetTests
    {
        [TestMethod]
        public void SetCellContents()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A1", 50);

            Assert.AreEqual(sheet.GetCellContents("A1"), 50.0);
        }

        [TestMethod]
        public void GetCellContentsEmpty()
        {
            Spreadsheet sheet = new Spreadsheet();
            Assert.AreEqual(sheet.GetCellContents("A1"), "");
            Assert.AreEqual(sheet.GetCellContents("_A1"), "");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellBadName()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.GetCellContents("*fs");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellBadName()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("*fs", 7);
        }

        [TestMethod]
        public void SetCellDepends()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("B1", new Formula("A1 * 17"));
            sheet.SetCellContents("C1", new Formula("18 + A1"));
            HashSet<string> set = sheet.SetCellContents("A1", 5).ToHashSet<string>();

            Assert.IsTrue(set.Contains("A1"));
            Assert.IsTrue(set.Contains("B1"));
            Assert.IsTrue(set.Contains("C1"));
        }
    }
}