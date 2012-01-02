using System;
using System.Windows.Input;
using System.Windows.Threading;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using NGTweet.ViewModels.INGTweetAuthenticationService;

namespace NGTweet.ViewModels
{
    public class TweetViewModel : ViewModelBase
    {
        protected readonly NGTweeterStatus TweeterStatus;

        private readonly DispatcherTimer _refreshTimer;

        public TweetViewModel(NGTweeterStatus tweeterStatus)
        {
            TweeterStatus = tweeterStatus;

            RetweetCommand = new RelayCommand(OnRetweet);

            _refreshTimer = new DispatcherTimer { Interval = TimeSpan.FromMinutes(1) };

            _refreshTimer.Tick += (o, args) => RaisePropertyChanged("CreatedDate");

            _refreshTimer.Start();
        }

        public string ProfileImageSource
        {
            get
            {
                return TweeterStatus.User.ProfileImageUrl;
            }
        }

        public virtual string ScreenName
        {
            get
            {
                return TweeterStatus.User.ScreenName;
            }
        }

        public string Tweet
        {
            get
            {
                return TweeterStatus.Tweet;
            }
        }

        public DateTime CreatedDate
        {
            get
            {
                return TweeterStatus.CreatedDate;
            }
        }

        public long Id
        {
            get
            {
                return TweeterStatus.Id;
            }
        }

        public bool IsRetweet
        {
            get
            {
                return TweeterStatus.RetweetedStatus != null;
            }
        }

        public ICommand RetweetCommand { get; set; }

        private void OnRetweet()
        {
            Messenger.Default.Send(TweeterStatus, "Retweet");
        }
    }
}