using System.Collections.Generic;
using System.Linq;

using TweetSharp;

using TweetSharpService.DomainObjects;

namespace TweetSharpService.Adapters
{
    public class TweeterStatusAdapter
    {
        public IEnumerable<NGTweeterStatus> Convert(IEnumerable<TwitterStatus> tweets)
        {
            return tweets
                .Select(Convert)
                .ToList();
        }

        public NGTweeterStatus Convert(TwitterStatus twitterStatus)
        {
            return new NGTweeterStatus
                {
                    Id = twitterStatus.Id,
                    User = new TweeterUserAdapter().Convert(twitterStatus.User),
                    Tweet = twitterStatus.Text,
                    CreatedDate = twitterStatus.CreatedDate.AddHours(8),
                    RetweetedStatus = twitterStatus.RetweetedStatus != null ? Convert(twitterStatus.RetweetedStatus) : null
                };
        }
    }
}