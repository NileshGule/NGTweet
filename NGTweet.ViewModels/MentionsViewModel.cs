using System;

using GalaSoft.MvvmLight.Threading;

using NGTweet.ViewModels.INGTweetAuthenticationService;

namespace NGTweet.ViewModels
{
    public class MentionsViewModel : TimeLineViewModelBase
    {
        public MentionsViewModel(INGTweetAuthenticationService.INGTweetAuthenticationService authenticationService, IApplicationSettingsProvider applicationSettingsProvider)
            : base(authenticationService, applicationSettingsProvider)
        {
        }

        public void GetTweetsMentioningMe()
        {
            IsBusy = true;

            OAuthAccessToken accessToken = (OAuthAccessToken)_applicationSettingsProvider[ACCESS_TOKEN];

            _authenticationService.BeginGetTweetsMentioningMe(accessToken, GetTweetsMentioningMeCompleted, null);
        }

        internal void GetTweetsMentioningMeCompleted(IAsyncResult asyncResult)
        {
            GetTweetResponse response = _authenticationService.EndGetTweetsMentioningMe(asyncResult);

            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                IsBusy = false;

                if (response.TweeterStatuses != null)
                {
                    AddNewTweets(response.TweeterStatuses);
                }
            });
        }
    }
}