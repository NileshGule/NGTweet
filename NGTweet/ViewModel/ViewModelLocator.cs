using NGTweet.ViewModels;
using NGTweet.ViewModels.INGTweetAuthenticationService;
using NGTweet.ViewServices;

namespace NGTweet.ViewModel
{
    public class ViewModelLocator
    {
        private static MainViewModel mainViewModel;

        private static TimeLineViewModel timeLineViewModel;

        private static MentionsViewModel mentionsViewModel;

        private static TweetActionViewModel tweetActionViewModel;

        public ViewModelLocator()
        {
            NGTweetAuthenticationServiceClient authenticationService = new NGTweetAuthenticationServiceClient();

            NGApplicationSettingsProvider settingsProvider = new NGApplicationSettingsProvider();

            mainViewModel = new MainViewModel(authenticationService, settingsProvider);

            timeLineViewModel = new TimeLineViewModel(authenticationService, settingsProvider);

            mentionsViewModel = new MentionsViewModel(authenticationService, settingsProvider);

            tweetActionViewModel = new TweetActionViewModel(authenticationService, settingsProvider);
        }

        public MainViewModel MainViewModel
        {
            get
            {
                return mainViewModel;
            }
        }

        public TimeLineViewModel TimeLineViewModel
        {
            get
            {
                return timeLineViewModel;
            }
        }

        public MentionsViewModel MentionsViewModel
        {
            get
            {
                return mentionsViewModel;
            }
        }

        public TweetActionViewModel TweetActionViewModel
        {
            get
            {
                return tweetActionViewModel;
            }
        }
    }
}