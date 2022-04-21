namespace jh_twitter_stats_api.Models
{
    public record QueueModel
    {
        public DateTime InsertTimestampUTC { get; init; }

        // Insert data into the queue without a definite type. 
        // This will allow us flexibilty to change it over time
        public string DataAsJson { get; init; } = String.Empty;
    }
}
