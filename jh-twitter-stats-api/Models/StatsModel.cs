namespace jh_twitter_stats_api.Models
{
    public record StatsModel
    {
        /// <summary>
        /// Count of total tweets, as of AsOfTimestampUTC
        /// </summary>
        public long TweetsReceived { get; set; }
        /// <summary>
        /// Serialized JSON string of our List of Top Hash Tags
        /// As the application is currently written (with no persistant data store) this string will continually grow
        /// as the number of hash tags encountered increases. 
        /// With a 'real' data store, we'd want to cut this last to some reasonable number
        /// </summary>
        public string TopHashTagsAsJsonString { get; set; } = "{}";
        /// <summary>
        /// Represents the time inserted into our queue, or acquired from Twitter API. 
        /// Not necessarily the creation of the tweet, but hopefully close.
        /// </summary>
        public DateTime AsOfTimestampUTC { get; set; }

        // Alternatively, we could use a library like AutoMapper (or roll our own through Reflection).
        // However, this is clean and simple.
        public static StatsModel FromDTO(StatsDTO dto)
        {
            return new StatsModel()
            {
                AsOfTimestampUTC = dto.AsOfTimestampUTC,
                TweetsReceived = dto.TweetsReceived,
                TopHashTagsAsJsonString = JsonSerializer.Serialize(dto.TopHashTags)
            };
        }
    }
}
