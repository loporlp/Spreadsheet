using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace SS
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
        private bool changed;

        public override bool Changed { get => changed; protected set => throw new NotImplementedException(); }

        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {
            this.isValid = isValid;
            this.normalize = normalize;
            this.version = version;
            cells = new Dictionary<string, Cell>();
            dependencyGraph = new DependencyGraph();
            changed = false;
        }

        public Spreadsheet(string filePath, Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {

            this.isValid = isValid;
            this.normalize = normalize;
            this.version = version;
            cells = new Dictionary<string, Cell>();
            dependencyGraph = new DependencyGraph();
            changed = false;

            if (!GetSavedVersion(filePath).Equals(version))
                throw new SpreadsheetReadWriteException("Invalid Version");

            try
            {
                using (XmlReader reader = XmlReader.Create(filePath))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement() && reader.Name == "cell")
                        {
                            reader.ReadToFollowing("name");
                            string name = reader.ReadElementContentAsString();
                            reader.ReadToFollowing("contents");
                            string content = reader.ReadElementContentAsString();

                            this.SetCellContents(name, content);
                        }
                    }
                }
            } catch(Exception)
            {
                throw new SpreadsheetReadWriteException("Error Reading From Spreadsheet");
            }   
        }

        public Spreadsheet() : base(s => true, s => s, "default")
        {
            this.isValid = s => true;
            this.normalize = s => s;
            this.version = "default";
            cells = new Dictionary<string, Cell>();
            dependencyGraph = new DependencyGraph();
            changed = false;
        }

        /// <inheritdoc/>
        public override object GetCellContents(string name)
        {
            name = normalize(name);

            if(!Regex.IsMatch(name, @"^[a-zA-Z]+[0-9]+$") || !isValid(name))
            {
                throw new InvalidNameException();
            }

            if(cells.ContainsKey(name))
            {
                if (cells[name].getContent() is Formula)
                {
                    return "="+cells[name].getContent().ToString();
                }
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

            Formula value = (Formula)cells[name].getContent();

            cells[name].setValue(value.Evaluate(s => Lookup(name)));

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
            name = normalize(name);

            if (!Regex.IsMatch(name, @"^[a-zA-Z]+[0-9]+$") || !isValid(name))
            {
                throw new InvalidNameException();
            }

            IList<string> cellsToCalculate = new List<string>();
            content = content.Trim();

            if(double.TryParse(content, out double number))
            {
                cellsToCalculate = SetCellContents(name, number);
            }
            else if (content.Count() > 1 && content[0].Equals('='))
            {
                object formula = new Formula(content[1..], normalize, isValid);

                //This will throw a CircularException if one is created
                cellsToCalculate = SetCellContents(name, (Formula)formula);
            }
            else
            {
                cellsToCalculate = SetCellContents(name, content);
            }
            
            foreach(String cellName in cellsToCalculate) 
            {
                if (cells[cellName].getContent() is Formula)
                {
                    Formula form = (Formula)cells[cellName].getContent();
                    cells[cellName].setValue(form.Evaluate(s => Lookup(s)));
                }
            }

            changed = true;
            return cellsToCalculate;

        }

        /// <inheritdoc/>
        public override string GetSavedVersion(string filename)
        {
            try
            {
                using (XmlReader reader = XmlReader.Create(filename))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            return reader.GetAttribute(0);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw new SpreadsheetReadWriteException("Error Reading From File");
            }

           throw new SpreadsheetReadWriteException("Unable to Read From File");
        }

        /// <inheritdoc/>
        public override void Save(string filename)
        {
            changed = false;
            try
            {
                WriteXml(filename);
            }
            catch (Exception)
            {
                throw new SpreadsheetReadWriteException("Error Reading From Spreadsheet");
            }
            
        }

        /// <summary>
        /// Write an XML representation of this Spreadsheet.
        /// </summary>
        /// <param Name="filename">The Name of the file where the XML representation will be saved.</param>
        private void WriteXml(string filename)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";

            using (XmlWriter writer = XmlWriter.Create(filename, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Spreadsheet");

                // Version of Spreadheet
                writer.WriteAttributeString("Version", version);


                // write the states themselves
                foreach (String s in cells.Keys)
                {
                    writer.WriteStartElement("cell");
                    writer.WriteElementString("name", s);
                    writer.WriteElementString("contents", cells[s].getContent() + "");
                    writer.WriteEndElement();
                }

                writer.WriteEndElement(); 
                writer.WriteEndDocument();
            }
        }


        /// <inheritdoc/>
        public override object GetCellValue(string name)
        {
            if (!Regex.IsMatch(name, @"^[a-zA-Z]+[0-9]+$"))
            {
                throw new InvalidNameException();
            }

            if (cells.ContainsKey(name))
            {
                return cells[name].getValue();
            }

            return "";
        }

        /// <summary>
        /// Helper method to be used with evaluate 
        /// to look up the value of variables
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public double Lookup(string name)
        {
            try
            {
                double num = (double)GetCellValue(name);
                return num;

            }
            catch(Exception)
            {
                throw new ArgumentException();
            }  
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
                value = content;
            }
            /// <summary>
            /// Returns the Content of the Cell Obeject
            /// </summary>
            /// <returns></returns>
            public object getContent()
            {
                return content;
            }

            /// <summary>
            /// Sets the Value of the Cell Object
            /// </summary>
            /// <param name="newVal"></param>
            public void setValue(object newVal)
            {
                value = newVal;
            }

            /// <summary>
            /// Returns the Value of the Cell object
            /// </summary>
            /// <returns></returns>
            public object getValue()
            {
                return value;
            }
        }
    }
}


