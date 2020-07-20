using System.Collections.Generic;
using System.Threading.Tasks;
using NewsfeedApi.Domain;

namespace NewsfeedApi.Services
{
    public interface IHackerNewsService
    {
        IEnumerable<NewsArticle> GetLatestNewsArticles(int count, int lowestArticleId);
        Task<IEnumerable<NewsArticle>> GetLatestArticlesAsync(int count, int lowestArticleId);
        Task<IEnumerable<NewsArticle>> SearchAsync(string keyword);
    }
}