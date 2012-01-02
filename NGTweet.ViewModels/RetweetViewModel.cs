using NGTweet.ViewModels.INGTweetAuthenticationService;

namespace NGTweet.ViewModels
{
    public class RetweetViewModel : TweetViewModel
    {
        public RetweetViewModel(NGTweeterStatus tweeterStatus)
            : base(tweeterStatus)
        {
        }

        public string OriginalProfileImageSource
        {
            get
            {
                return TweeterStatus.RetweetedStatus.User.ProfileImageUrl;
            }
        }

        public override string ScreenName
        {
            get
            {
                return string.Format(
                               "{0}, (RT by {1})",
                               TweeterStatus.RetweetedStatus.User.ScreenName,
                               TweeterStatus.User.ScreenName);
            }
        }
    }
}