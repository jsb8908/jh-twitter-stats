
namespace jh_twitter_stats_api.Repositories
{
    public interface ITweetModelRepository
    {
        Task Add(TweetModel model);
        Task<bool> Remove(TweetModel model);
        Task<IEnumerable<TweetModel>> GetAll();
        Task<TweetModel> Get(string id);
    }
}
