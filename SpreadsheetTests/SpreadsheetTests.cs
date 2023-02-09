using SpreadsheetUtilities;
using SS;

namespace SpreadsheetTests
{
    [TestClass]
    public class SpreadsheetTests
    {
        /// <summary>
        /// Tests setting with Double
        /// </summary>
        [TestMethod]
        public void SetCellContentsDouble()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A1", 50);

            Assert.AreEqual(sheet.GetCellContents("A1"), 50.0);
        }

        //TODO: SET CONTENTS OTHER OVERRIDES

        /// <summary>
        /// Gets the cell contents of cells
        /// that aren't in the spreadsheet
        /// </summary>
        [TestMethod]
        public void GetCellContentsEmpty()
        {
            Spreadsheet sheet = new Spreadsheet();
            Assert.AreEqual(sheet.GetCellContents("A1"), "");
            Assert.AreEqual(sheet.GetCellContents("_A1"), "");
        }

        /// <summary>
        /// Calls get Cell on an invalid cell name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellBadName()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.GetCellContents("*fs");
        }

        /// <summary>
        /// Calls setCell on an invalid cell name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellBadName()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("*fs", 7);
        }

        /// <summary>
        /// Get the Dependents of the Cell being added
        /// using the SetCell Function
        /// </summary>
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