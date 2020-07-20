using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using NewsfeedApi.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NewsfeedApi.Services
{
    public class HackerNewsServiceService : IHackerNewsService
    {
        private readonly ICache<NewsArticle> _newsArticleCache;
        private readonly HttpClient _httpClient = new HttpClient();

        public HackerNewsServiceService(ICache<NewsArticle> newsArticleCache)
        {
            _newsArticleCache = newsArticleCache;
        }

        private async Task<NewsArticle> GetArticleAsync(int articleId)
        {
            if (_newsArticleCache.TryGetValue(articleId.ToString(), out var value))
                return value;
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var body = await _httpClient.GetStringAsync($"https://hacker-news.firebaseio.com/v0/item/{articleId}.json");
            if (string.IsNullOrEmpty(body) || body == "null")
                return null;
            var jobject = JObject.Parse(body);
            var story = jobject["type"].Value<string>();
            if (jobject["type"].Value<string>() == "story")
            {
                var newsArticle = JsonConvert.DeserializeObject<NewsArticle>(body);
                _newsArticleCache.CreateEntry(articleId.ToString(), newsArticle);
                return newsArticle;
            }
            else
            {
                _newsArticleCache.CreateEntry(articleId.ToString(), null);
            }

            return null;
        }

        public IEnumerable<NewsArticle> GetLatestNewsArticles(int count, int lowestArticleId)
        {
            return null;
        }

        public async Task<IEnumerable<NewsArticle>> GetLatestArticlesAsync(int count, int lowestArticleId)
        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var body = await _httpClient.GetStringAsync("https://hacker-news.firebaseio.com/v0/newstories.json");
            var articleIds = JsonConvert.DeserializeObject<IEnumerable<int>>(body);
            articleIds = articleIds.OrderByDescending(i => i)
                .Where(i => i < lowestArticleId)
                .Take(count).ToArray();
            var articleTasks = new List<Task<NewsArticle>>();
            foreach (var id in articleIds)
                articleTasks.Add(GetArticleAsync(id));
            var articles = await Task.WhenAll(articleTasks);
            return articles.Where(i => i != null);
        }

        public async Task<IEnumerable<NewsArticle>> SearchAsync(string keyword)
        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var bodyTask = _httpClient.GetStringAsync($"https://hn.algolia.com/api/v1/search_by_date?query={keyword}&tags=story");
            return JObject.Parse(await  bodyTask)["hits"].ToObject<IEnumerable<NewsArticle>>();
        }
    }
}