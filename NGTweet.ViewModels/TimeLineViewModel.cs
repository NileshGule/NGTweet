using System;
using System.Linq;
using System.Windows.Threading;

using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;

using NGTweet.ViewModels.INGTweetAuthenticationService;

namespace NGTweet.ViewModels
{
    public class TimeLineViewModel : TimeLineViewModelBase
    {
        public TimeLineViewModel(INGTweetAuthenticationService.INGTweetAuthenticationService authenticationService, IApplicationSettingsProvider applicationSettingsProvider)
            : base(authenticationService, applicationSettingsProvider)
        {
            const int REFRESH_INTERVAL_IN_MINUTES = 2;

            DispatcherTimer refreshTimer = new DispatcherTimer { Interval = TimeSpan.FromMinutes(REFRESH_INTERVAL_IN_MINUTES) };

            refreshTimer.Tick += RefreshTimerTick;

            refreshTimer.Start();

            Messenger.Default.Register<NGTweeterStatus>(this, "SendTweetSuccess", AddNewTweet);
        }

        public void LoadTweetsFromTimeLine()
        {
            OAuthAccessToken accessToken = (OAuthAccessToken)_applicationSettingsProvider[ACCESS_TOKEN];

            IsBusy = true;

            if (HasExistingTweets())
            {
                GetTweetsSinceLastTweet(accessToken);
            }
            else
            {
                _authenticationService.BeginGetTweets(accessToken, GetTweetsCompleted, null);
            }
        }

        internal void GetTweetsCompleted(IAsyncResult asyncResult)
        {
            GetTweetResponse response = _authenticationService.EndGetTweets(asyncResult);

            DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    IsBusy = false;

                    if (response.TweeterStatuses != null)
                    {
                        AddNewTweets(response.TweeterStatuses);
                    }
                });
        }

        internal void GetTweetsSinceCompleted(IAsyncResult asyncResult)
        {
            GetTweetResponse response = _authenticationService.EndGetTweetsSince(asyncResult);

            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                IsBusy = false;

                if (response.TweeterStatuses != null)
                {
                    AddNewTweets(response.TweeterStatuses);
                }
            });
        }

        private void AddNewTweet(NGTweeterStatus newTweet)
        {
            if (newTweet.RetweetedStatus == null)
            {
                _tweeterStatusViewModels.Insert(0, new TweetViewModel(newTweet));
            }
            else
            {
                _tweeterStatusViewModels.Insert(0, new RetweetViewModel(newTweet));
            }
        }

        private void GetTweetsSinceLastTweet(OAuthAccessToken accessToken)
        {
            GetTweetsRequest request = new GetTweetsRequest
                {
                    LastTweetId = TweeterStatusViewModels.First().Id,
                    AccessToken = accessToken
                };

            _authenticationService.BeginGetTweetsSince(request, GetTweetsSinceCompleted, null);
        }

        private bool HasExistingTweets()
        {
            return TweeterStatusViewModels.Count > 0;
        }

        private void RefreshTimerTick(object sender, EventArgs e)
        {
            LoadTweetsFromTimeLine();
        }
    }
}