using System.ServiceModel;

using TweetSharp;

using TweetSharpService.DTO;

namespace TweetSharpService.Interfaces
{
    [ServiceContract]
    public interface INGTweetAuthenticationService
    {
        [OperationContract]
        LoginResponse Login(LoginRequest request);

        [OperationContract]
        GetRequestTokenResponse GetRequestToken();

        [OperationContract]
        GetTweetResponse GetTweets(OAuthAccessToken access);

        [OperationContract]
        GetTweetResponse GetTweetsMentioningMe(OAuthAccessToken access);

        [OperationContract]
        GetTweetResponse GetTweetsSince(GetTweetsRequest request);

        [OperationContract]
        SendTweetResponse SendTweet(SendTweetRequest request);

        [OperationContract]
        RetweetResponse Retweet(RetweetRequest request);
    }
}