using System.Collections.Generic;

using TweetSharp;

using TweetSharpService.Adapters;
using TweetSharpService.DTO;
using TweetSharpService.Interfaces;

namespace TweetSharpService
{
    public class NGTweetSharpService : ITweetSharpService
    {
        public GetTweetResponse GetTweetsOnPublicTimeline()
        {
            TwitterService service = new TwitterService();
            IEnumerable<TwitterStatus> tweets = service.ListTweetsOnPublicTimeline();

            TweeterStatusAdapter adapter = new TweeterStatusAdapter();

            return new GetTweetResponse { TweeterStatuses = adapter.Convert(tweets) };
        }
    }
}