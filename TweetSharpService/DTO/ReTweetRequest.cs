using System.Runtime.Serialization;

using TweetSharp;

namespace TweetSharpService.DTO
{
    [DataContract]
    public class RetweetRequest
    {
        [DataMember]
        public OAuthAccessToken AccessToken { get; set; }

        [DataMember]
        public long TweetId { get; set; }
    }
}