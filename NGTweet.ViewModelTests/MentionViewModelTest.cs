using System;
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight.Threading;

using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using NGTweet.ViewModels;
using NGTweet.ViewModels.INGTweetAuthenticationService;

namespace NGTweet.ViewModelTests
{
    [TestClass]
    [Tag("MentionsViewModelTests")]
    public class MentionViewModelTest
    {
        private MentionsViewModel _viewModel;

        private INGTweetAuthenticationService _mockAuthenticationService;
        private IApplicationSettingsProvider _mockApplicationSettingsProvider;

        [TestInitialize]
        public void Setup()
        {
            DispatcherHelper.Initialize();

            _mockAuthenticationService = new Mock<INGTweetAuthenticationService>().Object;
            _mockApplicationSettingsProvider = new Mock<IApplicationSettingsProvider>().Object;

            _viewModel = new MentionsViewModel(_mockAuthenticationService, _mockApplicationSettingsProvider);
        }

        [TestMethod]
        public void ConstructorTest()
        {
            Assert.IsNotNull(_viewModel);
        }

        [TestMethod]
        public void LoadTweetsFromTimeline_ShouldSetBusyIndicatorAndMakeAServiceCall()
        {
            OAuthAccessToken accessToken = new OAuthAccessToken();

            Mock.Get(_mockApplicationSettingsProvider)
                .SetupGet(m => m["accessToken"])
                .Returns(accessToken);

            Mock.Get(_mockAuthenticationService)
                .Setup(m => m.BeginGetTweetsMentioningMe(accessToken, It.IsAny<AsyncCallback>(), null));

            _viewModel.GetTweetsMentioningMe();

            Assert.IsTrue(_viewModel.IsBusy);

            Mock.Get(_mockAuthenticationService).VerifyAll();
        }

        [TestMethod]
        public void GetTweetsCompleted_ShouldUpdateCollectionTest()
        {
            Assert.AreEqual(0, _viewModel.TweeterStatusViewModels.Count);

            GetTweetResponse response = new GetTweetResponse
            {
                TweeterStatuses =
                    new ObservableCollection<NGTweeterStatus>
                            {
                                new NGTweeterStatus
                                    {
                                        CreatedDate = new DateTime(2011, 06, 10),
                                        Tweet = "Hello",
                                        User =
                                            new NGTweeterUser
                                                {
                                                    Id = 101,
                                                    Name = "Nilesh",
                                                    ScreenName = "Nileshgule",
                                                    ProfileImageUrl = "http://twitter.com/profileImages/nileshgule.jpg"
                                                }
                                    }
                            }
            };

            Mock.Get(_mockAuthenticationService)
                .Setup(m => m.EndGetTweetsMentioningMe(It.IsAny<IAsyncResult>()))
                .Returns(response);

            _viewModel.GetTweetsMentioningMeCompleted(new Mock<IAsyncResult>().Object);

            Assert.IsFalse(_viewModel.IsBusy);

            Assert.AreEqual(1, _viewModel.TweeterStatusViewModels.Count);

            Mock.Get(_mockAuthenticationService).VerifyAll();
        }
    }
}