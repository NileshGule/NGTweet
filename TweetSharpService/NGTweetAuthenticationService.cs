using System;
using System.Collections.Generic;

using TweetSharp;

using TweetSharpService.Adapters;
using TweetSharpService.DTO;
using TweetSharpService.Interfaces;

namespace TweetSharpService
{
    public class NGTweetAuthenticationService : INGTweetAuthenticationService
    {
        private const int INITIAL_TWEETS_COUNT = 100;

        private readonly TwitterService _twitterService;

        public NGTweetAuthenticationService(TwitterService twitterService)
        {
            _twitterService = twitterService;
        }

        public NGTweetAuthenticationService()
            : this(new TwitterService("8LIt3KSstszyLKfbKKwE6A", "AmcD9Fzvv7lTW3kOws3oiDBsWSdfQj6BPBGpgjDY"))
        {
            //_twitterService = new TwitterService("YourAppConsumerKeyHere", "YourAppConsumeSecretHere");
        }

        //public NGTweetAuthenticationService()
        //{
        //    _twitterService = new TwitterService("YourAppConsumerKeyHere", "YourAppConsumeSecretHere");
        //}

        public LoginResponse Login(LoginRequest request)
        {
            OAuthAccessToken access = _twitterService.GetAccessToken(request.RequestToken, request.Pin);

            return new LoginResponse { OAuthAccessToken = access };
        }

        public GetTweetResponse GetTweets(OAuthAccessToken access)
        {
            _twitterService.AuthenticateWith(access.Token, access.TokenSecret);

            IEnumerable<TwitterStatus> tweets = _twitterService.ListTweetsOnHomeTimeline(INITIAL_TWEETS_COUNT);

            TweeterStatusAdapter adapter = new TweeterStatusAdapter();

            return new GetTweetResponse { TweeterStatuses = adapter.Convert(tweets) };
        }

        public GetRequestTokenResponse GetRequestToken()
        {
            OAuthRequestToken requestToken = _twitterService.GetRequestToken();

            Uri uri = _twitterService.GetAuthorizationUri(requestToken);

            return new GetRequestTokenResponse { RequestToken = requestToken, AuthorizationUri = uri };
        }

        public GetTweetResponse GetTweetsMentioningMe(OAuthAccessToken access)
        {
            _twitterService.AuthenticateWith(access.Token, access.TokenSecret);

            IEnumerable<TwitterStatus> tweets = _twitterService.ListTweetsMentioningMe();

            TweeterStatusAdapter adapter = new TweeterStatusAdapter();

            return new GetTweetResponse { TweeterStatuses = adapter.Convert(tweets) };
        }

        public GetTweetResponse GetTweetsSince(GetTweetsRequest request)
        {
            OAuthAccessToken accessToken = request.AccessToken;

            _twitterService.AuthenticateWith(accessToken.Token, accessToken.TokenSecret);

            IEnumerable<TwitterStatus> tweets = _twitterService.ListTweetsOnHomeTimelineSince(request.LastTweetId);

            TweeterStatusAdapter adapter = new TweeterStatusAdapter();

            return new GetTweetResponse { TweeterStatuses = adapter.Convert(tweets) };
        }

        public SendTweetResponse SendTweet(SendTweetRequest request)
        {
            _twitterService.AuthenticateWith(request.AccessToken.Token, request.AccessToken.TokenSecret);

            TwitterStatus twitterStatus = _twitterService.SendTweet(request.Status);

            TweeterStatusAdapter tweeterStatusAdapter = new TweeterStatusAdapter();

            SendTweetResponse response = new SendTweetResponse { TweeterStatus = tweeterStatusAdapter.Convert(twitterStatus) };

            return response;
        }

        public RetweetResponse Retweet(RetweetRequest request)
        {
            _twitterService.AuthenticateWith(request.AccessToken.Token, request.AccessToken.TokenSecret);

            _twitterService.Retweet(request.TweetId);

            return new RetweetResponse();
        }
    }
}