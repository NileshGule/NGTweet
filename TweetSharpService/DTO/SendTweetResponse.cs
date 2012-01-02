using System.Runtime.Serialization;

using TweetSharpService.DomainObjects;

namespace TweetSharpService.DTO
{
    [DataContract]
    public class SendTweetResponse
    {
        [DataMember]
        public NGTweeterStatus TweeterStatus { get; set; }
    }
}