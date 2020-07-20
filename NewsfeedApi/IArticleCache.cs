using System.Threading.Tasks;
using NewsfeedApi.Domain;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace NewsfeedApi
{
    public interface ICache<T>
    {
        void CreateEntry(string key, T obj);
        void Remove(string key);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>True if key exists</returns>
        bool TryGetValue(string key, out T value);
    }
    public class ArticleCache : ICache<NewsArticle>
    {
        private readonly IConnectionMultiplexer _redis;

        public ArticleCache(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public void CreateEntry(string key, NewsArticle obj)
        {
            _redis.GetDatabase().StringSet(key, JsonConvert.SerializeObject(obj));
        }

        public void Remove(string key)
        {
            _redis.GetDatabase().KeyDelete(key);
        }

        public bool TryGetValue(string key, out NewsArticle value)
        {
            if (_redis.GetDatabase().KeyExists(key))
            {
                var redisValue = _redis.GetDatabase().StringGet(key);
                value = JsonConvert.DeserializeObject<NewsArticle>(redisValue);
                return true;
            }

            value = null;
            return false;
        }
    }
}