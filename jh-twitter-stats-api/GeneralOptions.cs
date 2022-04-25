namespace jh_twitter_stats_api
{
    public class GeneralOptions
    {
        public const string General = "General";
        /// <summary>
        /// How long we delay the stats calculation task when there is no work to do
        /// </summary>
        public int StatsCalculatorSleepNoWorkInMilliseconds { get; set; } = 100;
        /// <summary>
        /// How long we delay the stats calculation task when there is work to do
        /// </summary>
        public int StatsCalculatorSleepHasWorkInMilliseconds { get; set; } = 10;

        /// <summary>
        /// When retrieving all stat history, limit the size of the list
        /// </summary>
        public int MaxStatsReturnSize { get; set; } = 10;

        /// <summary>
        /// When retrieving all hash tags, limit the size of the list
        /// </summary>
        public int MaxHashTagsReturnSize { get; set; } = 10;
    }
}
