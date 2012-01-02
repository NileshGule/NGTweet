using System;

using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using NGTweet.ViewModels;
using NGTweet.ViewModels.INGTweetAuthenticationService;

namespace NGTweet.ViewModelTests
{
    [TestClass]
    [Tag("TweetActionViewModelTest")]
    public class TweetActionViewModelTest
    {
        private TweetActionViewModel viewModel;

        private INGTweetAuthenticationService _mockAuthenticationService;

        private IApplicationSettingsProvider _mockApplicationSettingsProvider;

        [TestInitialize]
        public void Setup()
        {
            _mockAuthenticationService = new Mock<INGTweetAuthenticationService>().Object;

            _mockApplicationSettingsProvider = new Mock<IApplicationSettingsProvider>().Object;

            viewModel = new TweetActionViewModel(_mockAuthenticationService, _mockApplicationSettingsProvider);
        }

        [TestMethod]
        public void ConstructorTest()
        {
            Assert.IsNotNull(viewModel);
            Assert.IsNotNull(viewModel.TweetActionCommand);
            Assert.IsFalse(viewModel.CanPerformTweetAction);
            Assert.AreEqual(0, viewModel.TweetText.Length);
        }

        [TestMethod]
        public void CanPerformTweetAction_WithEmptyText_ShouldReturnFalseTest()
        {
            viewModel.TweetText = string.Empty;

            Assert.IsFalse(viewModel.CanPerformTweetAction);
        }

        [TestMethod]
        public void CanPerformTweetAction_WithWhiteSpace_ShouldReturnFalseTest()
        {
            viewModel.TweetText = " ";

            Assert.IsFalse(viewModel.CanPerformTweetAction);
        }

        [TestMethod]
        public void CanPerformTweetAction_WithValidText_ShouldReturnTrueTest()
        {
            viewModel.TweetText = "This is the first tweet";

            Assert.IsTrue(viewModel.CanPerformTweetAction);
        }

        [TestMethod]
        public void CanPerformTweetAction_WithMoreThanMaximumAllowedCharacters_ShouldReturnFalseTest()
        {
            viewModel.TweetText = "This is the first tweet. This is the first tweet. This is the first tweet. This is the first tweet. This is the first tweet. This is the first tweet. This is the first tweet. ";

            Assert.IsFalse(viewModel.CanPerformTweetAction);
        }

        [TestMethod]
        public void OnTweetActionCommand_ShouldCallServiceTest()
        {
            viewModel.TweetText = "Hello World";

            OAuthAccessToken accessToken = new OAuthAccessToken();

            Mock.Get(_mockApplicationSettingsProvider)
                .SetupGet(m => m["accessToken"])
                .Returns(accessToken);

            Mock.Get(_mockAuthenticationService).Setup(
                m => m.BeginSendTweet(
                    It.Is<SendTweetRequest>(request => request.AccessToken == accessToken && request.Status == "Hello World"),
                    It.IsAny<AsyncCallback>(),
                    null));

            viewModel.OnTweetActionCommand();

            Assert.IsTrue(viewModel.IsBusy);

            Mock.Get(_mockAuthenticationService).VerifyAll();
        }

        [TestMethod]
        public void TweetActionViewModel_SendTweetCallBack_ShouldReturnNewTweetTest()
        {
            viewModel.IsBusy = true;

            viewModel.TweetText = "Hello World";

            viewModel.CanPerformTweetAction = true;

            SendTweetResponse sendTweetResponse = new SendTweetResponse();

            Mock.Get(_mockAuthenticationService)
                .Setup(m => m.EndSendTweet(null))
                .Returns(sendTweetResponse);

            viewModel.SendTweetCallback(null);

            Assert.IsFalse(viewModel.IsBusy);

            Assert.AreEqual(0, viewModel.TweetText.Length);

            Assert.IsFalse(viewModel.CanPerformTweetAction);

            Mock.Get(_mockAuthenticationService).VerifyAll();
        }
    }
}