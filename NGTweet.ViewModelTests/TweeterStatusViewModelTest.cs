using System;

using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NGTweet.ViewModels;
using NGTweet.ViewModels.INGTweetAuthenticationService;

namespace NGTweet.ViewModelTests
{
    [TestClass]
    [Tag("TweeterStatusViewModelTest")]
    public class TweeterStatusViewModelTest
    {
        private TweetViewModel _viewModel;

        [TestInitialize]
        public void Setup()
        {
            _viewModel = new TweetViewModel(new NGTweeterStatus());
        }

        [TestMethod]
        public void ConstructorShouldInitializeCommandTest()
        {
            Assert.IsNotNull(_viewModel);
            Assert.IsNotNull(_viewModel.RetweetCommand);
        }

        [TestMethod]
        public void IsRetweet_ShoulBeTrue_WhenRetweetStatusIsNotNullTest()
        {
            NGTweeterStatus tweeterStatus = new NGTweeterStatus
                {
                    CreatedDate = DateTime.Today,
                    Id = 101,
                    RetweetedStatus =
                        new NGTweeterStatus
                            {
                                CreatedDate = DateTime.Today,
                                Id = 102,
                                Tweet = "Original Tweet",
                                User =
                                    new NGTweeterUser
                                        {
                                            Id = 5,
                                            Name = "Nilesh",
                                            ProfileImageUrl = "Nilesh.jpg",
                                            ScreenName = "NileshGule"
                                        }
                            },
                    User = new NGTweeterUser
                        {
                            Id = 6,
                            Name = "TestName",
                            ProfileImageUrl = "Test.jpg",
                            ScreenName = "TestScreenName"
                        }
                };

            _viewModel = new TweetViewModel(tweeterStatus);
            Assert.IsTrue(_viewModel.IsRetweet);
        }

        [TestMethod]
        public void ScreenName_ShoulBeFormattedWithOriginalUserScreenName_WhenRetweetStatusIsNotNullTest()
        {
            NGTweeterStatus tweeterStatus = new NGTweeterStatus
            {
                CreatedDate = DateTime.Today,
                Id = 101,
                RetweetedStatus =
                    new NGTweeterStatus
                    {
                        CreatedDate = DateTime.Today,
                        Id = 102,
                        Tweet = "Original Tweet",
                        User =
                            new NGTweeterUser
                            {
                                Id = 5,
                                Name = "Nilesh",
                                ProfileImageUrl = "Nilesh.jpg",
                                ScreenName = "NileshGule"
                            }
                    },
                User = new NGTweeterUser
                {
                    Id = 6,
                    Name = "TestName",
                    ProfileImageUrl = "Test.jpg",
                    ScreenName = "TestScreenName"
                }
            };

            _viewModel = new TweetViewModel(tweeterStatus);
            Assert.AreEqual("NileshGule, (RT by TestScreenName)", _viewModel.ScreenName);
        }

        [TestMethod]
        public void ScreenName_ShouldReturnOnlyUsersScreenName_WhenRetweetStatusIsNullTest()
        {
            NGTweeterStatus tweeterStatus = new NGTweeterStatus
            {
                CreatedDate = DateTime.Today,
                Id = 102,
                Tweet = "Original Tweet",
                User =
                    new NGTweeterUser
                    {
                        Id = 5,
                        Name = "Nilesh",
                        ProfileImageUrl = "Nilesh.jpg",
                        ScreenName = "NileshGule"
                    }
            };

            _viewModel = new TweetViewModel(tweeterStatus);
            Assert.AreEqual("NileshGule", _viewModel.ScreenName);
        }
    }
}