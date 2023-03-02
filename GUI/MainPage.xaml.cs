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
        public delegate void ActionOnCompleted(char col, int row, string text);

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
                    Stroke = Color.FromRgb(0, 0, 0),
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
            for (int i = 1; i < 10; i++)
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
            for (int i = 1; i < 10; i++)
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
                        Stroke = Color.FromRgb(0, 0, 0),
                        StrokeThickness = 1,
                        HeightRequest = 30,
                        WidthRequest = 75,
                        HorizontalOptions = LayoutOptions.Center,
                        Content = entry
                    }
                    );

                    
                }
            }
        }



        /// <summary>
        ///   This method will be called by the individual Entry elements when Enter
        ///   is pressed in them.
        ///   
        ///   The idea is to move to the next cell in the list.
        /// </summary>
        /// <param name="col"> e.g., The 'A' in A5 </param>
        /// <param name="row"> e.g., The  5  in A5 </param>
        void handleCellChanged(char col, int row, string text)
        {
            string cellName = col + "" + row;
            MyEntry entry = cells[cellName];

            spreadsheet.SetContentsOfCell(cellName, text);

            entry.Text = spreadsheet.GetCellValue(cellName).ToString();

            cells[col + "" + (row + 1)].Focus();
        }

        /// <summary>
        ///     This method will be called by the individual Entry when
        ///     they are Focused
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        void handleCellFocused(char col, int row)
        {
            Debug.WriteLine(col + "" + row + "IS FOCUSED");
        }


        public static void FileMenuNew(object sender, System.EventArgs e)
        {
            foreach(MyEntry entry in cells.Values)
            {
                entry.ClearAndUnfocus();
            }
            spreadsheet = new Spreadsheet();

        }

        public static void FileMenuOpenAsync(object sender, System.EventArgs e)
        {

        }
    }
}