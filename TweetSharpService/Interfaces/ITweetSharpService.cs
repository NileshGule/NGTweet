using System.ServiceModel;

using TweetSharpService.DTO;

namespace TweetSharpService.Interfaces
{
    [ServiceContract]
    public interface ITweetSharpService
    {
        [OperationContract]
        GetTweetResponse GetTweetsOnPublicTimeline();
    }
}