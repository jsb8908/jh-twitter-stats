

namespace jh_twitter_stats_api.Models
{
    /// <summary>
    /// Twitter returns their response in an object like this
    /// See: https://developer.twitter.com/en/docs/twitter-api/data-dictionary/object-model/tweet
    /// </summary>
    public record TweetResponse
    {
        public TweetModel data { get; init; } = new TweetModel();
    }
    /// <summary>
    /// Implementing only a subset of the properties available.
    /// </summary>
    public record TweetModel
    {
#pragma warning disable IDE1006 // Naming Styles
        //Twitter calls it id
        public string id { get; init; } = string.Empty;
#pragma warning restore IDE1006 // Naming Styles
        public string text { get; init; } = string.Empty;
        public DateTime created_at { get; init; } = DateTime.MinValue;
        public TweetEntitiesModel entities { get;init; } = new TweetEntitiesModel();
    }

    public record TweetEntitiesModel
    {
        public List<TweetHashTagModel> hashtags { get; init; } = new List<TweetHashTagModel>();
    }

    public record TweetHashTagModel
    {
        public int start { get; init; } = -1;
        public int end { get; init; } = -1;
        public string tag { get; init; } = string.Empty;
    }
}
