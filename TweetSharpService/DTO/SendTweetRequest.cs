using System.Runtime.Serialization;

using TweetSharp;

namespace TweetSharpService.DTO
{
    [DataContract]
    public class SendTweetRequest
    {
        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public OAuthAccessToken AccessToken { get; set; }
    }
}