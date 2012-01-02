using System;

using GalaSoft.MvvmLight.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using NGTweet.ViewModels;
using NGTweet.ViewModels.INGTweetAuthenticationService;

namespace NGTweet.ViewModelTests
{
    [TestClass]
    public class MainViewModelTests
    {
        private MainViewModel _viewModel;

        private INGTweetAuthenticationService _mockAuthenticationService;

        private IApplicationSettingsProvider _mockApplicationSettingsProvider;

        [TestInitialize]
        public void Setup()
        {
            _mockAuthenticationService = new Mock<INGTweetAuthenticationService>().Object;
            _mockApplicationSettingsProvider = new Mock<IApplicationSettingsProvider>().Object;

            DispatcherHelper.Initialize();

            _viewModel = new MainViewModel(_mockAuthenticationService, _mockApplicationSettingsProvider);
        }

        [TestMethod]
        public void ConstructorTest()
        {
            Assert.IsNotNull(_viewModel);
            Assert.IsFalse(_viewModel.IsVerificationDetailsVisible);
            Assert.IsFalse(_viewModel.IsAuthenticUser);
        }

        [TestMethod]
        public void CheckIfAlreadyLoggedInUser_WithStoredAccessToken_ShouldRetrunTrueTest()
        {
            Mock.Get(_mockApplicationSettingsProvider)
                .Setup(m => m.Contains("accessToken"))
                .Returns(true);

            bool result = _viewModel.UserHasLoginHistory();

            Assert.IsTrue(result);

            Mock.Get(_mockApplicationSettingsProvider).VerifyAll();
        }

        [TestMethod]
        public void CheckIfAlreadyLoggedInUser_WithoutStoredAccessToken_ShouldRetrunFalseTest()
        {
            Mock.Get(_mockApplicationSettingsProvider)
                .Setup(m => m.Contains("accessToken"))
                .Returns(false);

            bool result = _viewModel.UserHasLoginHistory();

            Assert.IsFalse(result);

            Mock.Get(_mockApplicationSettingsProvider).VerifyAll();
        }

        [TestMethod]
        public void GetTweetsForUser_WithValidAccessToken_ShouldSetAuthenticUserToTrue()
        {
            _viewModel.GetTweetsForUser();

            Assert.IsFalse(_viewModel.IsVerificationDetailsVisible);
            Assert.IsTrue(_viewModel.IsAuthenticUser);
        }

        [TestMethod]
        public void Login_WithNoUserHistory_ShouldRequestForNewRequestToken()
        {
            Mock.Get(_mockApplicationSettingsProvider)
                .Setup(m => m.Contains("accessToken"))
                .Returns(false);

            Mock.Get(_mockAuthenticationService)
                .Setup(m => m.BeginGetRequestToken(It.IsAny<AsyncCallback>(), null));

            _viewModel.Login();

            Assert.IsTrue(_viewModel.IsVerificationDetailsVisible);

            Mock.Get(_mockAuthenticationService).VerifyAll();

            Mock.Get(_mockApplicationSettingsProvider).VerifyAll();
        }

        [TestMethod]
        public void GetRequestTokenCompleted_WithSuccess_ShouldRedirectUserToVerificationPage()
        {
            GetRequestTokenResponse requestTokenResponse = new GetRequestTokenResponse
                {
                    AuthorizationUri = new Uri("http://twitter.com/AuthorizeApp/NGTweet"),
                    RequestToken = new OAuthRequestToken()
                };

            Mock.Get(_mockAuthenticationService)
                .Setup(m => m.EndGetRequestToken(It.IsAny<IAsyncResult>()))
                .Returns(requestTokenResponse);

            Mock.Get(_mockApplicationSettingsProvider)
                .SetupSet(m => m["requestToken"] = requestTokenResponse.RequestToken);

            _viewModel.GetRequestTokenCompleted(new Mock<IAsyncResult>().Object);

            Assert.IsFalse(_viewModel.IsAuthenticUser);
            Assert.IsTrue(_viewModel.IsVerificationDetailsVisible);

            Mock.Get(_mockAuthenticationService).VerifyAll();
            Mock.Get(_mockApplicationSettingsProvider).VerifyAll();
        }

        [TestMethod]
        public void AuthorizeWithPIN_ShouldAuthorizeUser()
        {
            GetRequestTokenResponse requestTokenResponse = new GetRequestTokenResponse
                {
                    AuthorizationUri = new Uri("http://twitter.com/login"),
                    RequestToken = new OAuthRequestToken()
                };

            Mock.Get(_mockApplicationSettingsProvider)
                .SetupGet(m => m["requestToken"]).Returns(requestTokenResponse.RequestToken);

            Mock.Get(_mockAuthenticationService)
                .Setup(m => m.BeginLogin(It.IsAny<LoginRequest>(), It.IsAny<AsyncCallback>(), null));

            _viewModel.AuthorizeWithPin();

            Mock.Get(_mockApplicationSettingsProvider).VerifyAll();
            Mock.Get(_mockAuthenticationService).VerifyAll();
        }

        [TestMethod]
        public void AuthorizeWithPinCompleted_ShouldNotifyUser()
        {
            const string successMessage = "You have successfully authorized NGTweet to access Twitter.";

            LoginResponse loginResponse = new LoginResponse { OAuthAccessToken = new OAuthAccessToken() };

            Mock.Get(_mockAuthenticationService)
                .Setup(m => m.EndLogin(It.IsAny<IAsyncResult>()))
                .Returns(loginResponse);

            Mock.Get(_mockApplicationSettingsProvider)
                .SetupSet(m => m["accessToken"] = loginResponse.OAuthAccessToken);

            _viewModel.AuthorizeWithPinCompleted(new Mock<IAsyncResult>().Object);

            Assert.IsFalse(_viewModel.IsVerificationDetailsVisible);

            Mock.Get(_mockApplicationSettingsProvider).VerifyAll();
            Mock.Get(_mockAuthenticationService).VerifyAll();
        }
    }
}