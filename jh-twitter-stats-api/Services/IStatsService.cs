namespace jh_twitter_stats_api.Services
{
    public interface IStatsService
    {
        Task AddStats(StatsDTO dto);
        Task<IEnumerable<StatsDTO>> GetStats(DateTime? asOf = null);
    }
}
