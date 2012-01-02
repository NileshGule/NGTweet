using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;

using NGTweet.ViewModels.INGTweetAuthenticationService;

namespace NGTweet.ViewModels
{
    public class TweetActionViewModel : ViewModelBase, IDataErrorInfo
    {
        private readonly INGTweetAuthenticationService.INGTweetAuthenticationService _authenticationService;

        private readonly IApplicationSettingsProvider _applicationSettingsProvider;

        [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore",
            Justification = "Reviewed. Suppression is OK here.")]
        private const int MAX_CHARACTERS = 140;

        private bool _canPerformTweetAction;

        private string _tweetText;

        private bool _isBusy;

        public TweetActionViewModel(INGTweetAuthenticationService.INGTweetAuthenticationService authenticationService, IApplicationSettingsProvider applicationSettingsProvider)
        {
            _authenticationService = authenticationService;

            _applicationSettingsProvider = applicationSettingsProvider;

            TweetText = string.Empty;

            TweetActionCommand = new RelayCommand(OnTweetActionCommand);

            Messenger.Default.Register<NGTweeterStatus>(this, "Retweet", ReTweetExistingTweet);
        }

        internal void ReTweetExistingTweet(NGTweeterStatus tweet)
        {
            IsBusy = true;

            RetweetRequest retweetRequest = new RetweetRequest
                {
                    AccessToken = (OAuthAccessToken)_applicationSettingsProvider["accessToken"],
                    TweetId = tweet.Id
                };

            _authenticationService.BeginRetweet(retweetRequest, RetweetCallback, null);
        }

        private void RetweetCallback(IAsyncResult ar)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(
                () =>
                {
                    IsBusy = false;

                    RetweetResponse sendTweetResponse = _authenticationService.EndRetweet(ar);

                    TweetText = string.Empty;
                });
        }

        public bool CanPerformTweetAction
        {
            get
            {
                return _canPerformTweetAction;
            }

            set
            {
                if (_canPerformTweetAction != value)
                {
                    _canPerformTweetAction = value;

                    RaisePropertyChanged("CanPerformTweetAction");
                }
            }
        }

        public string TweetText
        {
            get
            {
                return _tweetText;
            }

            set
            {
                if (_tweetText != value)
                {
                    _tweetText = value;

                    CanPerformTweetAction = HasValidText();

                    RaisePropertyChanged("TweetText");
                }
            }
        }

        public ICommand TweetActionCommand { get; set; }

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }

            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;

                    RaisePropertyChanged("IsBusy");
                }
            }
        }

        public string Error
        {
            get
            {
                return null;
            }
        }

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "TweetText":
                        if (TweetText.Length >= MAX_CHARACTERS)
                        {
                            return "You have exceeded maximum character limit of 140 characters.";
                        }
                        break;
                    default:
                        return string.Empty;
                }

                return string.Empty;
            }
        }

        internal void OnTweetActionCommand()
        {
            IsBusy = true;

            SendTweetRequest sendTweetRequest = new SendTweetRequest
                {
                    AccessToken = (OAuthAccessToken)_applicationSettingsProvider["accessToken"],
                    Status = TweetText
                };

            _authenticationService.BeginSendTweet(sendTweetRequest, SendTweetCallback, null);
        }

        internal void SendTweetCallback(IAsyncResult ar)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(
                () =>
                {
                    IsBusy = false;

                    SendTweetResponse sendTweetResponse = _authenticationService.EndSendTweet(ar);

                    TweetText = string.Empty;

                    Messenger.Default.Send(sendTweetResponse.TweeterStatus, "SendTweetSuccess");
                });
        }

        private bool HasValidText()
        {
            return !string.IsNullOrEmpty(_tweetText)
                && !string.IsNullOrWhiteSpace(_tweetText)
                && _tweetText.Length <= MAX_CHARACTERS;
        }
    }
}