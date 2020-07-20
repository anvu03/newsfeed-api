using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewsfeedApi.Domain;

namespace NewsfeedApi
{
    public class InMemoryCache : ICache<NewsArticle>
    {
        private readonly Dictionary<string, NewsArticle> _cache = new Dictionary<string, NewsArticle>();

        public void CreateEntry(string key, NewsArticle obj)
        {
            try
            {
                if (_cache.ContainsKey(key))
                    _cache[key] = obj;
                else
                    _cache.Add(key, obj);
            }
            catch (Exception)
            {
            }
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        public bool TryGetValue(string key, out NewsArticle value)
        {
            return _cache.TryGetValue(key, out value);
        }
    }
}