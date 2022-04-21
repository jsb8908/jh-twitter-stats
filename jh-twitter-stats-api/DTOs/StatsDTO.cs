namespace jh_twitter_stats_api.DTOs
{
    /// <summary>
    /// Used to send data to the client / calling party
    /// Mostly mapped from our StatsModel.cs, with any business logic or transformations applied
    /// </summary>
    public class StatsDTO
    {
        /// <summary>
        /// <see cref="StatsModel.TweetsReceived"/>.
        /// </summary>
        public long TweetsReceived { get; set; }
        /// <summary>
        /// Our dynamic type for the JSON string here: <see cref="StatsModel.TopHashTagsAsJsonString"/>.
        /// </summary>
        public IEnumerable<HashTagDTO> TopHashTags { get; set; } = new List<HashTagDTO>();
        /// <summary>
        /// <see cref="StatsModel.AsOfTimestampUTC"/>.
        /// </summary>
        public DateTime AsOfTimestampUTC { get; set; }

        // Alternatively, we could use a library like AutoMapper (or roll our own through Reflection).
        // However, this is clean and simple.
        public static StatsDTO FromModel(StatsModel model)
        {
            return new StatsDTO()
            {
                TweetsReceived = model.TweetsReceived,
                AsOfTimestampUTC = model.AsOfTimestampUTC,
                TopHashTags = JsonSerializer.Deserialize<List<HashTagDTO>>(model.TopHashTagsAsJsonString ?? "{}")
            };
        }
    }

    /// <summary>
    /// Helper DTO for the front end.
    /// </summary>
    public class HashTagDTO
    {
        public string Tag { get; set; } = string.Empty;
        public int Count { get; set; } = 0;
    }
}
