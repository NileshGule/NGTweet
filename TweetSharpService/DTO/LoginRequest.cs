using System.Runtime.Serialization;

using TweetSharp;

namespace TweetSharpService.DTO
{
    [DataContract]
    public class LoginRequest
    {
        [DataMember]
        public OAuthRequestToken RequestToken { get; set; }

        [DataMember]
        public string Pin { get; set; }
    }
}