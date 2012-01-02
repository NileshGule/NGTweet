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
    [Tag("TimeLineViewModelTests")]
    public class TimeLineViewModelTest
    {
        private TimeLineViewModel _viewModel;

        private INGTweetAuthenticationService _mockAuthenticationService;
        private IApplicationSettingsProvider _mockApplicationSettingsProvider;

        [TestInitialize]
        public void Setup()
        {
            DispatcherHelper.Initialize();

            _mockAuthenticationService = new Mock<INGTweetAuthenticationService>().Object;

            _mockApplicationSettingsProvider = new Mock<IApplicationSettingsProvider>().Object;

            _viewModel = new TimeLineViewModel(_mockAuthenticationService, _mockApplicationSettingsProvider);
        }

        [TestMethod]
        public void ConstructorTest()
        {
            Assert.IsNotNull(_viewModel);
        }

        [TestMethod]
        public void AddNewTweets_ShouldAddTweetsToListTest()
        {
            ObservableCollection<NGTweeterStatus> tweeterStatuses = new ObservableCollection<NGTweeterStatus>
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
                };

            _viewModel.AddNewTweets(tweeterStatuses);

            Assert.AreEqual(1, _viewModel.TweeterStatusViewModels.Count);
        }

        [TestMethod]
        public void LoadTweetsFromTimeline_WhenThereAreExistingTweets_ShouldSetBusyIndicator_AndMakeAServiceCall_UsingLastTweetIdTest()
        {
            ObservableCollection<NGTweeterStatus> tweeterStatuses = new ObservableCollection<NGTweeterStatus>
                {
                    new NGTweeterStatus
                        {
                            Id = 1001,
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
                };

            _viewModel.AddNewTweets(tweeterStatuses);

            Assert.AreEqual(1, _viewModel.TweeterStatusViewModels.Count);

            OAuthAccessToken accessToken = new OAuthAccessToken();

            Mock.Get(_mockApplicationSettingsProvider)
                .SetupGet(m => m["accessToken"])
                .Returns(accessToken);

            Mock.Get(_mockAuthenticationService).Setup(
                m => m.BeginGetTweetsSince(
                    It.Is<GetTweetsRequest>(request => request.AccessToken == accessToken && request.LastTweetId == 1001),
                    It.IsAny<AsyncCallback>(),
                    null));

            _viewModel.LoadTweetsFromTimeLine();

            Assert.IsTrue(_viewModel.IsBusy);

            Mock.Get(_mockAuthenticationService).VerifyAll();
            Mock.Get(_mockApplicationSettingsProvider).VerifyAll();
        }

        [TestMethod]
        public void LoadTweetsFromTimeline__WhenThereAreNoExistingTweets_ShouldSetBusyIndicator_AndMakeAServiceCallTest()
        {
            OAuthAccessToken accessToken = new OAuthAccessToken();

            Mock.Get(_mockApplicationSettingsProvider)
                .SetupGet(m => m["accessToken"])
                .Returns(accessToken);

            Mock.Get(_mockAuthenticationService)
                .Setup(m => m.BeginGetTweets(accessToken, It.IsAny<AsyncCallback>(), null));

            _viewModel.LoadTweetsFromTimeLine();

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
                .Setup(m => m.EndGetTweets(It.IsAny<IAsyncResult>()))
                .Returns(response);

            _viewModel.GetTweetsCompleted(new Mock<IAsyncResult>().Object);

            Assert.IsFalse(_viewModel.IsBusy);

            Assert.AreEqual(1, _viewModel.TweeterStatusViewModels.Count);

            Mock.Get(_mockAuthenticationService).VerifyAll();
        }

        [TestMethod]
        public void GetTweetsCompletedSince_ShouldUpdateCollectionTest()
        {
            ObservableCollection<NGTweeterStatus> tweeterStatuses = new ObservableCollection<NGTweeterStatus>
                {
                    new NGTweeterStatus
                        {
                            Id = 1001,
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
                };

            _viewModel.AddNewTweets(tweeterStatuses);

            Assert.AreEqual(1, _viewModel.TweeterStatusViewModels.Count);

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
                .Setup(m => m.EndGetTweetsSince(It.IsAny<IAsyncResult>()))
                .Returns(response);

            _viewModel.GetTweetsSinceCompleted(new Mock<IAsyncResult>().Object);

            Assert.IsFalse(_viewModel.IsBusy);

            Assert.AreEqual(2, _viewModel.TweeterStatusViewModels.Count);

            Mock.Get(_mockAuthenticationService).VerifyAll();
        }
    }
}