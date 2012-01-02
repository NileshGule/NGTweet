using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Threading;

using NGTweet.ViewModels.INGTweetAuthenticationService;

namespace NGTweet.ViewModels
{
    public abstract class TimeLineViewModelBase : ViewModelBase
    {
        [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore",
            Justification = "Reviewed. Suppression is OK here.")]
        protected const string ACCESS_TOKEN = "accessToken";

        protected INGTweetAuthenticationService.INGTweetAuthenticationService _authenticationService;

        protected IApplicationSettingsProvider _applicationSettingsProvider;

        protected ObservableCollection<TweetViewModel> _tweeterStatusViewModels;

        private bool _isBusy;

        protected TimeLineViewModelBase(INGTweetAuthenticationService.INGTweetAuthenticationService authenticationService, IApplicationSettingsProvider applicationSettingsProvider)
        {
            _authenticationService = authenticationService;

            _applicationSettingsProvider = applicationSettingsProvider;

            _tweeterStatusViewModels = new ObservableCollection<TweetViewModel>();
        }

        public ObservableCollection<TweetViewModel> TweeterStatusViewModels
        {
            get
            {
                return _tweeterStatusViewModels;
            }
        }

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

        public void AddNewTweets(ObservableCollection<NGTweeterStatus> tweeterStatuses)
        {
            tweeterStatuses.Reverse();
            DispatcherHelper.CheckBeginInvokeOnUI(
                () => tweeterStatuses.OrderBy(ts => ts.CreatedDate).ToList().ForEach(
                    t =>
                    {
                        if (t.RetweetedStatus == null)
                            _tweeterStatusViewModels.Insert(0, new TweetViewModel(t));
                        else
                        {
                            _tweeterStatusViewModels.Insert(0, new RetweetViewModel(t));
                        }
                    }));
        }
    }
}