using NGTweet.ViewModels;

namespace NGTweet.UserControls
{
    public partial class Mentions
    {
        public Mentions()
        {
            InitializeComponent();
            Loaded += Mentions_Loaded;
        }

        private void Mentions_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            MentionsViewModel viewModel = DataContext as MentionsViewModel;

            if (viewModel != null)
            {
                viewModel.GetTweetsMentioningMe();
            }
        }
    }
}