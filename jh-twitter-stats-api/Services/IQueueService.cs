using jh_twitter_stats_api.Models;

namespace jh_twitter_stats_api.Services
{
    public interface IQueueService
    {
        Task Add<T>(T model);
        Task<QueueModel> GetNext();
    }
}
