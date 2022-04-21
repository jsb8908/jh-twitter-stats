namespace jh_twitter_stats_api
{
    public class TwitterAPIOptions
    {
        public const string TwitterAPI = "TwitterAPI";
        public string BaseUrl { get; set; } = String.Empty;
        public string SampleStreamEndpoint { get; set; } = String.Empty;
        public string BearerToken { get; set; } = String.Empty;
    }
}
