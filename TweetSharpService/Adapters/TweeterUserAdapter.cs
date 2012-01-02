using TweetSharp;

using TweetSharpService.DomainObjects;

namespace TweetSharpService.Adapters
{
    public class TweeterUserAdapter
    {
        public NGTweeterUser Convert(TwitterUser twitterUser)
        {
            return new NGTweeterUser
                {
                    Id = twitterUser.Id,
                    Name = twitterUser.Name,
                    ScreenName = twitterUser.ScreenName,
                    ProfileImageUrl = twitterUser.ProfileImageUrl
                };
        }
    }
}