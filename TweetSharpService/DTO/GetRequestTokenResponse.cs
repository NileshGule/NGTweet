using System;
using System.Runtime.Serialization;

using TweetSharp;

namespace TweetSharpService.DTO
{
    [DataContract]
    public class GetRequestTokenResponse
    {
        [DataMember]
        public OAuthRequestToken RequestToken { get; set; }

        [DataMember]
        public Uri AuthorizationUri { get; set; }
    }
}