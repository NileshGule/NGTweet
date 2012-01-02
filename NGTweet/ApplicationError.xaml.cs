using System.Windows;

namespace NGTweet
{
    public partial class ApplicationError
    {
        public ApplicationError()
        {
            InitializeComponent();
            DataContext = ErrorMessage;
        }

        public string ErrorMessage { get; set; }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}