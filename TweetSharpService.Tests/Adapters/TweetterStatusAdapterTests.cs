using System.Collections.Generic;
using System.Linq;

using FizzWare.NBuilder;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using TweetSharp;
using TweetSharpService.Adapters;
using TweetSharpService.DomainObjects;

namespace TweetSharpService.Tests.Adapters
{
    [TestClass]
    public class TweetterStatusAdapterTests
    {
        private TweeterStatusAdapter _tweeterStatusAdapter;

        [TestInitialize]
        public void Setup()
        {
            _tweeterStatusAdapter = new TweeterStatusAdapter();
        }

        [TestMethod]
        public void Convert_WithTwoTwitterStatuses_ShouldReturnTwoResults()
        {
            List<TwitterStatus> twitterStatuses = Builder<TwitterStatus>
                .CreateListOfSize(2)
                .WhereAll()
                .Have(ts => ts.User = Builder<TwitterUser>.CreateNew().Build())
                .Build()
                .ToList();

            IEnumerable<NGTweeterStatus> result = _tweeterStatusAdapter.Convert(twitterStatuses);

            Assert.AreEqual(2, result.Count());

            VerifyListResults(result.ToList(), twitterStatuses);
        }

        [TestMethod]
        public void Convert_WithSingleTwitterStatuses_ShouldReturnSingleResults()
        {
            TwitterStatus twitterStatus =
                Builder<TwitterStatus>
                .CreateNew().With(ts => ts.User = Builder<TwitterUser>.CreateNew().Build())
                .Build();

            NGTweeterStatus result = _tweeterStatusAdapter.Convert(twitterStatus);

            VerifyResults(result, twitterStatus);
        }

        private static void VerifyListResults(IList<NGTweeterStatus> adaptedStatuses, List<TwitterStatus> twitterStatuses)
        {
            for (int count = 0; count < adaptedStatuses.Count(); count++)
            {
                VerifyResults(adaptedStatuses[count], twitterStatuses[count]);
            }
        }

        private static void VerifyResults(NGTweeterStatus adaptedStatus, TwitterStatus twitterStatus)
        {
            Assert.AreEqual(twitterStatus.Text, adaptedStatus.Tweet);
            Assert.AreEqual(twitterStatus.CreatedDate.AddHours(8), adaptedStatus.CreatedDate);
        }
    }
}