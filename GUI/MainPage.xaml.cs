using SpreadsheetUtilities;
using SS;
using System.Diagnostics;

namespace GUI
{
    public partial class MainPage : ContentPage
    {

        /// <summary>
        ///    Definition of what information (method signature) must be sent
        ///    by the Entry when it is modified.
        /// </summary>
        /// <param name="col"> col (char) in grid, e.g., A5 </param>
        /// <param name="row"> row (int) in grid,  e.g., A5 </param>
        public delegate void ActionOnCompleted(char col, int row, string text, bool refocus, bool textChanged);

        /// <summary>
        ///     Definition of what information must be sent by the Entry
        ///     When it is focused
        /// </summary>
        /// <param name="col"> col char in grid ex. A5 </param>
        /// <param name="row">row int in grid eg A5 </param>
        public delegate void ActionOnFocus(char col, int row);

        private static AbstractSpreadsheet spreadsheet;
        private static Dictionary<string, MyEntry> cells;

        public MainPage()
        {
            InitializeComponent();

            spreadsheet = new Spreadsheet();

            //Loop to add Top Labels
            char[] letters = " ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToArray();
            foreach (char c in letters)
            {
                TopLabels.Add(
                new Border
                {
                    StrokeThickness = 1,
                    HeightRequest = 20,
                    WidthRequest = 75,
                    HorizontalOptions = LayoutOptions.Center,
                    Content =
                        new Label
                        {
                            Text = $"{c}",
                            BackgroundColor = Color.FromRgb(200, 200, 250),
                            HorizontalTextAlignment = TextAlignment.Center
                        }
                }
                );
            }

            //Loop to add Left Labels
            for (int i = 1; i < 11; i++)
            {
                LeftLabels.Add(
                new Border
                {
                    Stroke = Color.FromRgb(0, 0, 0),
                    StrokeThickness = 1,
                    HeightRequest = 30,
                    WidthRequest = 75,
                    HorizontalOptions = LayoutOptions.Center,
                    Content =
                        new Label
                        {
                            Text = $"{i}",
                            BackgroundColor = Color.FromRgb(200, 200, 250),
                            HorizontalTextAlignment = TextAlignment.Center
                        }
                }
                );
            }

            cells = new Dictionary<string, MyEntry>();
            letters = letters[1..];
            //Loops to add Entry Boxes
            for (int i = 1; i < 11; i++)
            {
                var horiz = new HorizontalStackLayout();
                Grid.Add(horiz);

                foreach (char c in letters)
                {
                    MyEntry entry = new MyEntry(c, i, handleCellChanged, handleCellFocused);
                    cells.Add(c + "" + i, entry);
                    horiz.Add(
                    new Border
                    {
                        Stroke = Color.FromRgb(250, 250, 250),
                        StrokeThickness = 1,
                        HeightRequest = 30,
                        WidthRequest = 75,
                        HorizontalOptions = LayoutOptions.Center,
                        Content = entry                        
                    }
                    );  
                }
            }
            cells["A1"].Focus();
        }



