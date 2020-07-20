using NewsfeedApi.Services;
using NUnit.Framework;

namespace NewsfeedApi.Test
{
    [TestFixture]
    public class HackerNewsServiceTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void InstantiationTest()
        {
            Assert.DoesNotThrow(() =>
            {
                var hackerNews = new HackerNewsServiceService(new InMemoryCache());
            });

            IHackerNewsService service = new HackerNewsServiceService(new InMemoryCache());
            Assert.NotNull(service);
        }
    }
}