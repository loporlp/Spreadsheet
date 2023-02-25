namespace GUI
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();

            char[] letters = "ABCDEFGHIJKLMNOPQRSTUVWXWY".ToArray();
            foreach(char c in letters)
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
            
        }


        public static void FileMenuNew(object sender, System.EventArgs e)
        {

        }

        public static void FileMenuOpenAsync(object sender, System.EventArgs e)
        {

        }
    }
}