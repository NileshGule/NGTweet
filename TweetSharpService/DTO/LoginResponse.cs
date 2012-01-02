using System.Runtime.Serialization;

using TweetSharp;

namespace TweetSharpService.DTO
{
    [DataContract]
    public class LoginResponse
    {
        [DataMember]
        public OAuthAccessToken OAuthAccessToken { get; set; }
    }
}