using System;
using System.Collections.Generic;
using System.Linq;

using FizzWare.NBuilder;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using TweetSharp;

using TweetSharpService.DTO;

namespace TweetSharpService.Tests.Services
{
    [TestClass]
    public class NGTweetAuthenticationServiceTest
    {
        private NGTweetAuthenticationService _authenticationService;

        private TwitterService _mockTwitterService;

        [TestInitialize]
        public void Setup()
        {
            _mockTwitterService = new Mock<TwitterService>().Object;

            _authenticationService = new NGTweetAuthenticationService(_mockTwitterService);
        }

        [TestMethod]
        public void Login_ShouldReturnLoginResponse()
        {
            LoginRequest loginRequest = new LoginRequest { RequestToken = new OAuthRequestToken(), Pin = "1234567" };

            OAuthAccessToken accessToken = new OAuthAccessToken();

            Mock.Get(_mockTwitterService)
                .Setup(mockService => mockService.GetAccessToken(loginRequest.RequestToken, loginRequest.Pin))
                .Returns(accessToken);

            LoginResponse result = _authenticationService.Login(loginRequest);

            Mock.Get(_mockTwitterService).Verify(mockService => mockService.GetAccessToken(loginRequest.RequestToken, loginRequest.Pin));

            Assert.AreSame(accessToken, result.OAuthAccessToken);
        }

        [TestMethod]
        public void GetRequestToken_ShouldReturnRequestTokenResponse()
        {
            OAuthRequestToken requestToken = new OAuthRequestToken();

            Uri authorizationUri = new Uri("http://Twitter.com/xyz");

            Mock.Get(_mockTwitterService)
                .Setup(mockService => mockService.GetRequestToken())
                .Returns(requestToken);

            Mock.Get(_mockTwitterService)
                .Setup(mockService => mockService.GetAuthorizationUri(requestToken))
                .Returns(authorizationUri);

            GetRequestTokenResponse requestTokenResponse = _authenticationService.GetRequestToken();

            Mock.Get(_mockTwitterService).VerifyAll();

            Assert.AreEqual(requestToken, requestTokenResponse.RequestToken);
            Assert.AreEqual(authorizationUri, requestTokenResponse.AuthorizationUri);
        }

        [TestMethod]
        public void GetTweetsMentioningMe_ShouldReturnTweetsWithMentions()
        {
            List<TwitterStatus> twitterStatuses = Builder<TwitterStatus>
                .CreateListOfSize(2)
                .WhereAll()
                .Have(ts => ts.User = Builder<TwitterUser>.CreateNew().Build())
                .Build()
                .ToList();

            OAuthAccessToken accessToken = new OAuthAccessToken { Token = "abcd", TokenSecret = "A@c#" };

            Mock.Get(_mockTwitterService).Setup(mockService => mockService.AuthenticateWith(accessToken.Token, accessToken.TokenSecret));

            Mock.Get(_mockTwitterService).Setup(mockService => mockService.ListTweetsMentioningMe()).Returns(
                twitterStatuses);

            GetTweetResponse response = _authenticationService.GetTweetsMentioningMe(accessToken);

            Mock.Get(_mockTwitterService).VerifyAll();

            Assert.IsNotNull(response);
            Assert.AreEqual(2, response.TweeterStatuses.Count());
        }

        [TestMethod]
        public void GetTweetsSince_ShouldReturnTweetsSinceTweetId()
        {
            List<TwitterStatus> twitterStatuses = Builder<TwitterStatus>
                .CreateListOfSize(2)
                .WhereAll()
                .Have(ts => ts.User = Builder<TwitterUser>.CreateNew().Build())
                .Build()
                .ToList();

            OAuthAccessToken accessToken = new OAuthAccessToken { Token = "abcd", TokenSecret = "A@c#" };

            Mock.Get(_mockTwitterService).Setup(mockService => mockService.AuthenticateWith(accessToken.Token, accessToken.TokenSecret));

            Mock.Get(_mockTwitterService).Setup(mockService => mockService.ListTweetsOnHomeTimelineSince(100)).Returns(
                twitterStatuses);

            GetTweetsRequest getTweetsRequest = new GetTweetsRequest { AccessToken = accessToken, LastTweetId = 100 };
            GetTweetResponse response = _authenticationService.GetTweetsSince(getTweetsRequest);

            Mock.Get(_mockTwitterService).VerifyAll();

            Assert.IsNotNull(response);
            Assert.AreEqual(2, response.TweeterStatuses.Count());
        }

        [TestMethod]
        public void SendTweet_ShouldUpdateTheUserStatus_AndReturnNewTweeterStatusTest()
        {
            OAuthAccessToken accessToken = new OAuthAccessToken { Token = "abcd", TokenSecret = "A@c#" };

            Mock.Get(_mockTwitterService).Setup(mockService => mockService.AuthenticateWith(accessToken.Token, accessToken.TokenSecret));

            SendTweetRequest request = new SendTweetRequest { AccessToken = accessToken, Status = "Hello World" };

            TwitterStatus twitterStatusStatus =
                Builder<TwitterStatus>
                .CreateNew()
                .With(x => x.Text = request.Status)
                .With(x => x.User = Builder<TwitterUser>.CreateNew().Build())
                .Build();

            Mock.Get(_mockTwitterService).Setup(mockService => mockService.SendTweet(request.Status)).Returns(twitterStatusStatus);

            SendTweetResponse sendTweetResponse = _authenticationService.SendTweet(request);

            Assert.IsNotNull(sendTweetResponse);
            Assert.AreSame(request.Status, sendTweetResponse.TweeterStatus.Tweet);
        }

        [TestMethod]
        public void ReTweet_ShouldTweetSomeOtherUsersMessage_AndReturnRetweetedMessageTest()
        {
            OAuthAccessToken accessToken = new OAuthAccessToken { Token = "abcd", TokenSecret = "A@c#" };

            Mock.Get(_mockTwitterService).Setup(mockService => mockService.AuthenticateWith(accessToken.Token, accessToken.TokenSecret));

            RetweetRequest request = new RetweetRequest { AccessToken = accessToken, TweetId = 101 };

            Mock.Get(_mockTwitterService).Setup(mockService => mockService.Retweet(request.TweetId));

            _authenticationService.Retweet(request);

            Mock.Get(_mockTwitterService).VerifyAll();
        }
    }
}