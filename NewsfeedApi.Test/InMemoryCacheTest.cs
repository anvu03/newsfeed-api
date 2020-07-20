using NewsfeedApi.Domain;
using NUnit.Framework;

namespace NewsfeedApi.Test
{
    [TestFixture]
    public class InMemoryCacheTest
    {
        [Test]
        public void TestConstructor()
        {
            Assert.DoesNotThrow(() =>
            {
                var cache = new InMemoryCache();
            });

            Assert.NotNull(new InMemoryCache());
        }
        [Test]
        public void TestCreateEntry_NoException()
        {
            var cache = new InMemoryCache();

            Assert.DoesNotThrow(() =>
            {
                cache.CreateEntry("key", new NewsArticle());
            });
        }

        [Test]
        public void TestTryGetValue_EmptyCache_ReturnFalse()
        {
            var cache = new InMemoryCache();
            Assert.IsFalse(cache.TryGetValue("key", out var value));
            Assert.IsNull(value);
        }

        [Test]
        public void TestCreateEntry_ReturnValue()
        {
            var cache = new InMemoryCache();
            cache.CreateEntry("key", new NewsArticle());
            Assert.IsTrue(cache.TryGetValue("key", out var value));
            Assert.NotNull(value);
        }

        [Test]
        public void TestCreateEntry_DupKey_NoException_Override()
        {
            var cache = new InMemoryCache();
            cache.CreateEntry("key", new NewsArticle(){Id = 1});
            Assert.DoesNotThrow(() =>
            {
                cache.CreateEntry("key", new NewsArticle(){Id=2});
            });
            Assert.IsTrue(cache.TryGetValue("key", out var value));
            Assert.AreEqual(2, value.Id);
        }

        [Test]
        public void TestRemove_KeyNotExist_DoesNotThrow()
        {
            var cache = new InMemoryCache();
            Assert.DoesNotThrow(() =>
            {
                cache.Remove("key");
            });
        }

        [Test]
        public void TestRemove_KeyExists_ShouldRemove()
        {
            var cache = new InMemoryCache();
            cache.CreateEntry("key", new NewsArticle());
            Assert.DoesNotThrow(() =>
            {
                cache.Remove("key");
            });

            Assert.IsFalse(cache.TryGetValue("key", out var value));
        }
    }
}