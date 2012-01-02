using System.Collections.Generic;
using System.Runtime.Serialization;

using TweetSharpService.DomainObjects;

namespace TweetSharpService.DTO
{
    [DataContract]
    public class GetTweetResponse
    {
        [DataMember]
        public IEnumerable<NGTweeterStatus> TweeterStatuses { get; set; }
    }
}