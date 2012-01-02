using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;

using NGTweet.ViewModels.INGTweetAuthenticationService;

namespace NGTweet.ViewModels
{
    public delegate void NavigateToAuthorizationPage(object sender, NavigateToAuthorizationPageArgs args);

    public class MainViewModel : ViewModelBase
    {
        [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore",
            Justification = "Reviewed. Suppression is OK here.")]
        private const string ACCESS_TOKEN = "accessToken";

        [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore",
            Justification = "Reviewed. Suppression is OK here.")]
        private const string REQUEST_TOKEN = "requestToken";

        [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore",
            Justification = "Reviewed. Suppression is OK here.")]
        private const string AUTHORIZATION_COMPLETED = "AuthorizationCompleted";

        private readonly INGTweetAuthenticationService.INGTweetAuthenticationService _authenticationService;

        private readonly IApplicationSettingsProvider _applicationSettingsProvider;

        private bool _isVerificationDetailsVisible;

        private ObservableCollection<NGTweeterStatus> _tweeterStatuses;

        private bool _isAuthenticUser;

        private string _verificationPin;

        public MainViewModel(
            INGTweetAuthenticationService.INGTweetAuthenticationService authenticationService,
            IApplicationSettingsProvider applicationSettingsProvider)
        {
            _authenticationService = authenticationService;

            _applicationSettingsProvider = applicationSettingsProvider;

            AuthorizeAppCommand = new RelayCommand(AuthorizeWithPin);
        }

        public ICommand AuthorizeAppCommand { get; set; }

        public bool IsVerificationDetailsVisible
        {
            get
            {
                return _isVerificationDetailsVisible;
            }

            set
            {
                if (_isVerificationDetailsVisible != value)
                {
                    _isVerificationDetailsVisible = value;

                    DispatcherHelper.CheckBeginInvokeOnUI(() => RaisePropertyChanged("IsVerificationDetailsVisible"));
                }
            }
        }

        public ObservableCollection<NGTweeterStatus> TweeterStatuses
        {
            get
            {
                return _tweeterStatuses;
            }

            set
            {
                _tweeterStatuses = value;

                //DispatcherHelper.UIDispatcher.BeginInvoke(() => RaisePropertyChanged("TweeterStatuses"));
                RaisePropertyChanged("TweeterStatuses");
            }
        }

        private ObservableCollection<TweetViewModel> _tweeterStatusViewModels;

        public ObservableCollection<TweetViewModel> TweeterStatusViewModels
        {
            get
            {
                return _tweeterStatusViewModels;
            }

            set
            {
                _tweeterStatusViewModels = value;

                RaisePropertyChanged("TweeterStatusViewModels");
            }
        }

        public bool IsAuthenticUser
        {
            get
            {
                return _isAuthenticUser;
            }

            set
            {
                if (_isAuthenticUser != value)
                {
                    _isAuthenticUser = value;

                    DispatcherHelper.UIDispatcher.BeginInvoke(() => RaisePropertyChanged("IsAuthenticUser"));
                }
            }
        }

        public string VerificationPIN
        {
            get
            {
                return _verificationPin;
            }

            set
            {
                if (_verificationPin != value)
                {
                    _verificationPin = value;

                    RaisePropertyChanged("VerificationPIN");
                }
            }
        }

        public void Login()
        {
            if (UserHasLoginHistory())
            {
                GetTweetsForUser();
            }
            else
            {
                IsVerificationDetailsVisible = true;

                _authenticationService.BeginGetRequestToken(GetRequestTokenCompleted, null);
            }
        }

        internal void GetRequestTokenCompleted(IAsyncResult result)
        {
            GetRequestTokenResponse requestTokenResponse = _authenticationService.EndGetRequestToken(result);

            _applicationSettingsProvider[REQUEST_TOKEN] = requestTokenResponse.RequestToken;

            IsVerificationDetailsVisible = true;

            Messenger.Default.Send(requestTokenResponse.AuthorizationUri);
        }

        internal bool UserHasLoginHistory()
        {
            return _applicationSettingsProvider.Contains(ACCESS_TOKEN);
        }

        internal void GetTweetsForUser()
        {
            IsVerificationDetailsVisible = false;

            IsAuthenticUser = true;
        }

        internal void GetTweetsCompleted(IAsyncResult result)
        {
            //GetDataResponse response = _authenticationService.EndGetTweets(result);

            //TweeterStatuses = response.TweeterStatuses;

            //ObservableCollection<TweeterStatusViewModel> statuses = new ObservableCollection<TweeterStatusViewModel>();

            //foreach (var status in TweeterStatuses)
            //{
            //    statuses.Add(new TweeterStatusViewModel(status));
            //}

            //TweeterStatusViewModels = statuses;

            //IsAuthenticUser = true;
        }

        internal void AuthorizeWithPin()
        {
            LoginRequest loginRequest = new LoginRequest { Pin = VerificationPIN, RequestToken = (OAuthRequestToken)_applicationSettingsProvider[REQUEST_TOKEN] };

            _authenticationService.BeginLogin(loginRequest, AuthorizeWithPinCompleted, null);
        }

        internal void AuthorizeWithPinCompleted(IAsyncResult asyncResult)
        {
            const string SUCCESS_MESSAGE = "You have successfully authorized NGTweet to access Twitter.";

            LoginResponse loginResponse = _authenticationService.EndLogin(asyncResult);

            _applicationSettingsProvider[ACCESS_TOKEN] = loginResponse.OAuthAccessToken;

            DialogMessage dialogMessage = new DialogMessage(SUCCESS_MESSAGE, null) { Caption = "Success" };

            Messenger.Default.Send(dialogMessage, AUTHORIZATION_COMPLETED);

            IsVerificationDetailsVisible = false;
        }
    }
}