using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SS
{
    /// <summary>
    /// Author: Mason Sansom
    /// Partner: -none-
    /// Date: 8-Feb-2023
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
    /// This Class Represents a Spreadsheet Currently implements
    /// AbstractSpreadsheet.cs
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        private Dictionary<string, Cell> cells;
        private DependencyGraph dependencyGraph;
        private Func<string, bool> isValid;
        private Func<string, string> normalize;
        private string version;

        public override bool Changed { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }

        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {
            this.isValid = isValid;
            this.normalize = normalize;
            this.version = version;
            cells = new Dictionary<string, Cell>();
            dependencyGraph = new DependencyGraph();
        }

        public Spreadsheet(string filePath, Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {
            //TODO implement filePath
        }

        public Spreadsheet() : base(s => true, s => s, "default")
        {
            this.isValid = s => true;
            this.normalize = s => s;
            this.version = "default";
            cells = new Dictionary<string, Cell>();
            dependencyGraph = new DependencyGraph();
        }

        /// <inheritdoc/>
        public override object GetCellContents(string name)
        {
            if(!Regex.IsMatch(name, @"^[a-zA-Z]+[0-9]+$"))
            {
                throw new InvalidNameException();
            }

            if(cells.ContainsKey(name))
            {
                return cells[name].getContent();
            }

            return "";
        }

        /// <inheritdoc/>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            HashSet<string> names = new HashSet<string>();
            foreach(string cellName in cells.Keys)
            {
                if(cells[cellName].getContent().ToString() != "")
                {
                    names.Add(cellName);
                }
            }

            return names;
        }

        /// <inheritdoc/>
        protected override IList<string> SetCellContents(string name, double number)
        {
            if (!Regex.IsMatch(name, @"^[a-zA-Z]+[0-9]+$") || !isValid(name))
            {
                throw new InvalidNameException();
            }

            if (cells.ContainsKey(name))
            {
                dependencyGraph.ReplaceDependents(name, new List<string>());
                cells[name] = new Cell(number);
            }
            else
            {
                cells.Add(name, new Cell(number));
            }

            List<string> list = new List<string>();

            foreach (string dependee in this.GetCellsToRecalculate(name))
            {
                list.Add(dependee);
            }

            return list;
        }

        /// <inheritdoc/>
        protected override IList<string> SetCellContents(string name, string text)
        {
            if (!Regex.IsMatch(name, @"^[a-zA-Z]+[0-9]+$") || !isValid(name))
            {
                throw new InvalidNameException();
            }

            if (cells.ContainsKey(name))
            {
                dependencyGraph.ReplaceDependents(name, new List<string>());
                cells[name] = new Cell(text);
            }
            else
            {
                cells.Add(name, new Cell(text));
            }

            List<string> list = new List<string>();

            foreach (string dependee in this.GetCellsToRecalculate(name))
            {
                list.Add(dependee);
            }

            return list;
        }

        /// <inheritdoc/>
        protected override IList<string> SetCellContents(string name, Formula formula)
        {

            if (!Regex.IsMatch(name, @"^[a-zA-Z]+[0-9]+$") || !isValid(name))
            {
                throw new InvalidNameException();
            }

            object oldItem = "";

            if (cells.ContainsKey(name))
            {
                dependencyGraph.ReplaceDependents(name, new List<string>());
                oldItem = cells[name].getContent();
                cells[name] = new Cell(formula);
            }
            else
            {
                cells.Add(name, new Cell(formula));
            }

            foreach (string variable in formula.GetVariables())
            {
                dependencyGraph.AddDependency(name, variable);
            }

            try
            {
                GetCellsToRecalculate(name);
            }
            catch (CircularException) 
            {
                foreach (string variable in formula.GetVariables())
                {
                    dependencyGraph.RemoveDependency(name, variable);
                }

                if (oldItem is Formula)
                {
                    this.SetCellContents(name, (Formula)oldItem);
                }
                else if (oldItem is string)
                {
                    this.SetCellContents(name, (string)oldItem);
                }
                else 
                {
                    this.SetCellContents(name, (double)oldItem);
                }

                

                throw new CircularException();
            }

            List<string> list = new List<string>();

            foreach (string dependee in this.GetCellsToRecalculate(name))
            {
                list.Add(dependee);
            }

            return list;
        }

        /// <inheritdoc/>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            return dependencyGraph.GetDependees(name);
        }

        public override IList<string> SetContentsOfCell(string name, string content)
        {
            if (!Regex.IsMatch(name, @"^[a-zA-Z]+[0-9]+$") || !isValid(name))
            {
                throw new InvalidNameException();
            }

            content = content.Trim();

            if(double.TryParse(content, out double number))
            {
                return SetCellContents(name, number);
            }

            if (content.Count() > 1 && content[0].Equals('='))
            {
                object formula = new Formula(content[1..], normalize, isValid);

                if(formula is FormulaError)
                {
                    throw new FormulaFormatException("Invalid Formula");
                }

                //This will throw a CircularException if one is created
               return SetCellContents(name, (Formula)formula);
            }

            return SetCellContents(name, content);
        }

        public override string GetSavedVersion(string filename)
        {
            throw new NotImplementedException();
        }

        public override void Save(string filename)
        {
            throw new NotImplementedException();
        }

        public override object GetCellValue(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Private Class Representing a Cell
        /// </summary>
        private class Cell
        {
            private object content;
            private object value;
            public Cell(object content)
            {
                this.content = content;
                if(content is Formula)
                {
                    Formula form = (Formula)content;
                  //  value = form.Evaluate();
                }
                else
                {
                    value = content;
                }
            }
            /// <summary>
            /// Returns the Value of the Cell Obeject
            /// </summary>
            /// <returns></returns>
            public object getContent()
            {
                return content;
            }
        }
    }
}


