using SpreadsheetUtilities;
using SS;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SpreadsheetTests
{

    /// <summary>
    /// Author: Mason Sansom
    /// Partner: -none-
    /// Date: 14-Feb-2023
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
    ///This Class Does Extensive testing on Spreadsheet.cs
    /// </summary>
    [TestClass]
    public class SpreadsheetTests
    {
        /// <summary>
        /// Tests setting with Double
        /// </summary>
        [TestMethod]
        public void SetContentsOfCellDouble()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "50");

            Assert.AreEqual(sheet.GetCellContents("A1"), 50.0);
        }

        /// <summary>
        /// Tests setting with Formula
        /// </summary>
        [TestMethod]
        public void SetContentsOfCellFormula()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=1 + 1");

            Assert.AreEqual(sheet.GetCellContents("A1"), "=1+1");
        }

        /// <summary>
        /// Tests setting with String
        /// </summary>
        [TestMethod]
        public void SetContentsOfCellString()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "Word");

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
            sheet.SetContentsOfCell("A1", "word");
            sheet.SetContentsOfCell("A1", "50");

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
            sheet.SetContentsOfCell("A1", "5");
            sheet.SetContentsOfCell("A1", "=1 + 1");

            Assert.AreEqual(sheet.GetCellContents("A1"), "=1+1");
        }

        /// <summary>
        /// Tests setting with String
        /// overriding existing cell
        /// </summary>
        [TestMethod]
        public void OverrideCellContentsString()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "5");
            sheet.SetContentsOfCell("A1", "Word");

            Assert.AreEqual(sheet.GetCellContents("A1"), "Word");
        }

        /// <summary>
        /// Gets the cell contents of cells
        /// that aren't in the spreadsheet
        /// </summary>
        [TestMethod]
        public void GetCellContentsEmpty()
        {
            Spreadsheet sheet = new Spreadsheet();
            Assert.AreEqual(sheet.GetCellContents("A1"), "");
            Assert.AreEqual(sheet.GetCellContents("B1"), "");
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
            sheet.SetContentsOfCell("*fs", "7");
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
            sheet.SetContentsOfCell("*fs", "4");
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
            sheet.SetContentsOfCell("*fs", "Word");
        }

        /// <summary>
        /// Get the Dependents of the Cell being added
        /// using the SetCell Function that takes a double
        /// </summary>
        [TestMethod]
        public void SetCellDependsDouble()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("B1", "=A1 * 17");
            sheet.SetContentsOfCell("C1", "=18 + A1");
            HashSet<string> set = sheet.SetContentsOfCell("A1", "5").ToHashSet<string>();

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
            sheet.SetContentsOfCell("B1", "=A1 * 17");
            sheet.SetContentsOfCell("C1", "= 18 + A1");
            HashSet<string> set = sheet.SetContentsOfCell("A1", "Word").ToHashSet<string>();

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
            sheet.SetContentsOfCell("B1", "=A1 * 17");
            sheet.SetContentsOfCell("C1", "=18 + A1");
            HashSet<string> set = sheet.SetContentsOfCell("A1", "D6 + H9").ToHashSet<string>();

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
            sheet.SetContentsOfCell("B1", "=A1 * 17");
            sheet.SetContentsOfCell("C1", "=18 + A1 + A1");
            HashSet<string> set = sheet.SetContentsOfCell("A1", "5").ToHashSet<string>();

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
            sheet.SetContentsOfCell("A1", "5");
            sheet.SetContentsOfCell("B1", "A1 * 17");
            sheet.SetContentsOfCell("C1", "18 + A1 + A1");

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

        /// <summary>
        /// Makes sure Circular Exception is thrown
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void SetCircular()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=B1 + 6");
            sheet.SetContentsOfCell("B1", "=C1 + 5");
            sheet.SetContentsOfCell("C1", "=A1 + 5"); 
        }

        /// <summary>
        /// Makes sure Spreadsheet stays the same after circular
        /// exception is thrown
        /// </summary>
        [TestMethod]
        public void SetCircularStaysTheSame()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=B1 + 6");
            sheet.SetContentsOfCell("B1", "=C1 + 5");
            try
            {
                sheet.SetContentsOfCell("C1", "=A1 + 5");
            } 
            catch (CircularException)
            {
                Assert.IsFalse(sheet.GetNamesOfAllNonemptyCells().Contains("C1"));
            }
        }

        /// <summary>
        /// Makes sure Spreadsheet stays the same with a double
        /// after circular exception is thrown
        /// </summary>
        [TestMethod]
        public void SetCircularStaysTheSameDouble()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=B1 + 6");
            sheet.SetContentsOfCell("B1", "=C1 + 5");
            sheet.SetContentsOfCell("C1", "5");
            try
            {
                sheet.SetContentsOfCell("C1", "=A1 + 5");
            }
            catch (CircularException)
            {
                Assert.AreEqual(sheet.GetCellContents("C1"), 5.0);
            }
        }

        /// <summary>
        /// Tests getting the value of a formula content cell
        /// </summary>
        [TestMethod]
        public void CellValueCalculated()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=5 + 5");
            Assert.AreEqual((double)sheet.GetCellValue("A1"), 10);
        }
        
        /// <summary>
        /// Tests that a formula containing other cells as variables
        /// is calculated correctly
        /// </summary>
        [TestMethod]
        public void CellValueFromOtherCells()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=A2 + 5");
            sheet.SetContentsOfCell("A2", "5");
            Assert.AreEqual((double)sheet.GetCellValue("A1"), 10);
        }

        /// <summary>
        /// Tests that the value of a cell containing a formula
        /// with other cell name variables is calculated even if the other
        /// cells were created before or after formula
        /// </summary>
        [TestMethod]
        public void CellValueFromOtherCellsComplex()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A6", "5");
            sheet.SetContentsOfCell("A2", "=A3 + 5");
            sheet.SetContentsOfCell("A1", "=A2 + 5");           
            sheet.SetContentsOfCell("A4", "=A5 + A6");
            sheet.SetContentsOfCell("A3", "=A4 + 5");
            sheet.SetContentsOfCell("A5", "5");
            Assert.AreEqual((double)sheet.GetCellValue("A1"), 25);
        }

        /// <summary>
        /// Invalid formula should throw an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void InvalidFormula()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=B1 + ?");
        }

        /// <summary>
        /// makes sure changed is updating
        /// </summary>
        [TestMethod]
        public void ChangedSimple() 
        {
            Spreadsheet sheet = new Spreadsheet();

            Assert.IsFalse(sheet.Changed);

            sheet.SetContentsOfCell("A1", "5");

            Assert.IsTrue(sheet.Changed);

            sheet.Save("save.txt");

            Assert.IsFalse(sheet.Changed);

        }

        /// <summary>
        /// Make sure change is not updated after attempting a circular exception
        /// </summary>
        [TestMethod]
        public void ChangedFormulaCirlce()
        {
            Spreadsheet sheet = new Spreadsheet();

            Assert.IsFalse(sheet.Changed);

            sheet.SetContentsOfCell("A1", "A2 + 5");

            try
            {
                sheet.SetContentsOfCell("A2", "A1 + 5");
            } catch ( CircularException)
            {
                Assert.IsFalse(sheet.Changed);
            } 
        }

        /// <summary>
        /// makes sure cells can hold strings
        /// </summary>
        [TestMethod]
        public void ChangedString()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "Hello");

            Assert.AreEqual(sheet.GetCellContents("A1"), "Hello");
        }

        /// <summary>
        /// Makes sure file is getting saved correctly
        /// </summary>
        [TestMethod]
        public void SaveSimple()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "5  + 6");
            sheet.SetContentsOfCell("A2", "6");
            sheet.SetContentsOfCell("A3", "Hi");

            sheet.Save("SavedSheet.txt");

            Assert.AreEqual(sheet.GetSavedVersion("SavedSheet.txt"), "default");
        }

        /// <summary>
        /// gets the correct version from a saved spreadsheet
        /// </summary>
        [TestMethod]
        public void GetVersion()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "5  + 6");
            sheet.SetContentsOfCell("A2", "6");
            sheet.SetContentsOfCell("A3", "Hi");

            sheet.Save("SavedSheet.txt");

            Assert.AreEqual("default", sheet.GetSavedVersion("SavedSheet.txt"));
        }

        /// <summary>
        /// Builds spreadsheet from a saved file
        /// </summary>
        [TestMethod]
        public void SpreadSheetReadFromFile()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "5 + 6");
            sheet.SetContentsOfCell("A2", "6");
            sheet.SetContentsOfCell("A3", "Hi");

            sheet.Save("saved.txt");

            Spreadsheet sheet2 = new Spreadsheet("saved.txt", s => true, s => s, "default");

            Assert.AreEqual(sheet2.GetCellContents("A1"), "5 + 6");
        }

        /// <summary>
        /// Exception is thrown from invalid filepath
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void invalidFilePath()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.Save("totally/bad/directory.xml");
        }

        /// <summary>
        /// Exception is thrown from invalid file path while getting 
        /// the version
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void InvalidFilePathGetVersion()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.GetSavedVersion("totally/bad/directory.xml");
        }

        /// <summary>
        /// If building a spreadsheet from a file and versions
        /// do not match an exception is thrown
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void InvalidFilePathVersion()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.Save("save.txt");

            sheet = new Spreadsheet("save.txt", s => true, s => s, "wrong");
        }

        /// <summary>
        /// Exception is thrown when getCellContents is given invalid
        /// cell name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void InvalidCellNameContent()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.GetCellContents("?");
        }

        /// <summary>
        /// Exception is throw when getcellvalue is given invalid
        /// cell name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void InvalidCellNameValue()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.GetCellValue("?");
        }

        /// <summary>
        /// Makes sure that Cell names are being Normalized
        /// </summary>
        [TestMethod]
        public void Normalize()
        {
            Spreadsheet sheet = new Spreadsheet(s => true, s => s.ToUpper(), "1.0");

            sheet.SetContentsOfCell("a1", "=a2 + 6");
            sheet.SetContentsOfCell("A2", "=a3");
            sheet.SetContentsOfCell("a3", "5");
            sheet.SetContentsOfCell("A3", "6");


            Assert.AreEqual(sheet.GetCellContents("A1").ToString(), "=A2+6");
            Assert.AreEqual(sheet.GetCellContents("a2").ToString(), "=A3");
            Assert.AreEqual(sheet.GetCellContents("A3"), 6.0);
        }
    }
}