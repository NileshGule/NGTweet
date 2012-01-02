using System.Runtime.Serialization;

namespace TweetSharpService.DomainObjects
{
    [DataContract]
    public class NGTweeterUser
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string ScreenName { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string ProfileImageUrl { get; set; }
    }
}