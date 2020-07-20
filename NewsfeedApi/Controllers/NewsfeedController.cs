using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsfeedApi.Domain;
using NewsfeedApi.Services;

namespace NewsfeedApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsFeedController : ControllerBase
    {
        private readonly IHackerNewsService _hackerNewsService;

        public NewsFeedController(IHackerNewsService hackerNewsService)
        {
            _hackerNewsService = hackerNewsService;
        }

        // GET: api/Newsfeed
        [HttpGet]
        public async Task<IEnumerable<NewsArticle>> Get([FromQuery(Name = "count")] int count = 20, [FromQuery(Name = "lowestArticleId")] int lowestArticleId = int.MaxValue)
        {
            return await _hackerNewsService.GetLatestArticlesAsync(count, lowestArticleId);
        }

        [HttpGet]
        [Route("search")]
        public async Task<IEnumerable<NewsArticle>> Search([FromQuery(Name = "title")] string title)
        {
            return await _hackerNewsService.SearchAsync(title);
        }
    }
}
