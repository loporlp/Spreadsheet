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

        public Spreadsheet() 
        {
            cells = new Dictionary<string, Cell>();
            dependencyGraph = new DependencyGraph();
        }

        /// <inheritdoc/>
        public override object GetCellContents(string name)
        {
            if(!Regex.IsMatch(name, @"^[a-zA-Z_][a-zA-Z_0-9]*$"))
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
            return cells.Keys;
        }

        /// <inheritdoc/>
        public override ISet<string> SetCellContents(string name, double number)
        {
            if (!Regex.IsMatch(name, @"^[a-zA-Z_][a-zA-Z_0-9]*$"))
            {
                throw new InvalidNameException();
            }

            if (cells.ContainsKey(name))
            {
                cells[name] = new Cell(number);
            }
            else
            {
                cells.Add(name, new Cell(number));
            }

            HashSet<string> set = new HashSet<string>();

            set.Add(name);

            foreach (string dependee in dependencyGraph.GetDependees(name))
            {
                set.Add(dependee);
            }

            return set;
        }

        /// <inheritdoc/>
        public override ISet<string> SetCellContents(string name, string text)
        {
            if (!Regex.IsMatch(name, @"^[a-zA-Z_][a-zA-Z_0-9]*$"))
            {
                throw new InvalidNameException();
            }

            if (cells.ContainsKey(name))
            {
                cells[name] = new Cell(text);
            }

            HashSet<string> set = new HashSet<string>();

            set.Add(name);

            foreach (string dependee in dependencyGraph.GetDependees(name))
            {
                set.Add(dependee);
            }

            return set;
        }

        /// <inheritdoc/>
        public override ISet<string> SetCellContents(string name, Formula formula)
        {
            if (!Regex.IsMatch(name, @"^[a-zA-Z_][a-zA-Z_0-9]*$"))
            {
                throw new InvalidNameException();
            }

            if (cells.ContainsKey(name))
            {
                cells[name] = new Cell(formula);
            }

            foreach(string variable in formula.GetVariables())
            {
                dependencyGraph.AddDependency(name, variable);
            }

            HashSet<string> set = new HashSet<string>();

            set.Add(name);

            foreach (string dependee in dependencyGraph.GetDependees(name))
            {
                set.Add(dependee);
            }

            return set;
        }

        /// <inheritdoc/>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            return dependencyGraph.GetDependents(name);
        }
        /// <summary>
        /// Private Class Representing a Cell
        /// </summary>
        private class Cell
        {
            private object content;
            public Cell(object content)
            {
                this.content = content;
            }
            /// <summary>
            /// Sets the Value of the Cell Object
            /// </summary>
            /// <param name="item"></param>

            public void setCell(object item)
            {
                content = item;
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


