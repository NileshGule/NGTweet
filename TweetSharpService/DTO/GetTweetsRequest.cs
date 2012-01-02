using System.Runtime.Serialization;

using TweetSharp;

namespace TweetSharpService.DTO
{
    [DataContract]
    public class GetTweetsRequest
    {
        [DataMember]
        public OAuthAccessToken AccessToken { get; set; }

        [DataMember]
        public long LastTweetId { get; set; }
    }
}