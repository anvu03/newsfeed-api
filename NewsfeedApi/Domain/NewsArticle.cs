using System;
using Newtonsoft.Json;

namespace NewsfeedApi.Domain
{
    public class NewsArticle
    {
        public int Id { get; set; }
        [JsonProperty("time")] public int CreationDateUnix { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        [JsonProperty("by")] 
        public string Creator { get; set; }
        [JsonProperty("author")]
        public string Author
        {
            get => Creator;
            set => Creator = value;
        }

        public int Score { get; set; }
    }
}