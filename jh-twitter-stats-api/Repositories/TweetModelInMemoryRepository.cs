//disable this warning....keeping the async modifier on these methods for consistency with any other concrete implementations (EF)
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace jh_twitter_stats_api.Repositories
{
    /// <summary>
    /// Simple In-Memory Repository for storing Tweet objects.
    /// This class is a singleton.
    /// This implementation (and the interface generally) is not being used. I've included as an example of how to (naively) store
    /// the incoming tweets being received. 
    /// A 'real' implementation would involve a persistant data store (SQL Server, Postgres, etc...)
    /// </summary>
    public class TweetModelInMemoryRepository : ITweetModelRepository
    {
        // this class is a singleton, there is only one _tweets List
        List<TweetModel> _tweets = new List<TweetModel>();
        public async Task Add(TweetModel model) => _tweets.Add(model);
        public async Task<bool> Remove(TweetModel model) => _tweets.Remove(model);
        public async Task<TweetModel> Get(string id) => _tweets.First(t => t.id == id);
        public async Task<IEnumerable<TweetModel>> GetAll() => _tweets;
    }
}
