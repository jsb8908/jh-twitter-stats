using System.Collections.Concurrent;

//disable this warning....keeping the async modifier on these methods for consistency with any other concrete implementations (Azure Queues, NServiceBus, etc..)
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace jh_twitter_stats_api.Services
{
    /// <summary>
    /// Thread safe, FIFO, In-Memory Queue.
    /// </summary>
    public class QueueService : IQueueService
    {
        // ConcurrentQueue ensures thread saftety with our Background processing
        // marked static because this Service is transient. Its DI'd per controller/action
        static ConcurrentQueue<QueueModel> _queue = new ConcurrentQueue<QueueModel>();

        /// <summary>
        /// Serializes an object as JSON and enqueues it
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task Add<T>(T model) => _queue.Enqueue(new QueueModel()
        {
            DataAsJson = JsonSerializer.Serialize(model),
            InsertTimestampUTC = DateTime.UtcNow
        });

        /// <summary>
        /// Returns null if no item retrieved.
        /// Caller needs to check return validity
        /// </summary>
        /// <returns></returns>
        public async Task<QueueModel> GetNext()
        {
            QueueModel model;
            _queue.TryDequeue(out model);
            return model;
        }
    }
}
