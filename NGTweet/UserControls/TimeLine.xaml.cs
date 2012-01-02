using NGTweet.ViewModels;

namespace NGTweet.UserControls
{
    public partial class TimeLine
    {
        public TimeLine()
        {
            InitializeComponent();
            Loaded += TimeLine_Loaded;
        }

        private void TimeLine_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            TimeLineViewModel viewModel = DataContext as TimeLineViewModel;

            if (viewModel != null)
            {
                viewModel.LoadTweetsFromTimeLine();
            }
        }
    }
}