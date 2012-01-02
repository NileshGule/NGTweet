using System;
using System.Runtime.Serialization;

namespace TweetSharpService.DomainObjects
{
    [DataContract]
    public class NGTweeterStatus
    {
        [DataMember]
        public string Tweet { get; set; }

        [DataMember]
        public NGTweeterUser User { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public NGTweeterStatus RetweetedStatus { get; set; }
    }
}