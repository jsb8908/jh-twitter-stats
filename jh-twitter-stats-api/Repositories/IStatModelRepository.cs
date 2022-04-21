namespace jh_twitter_stats_api.Repositories
{
    public interface IStatModelRepository
    {
        Task Add(StatsModel model);
        Task<bool> Remove(StatsModel model);
        Task<IEnumerable<StatsModel>> GetAll();
        Task<StatsModel> GetLatest();
        Task<IEnumerable<StatsModel>> GetAsOf(DateTime asOfDate);
    }
}
