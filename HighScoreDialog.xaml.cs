using System.Windows;

namespace SnakeGameWPF
{
    /// <summary>
    /// Interaction logic for HighScoreDialog.xaml
    /// </summary>
    public partial class HighScoreDialog : Window
    {
        public HighScoreDialog()
        {
            InitializeComponent();
        }

        public string UserName
        {
            get { return ResponseTextBox.Text; }
            set { ResponseTextBox.Text = value; }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