        /// <summary>
        ///   This method will be called by the individual Entry elements when Enter
        ///   is pressed in them.
        ///   
        ///   The idea is to move to the next cell in the list.
        /// </summary>
        /// <param name="col"> e.g., The 'A' in A5 </param>
        /// <param name="row"> e.g., The  5  in A5 </param>
        async void handleCellChanged(char col, int row, string text, bool refocus, bool textChanged)
        {
            string cellName = col + "" + row;

            MyEntry entry = cells[cellName];
           
            IList<string> cellsToUpdate = new List<string>();

            //Check if we need to modify cell (used so we dont turn a formula into a number)
            if (textChanged)
            {
                try
                {
                    cellsToUpdate = spreadsheet.SetContentsOfCell(cellName, text);
                }
                catch (FormulaFormatException e)
                {
                    Debug.WriteLine(e.Message);
                    await DisplayAlert("FORMULA FORMAT ERROR", "Please re-enter formula and try again", "OK");
                }

                catch (CircularException e)
                {
                    Debug.WriteLine(e.Message);
                    await DisplayAlert("CIRCULAR DEPENDENCY ERROR", "Please re-enter formula and try again, removing all circular dependencies", "OK");
                }

                entry.Text = spreadsheet.GetCellValue(cellName).ToString();
                selectedCell.Text = spreadsheet.GetCellContents(cellName).ToString();
                
            }

            entry.Text = spreadsheet.GetCellValue(cellName).ToString();
                selectedCell.Text = spreadsheet.GetCellContents(cellName).ToString();
            

            //if needed move focus to next cell under if no such cell exists unfocus 
            if (refocus)
            {
                try
                {
                    cells[col + "" + (row + 1)].Focus();
                }
                catch (Exception)
                {
                    cells[cellName].Unfocus();
                }
            


               
            //Update all cells that need to be updated
            foreach (string name in cellsToUpdate)
            {
                cells[name].Text = spreadsheet.GetCellValue(name).ToString();
            }

        }

            //Check if we need to modify cell (used so we dont turn a formula into a number)
            if (textChanged)
            {
                cellsToUpdate = spreadsheet.SetContentsOfCell(cellName, text);

                entry.Text = spreadsheet.GetCellValue(cellName).ToString();
                selectedCell.Text = spreadsheet.GetCellContents(cellName).ToString();
            }

            //if needed move focus to next cell under if no such cell exists unfocus 
            if(refocus)
            {
                try
                {
                    cells[col + "" + (row + 1)].Focus();
                }
                catch (Exception)
                {
                    cells[cellName].Unfocus();
                }
            }
 
            //Update all cells that need to be updated
            foreach(string name in cellsToUpdate)
            {
                cells[name].Text = spreadsheet.GetCellValue(name).ToString();
            }
            
        }

        /// <summary>
        ///     This method will be called by the individual Entry when
        ///     they are Focused
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        void handleCellFocused(char col, int row)
        {
            string cellName = col + "" + row;

            selectedCell.Text = cellName;
            selectedCellContent.Text = spreadsheet.GetCellContents(cellName).ToString();
        }

        void FileMenuNew(object sender, System.EventArgs e)
        {

            foreach (MyEntry entry in cells.Values)
            {
                entry.ClearAndUnfocus();
            }
            spreadsheet = new Spreadsheet();
            selectedCell.Text = "";

        }

        public static async void FileMenuOpenAsync(object sender, System.EventArgs e)
        {
            FileResult? fileResult = await FilePicker.Default.PickAsync();
            if(fileResult is not null)
            {
                foreach (MyEntry entry in cells.Values)
                {
                    entry.ClearAndUnfocus();
                }

                spreadsheet = new Spreadsheet(fileResult.FullPath, s=>true, s=>s.ToUpper(), "default");


                foreach(string cell in spreadsheet.GetNamesOfAllNonemptyCells())
                {
                    MyEntry entry = cells[cell];
                    entry.Text = spreadsheet.GetCellValue(cell).ToString();
                }

            }

        }

        public async void FileMenuSaveAsync(object sender, System.EventArgs e)
        {
            string name = await DisplayPromptAsync("File Name", "Please enter name of spreadsheet. DO NOT INCLUDE EXTENSIONS OR ANY UNUSUAL CHARACTERS");
            string path = await DisplayPromptAsync("Save File", "Please enter full path of folder you would like to save to");
            string filePath = path + "\\" + name + ".sprd";


            try
            {
                spreadsheet.Save(filePath);
                await DisplayAlert("FILE SAVE SUCESSFUL", "Feel free to close this alert box and continue", "OK");

            }
            catch
            {
                await DisplayAlert("FILE SAVE FAIL", "Please re-enter file path and try again", "OK");
            }

        }
    }
}