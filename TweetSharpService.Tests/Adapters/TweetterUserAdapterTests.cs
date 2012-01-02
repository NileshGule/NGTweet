using Microsoft.VisualStudio.TestTools.UnitTesting;

using TweetSharp;

using TweetSharpService.Adapters;
using TweetSharpService.DomainObjects;

namespace TweetSharpService.Tests.Adapters
{
    [TestClass]
    public class TweetterUserAdapterTests
    {
        private TweeterUserAdapter userAdapter;

        [TestInitialize]
        public void Setup()
        {
            userAdapter = new TweeterUserAdapter();
        }

        [TestMethod]
        public void Convert_WithTwitterUser_ShouldAdaptToNGTweeterUser()
        {
            TwitterUser sourceUser = new TwitterUser
                {
                    Id = 123,
                    Name = "Nilesh",
                    ScreenName = "nileshgule",
                    ProfileImageUrl = "http://twitter.com/profileImages/nileshgule.jpg"
                };

            NGTweeterUser result = userAdapter.Convert(sourceUser);

            Assert.AreEqual(123, result.Id);
            Assert.AreEqual("Nilesh", result.Name);
            Assert.AreEqual("nileshgule", result.ScreenName);
            Assert.AreEqual("http://twitter.com/profileImages/nileshgule.jpg", result.ProfileImageUrl);
        }
    }
}