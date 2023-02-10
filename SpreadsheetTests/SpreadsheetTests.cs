using SpreadsheetUtilities;
using SS;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SpreadsheetTests
{

    /// <summary>
    ///This is a test class for Spreadsheet.cs and is intended
    ///to contain all Spreadsheet.cs Unit Tests
    ///</summary>
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

        /// <summary>
        /// Tests setting with Formula
        /// </summary>
        [TestMethod]
        public void SetCellContentsFormula()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A1", new Formula("1 + 1"));

            Assert.AreEqual(sheet.GetCellContents("A1"), new Formula("1 + 1"));
        }

        /// <summary>
        /// Tests setting with String
        /// </summary>
        [TestMethod]
        public void SetCellContentsString()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A1", "Word");

            Assert.AreEqual(sheet.GetCellContents("A1"), "Word");
        }

        /// <summary>
        /// Tests setting with Double
        /// overriding cell that existed
        /// </summary>
        [TestMethod]
        public void OverrideCellContentsDouble()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A1", "word");
            sheet.SetCellContents("A1", 50);

            Assert.AreEqual(sheet.GetCellContents("A1"), 50.0);
        }

        /// <summary>
        /// Tests setting with Formula
        /// overriding cell that existed
        /// </summary>
        [TestMethod]
        public void OverrideCellContentsFormula()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A1", 5);
            sheet.SetCellContents("A1", new Formula("1 + 1"));

            Assert.AreEqual(sheet.GetCellContents("A1"), new Formula("1 + 1"));
        }

        /// <summary>
        /// Tests setting with String
        /// overriding existing cell
        /// </summary>
        [TestMethod]
        public void OverrideCellContentsString()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A1", 5);
            sheet.SetCellContents("A1", "Word");

            Assert.AreEqual(sheet.GetCellContents("A1"), "Word");
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
        /// using set Cell that takes a double
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellBadName()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("*fs", 7);
        }

        /// <summary>
        /// Calls setCell on an invalid cell name
        /// using set Cell that takes a formula
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellBadNameFormula()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("*fs", new Formula("4"));
        }

        /// <summary>
        /// Calls setCell on an invalid cell name
        /// using set Cell that takes a string
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellBadNameString()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("*fs", "Word");
        }

        /// <summary>
        /// Get the Dependents of the Cell being added
        /// using the SetCell Function that takes a double
        /// </summary>
        [TestMethod]
        public void SetCellDependsDouble()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("B1", new Formula("A1 * 17"));
            sheet.SetCellContents("C1", new Formula("18 + A1"));
            HashSet<string> set = sheet.SetCellContents("A1", 5).ToHashSet<string>();

            Assert.IsTrue(set.Contains("A1"));
            Assert.IsTrue(set.Contains("B1"));
            Assert.IsTrue(set.Contains("C1"));
        }

        /// <summary>
        /// Get the Dependents of the Cell being added
        /// using the SetCell Function that takes a string
        /// </summary>
        [TestMethod]
        public void SetCellDependsString()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("B1", new Formula("A1 * 17"));
            sheet.SetCellContents("C1", new Formula("18 + A1"));
            HashSet<string> set = sheet.SetCellContents("A1", "Word").ToHashSet<string>();

            Assert.IsTrue(set.Contains("A1"));
            Assert.IsTrue(set.Contains("B1"));
            Assert.IsTrue(set.Contains("C1"));
        }

        /// <summary>
        /// Get the Dependents of the Cell being added
        /// using the SetCell Function that takes a formula
        /// </summary>
        [TestMethod]
        public void SetCellDependsFormula()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("B1", new Formula("A1 * 17"));
            sheet.SetCellContents("C1", new Formula("18 + A1"));
            HashSet<string> set = sheet.SetCellContents("A1", new Formula("D6 + H9")).ToHashSet<string>();

            Assert.IsTrue(set.Contains("A1"));
            Assert.IsTrue(set.Contains("B1"));
            Assert.IsTrue(set.Contains("C1"));
        }

        /// <summary>
        /// Get the dependents of the cell being
        /// added using the SetCell Function
        /// and makes sure there are no dupelicates
        /// </summary>
        [TestMethod]
        public void SetCellDependsDoubleVar()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("B1", new Formula("A1 * 17"));
            sheet.SetCellContents("C1", new Formula("18 + A1 + A1"));
            HashSet<string> set = sheet.SetCellContents("A1", 5).ToHashSet<string>();

            Assert.IsTrue(set.Contains("A1"));
            Assert.IsTrue(set.Contains("B1"));
            Assert.IsTrue(set.Contains("C1"));
            Assert.IsTrue(set.Count == 3);
        }

        /// <summary>
        /// Tests Getting a list of non empty cells
        /// </summary>
        [TestMethod]
        public void GetNonEmptyCells()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A1", 5);
            sheet.SetCellContents("B1", new Formula("A1 * 17"));
            sheet.SetCellContents("C1", new Formula("18 + A1 + A1"));

            List<string> names = sheet.GetNamesOfAllNonemptyCells().ToList<string>();

            Assert.IsTrue(names.Contains("A1"));
            Assert.IsTrue(names.Contains("B1"));
            Assert.IsTrue(names.Contains("C1"));
            Assert.IsTrue(names.Count == 3);

        }
        /// <summary>
        /// Get NonEmptyCells with all cells being empty
        /// </summary>
        [TestMethod]
        public void GetNonEmptyCellsEmpty() 
        {
            Spreadsheet sheet = new Spreadsheet();

            List<string> names = sheet.GetNamesOfAllNonemptyCells().ToList<string>();

            Assert.AreEqual(0, names.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void SetCircular()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A1", new Formula("B1 + 6"));
            sheet.SetCellContents("B1", new Formula("C1 + 5"));
            sheet.SetCellContents("C1", new Formula("A1 + 5")); 
        }

        [TestMethod]
        public void SetCircularStaysTheSame()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A1", new Formula("B1 + 6"));
            sheet.SetCellContents("B1", new Formula("C1 + 5"));
            try
            {
                sheet.SetCellContents("C1", new Formula("A1 + 5"));
            } 
            catch (CircularException)
            {
                Assert.IsFalse(sheet.GetNamesOfAllNonemptyCells().Contains("C1"));
            }
            


        }

    }
}