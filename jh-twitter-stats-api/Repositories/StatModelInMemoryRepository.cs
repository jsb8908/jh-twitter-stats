//disable this warning....keeping the async modifier on these methods for consistency with any other concrete implementations (EF)
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace jh_twitter_stats_api.Repositories
{
    /// <summary>
    /// Simple In-Memory Repository for storing Stat objects.
    /// This class is a singleton.
    /// </summary>
    public class StatModelInMemoryRepository : IStatModelRepository
    {
        // this class is a singleton, there is only one _stats List
        List<StatsModel> _stats = new List<StatsModel>();
        public async Task Add(StatsModel model) => _stats.Add(model);
        public async Task<bool> Remove(StatsModel model) => _stats.Remove(model);
        // Because it's a List<T> and the Add() method above, I know this is true.
        // Obviously, when dealing with a 'real' data store, this would be different and even with the List<T> it's probably safer (if not more expensive)
        // to OrderBy first....yea I'm cheating a little :)
        public async Task<StatsModel> GetLatest() => _stats.LastOrDefault(); 
        /// <summary>
        /// asOfDate is inclusive such that '>='
        /// </summary>
        /// <param name="asOfDate"></param>
        /// <returns></returns>
        public async Task<IEnumerable<StatsModel>> GetAsOf(DateTime asOfDate) => _stats.Where(s =>s.AsOfTimestampUTC.Date >= asOfDate.Date);
        public async Task<IEnumerable<StatsModel>> GetAll() => _stats;
    }
}
