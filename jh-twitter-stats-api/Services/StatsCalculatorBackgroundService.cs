using Microsoft.Extensions.Options;
using System.Text;

namespace jh_twitter_stats_api.Services
{
    public class StatsCalculatorBackgroundService : BackgroundService
    {
        private readonly ILogger<StatsCalculatorBackgroundService> _logger;
        private readonly GeneralOptions _generalOptions;
        private readonly IQueueService _queueService;
        private readonly IStatsService _statsService;

        public StatsCalculatorBackgroundService(ILogger<StatsCalculatorBackgroundService> logger,
                                                IOptions<GeneralOptions> generalOptions,
                                                IStatsService statsService,
                                                IQueueService queueService)
        {
            _logger = logger;
            _generalOptions = generalOptions.Value;
            _queueService = queueService;
            _statsService = statsService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    // using a Queue to ensure this background service gives priority to stats calculation
                    // this is a crucial coupleing point
                    // there are two primary functions of the application:
                    // 1. Twitter API data acquisition (SampleStreamBackgroundService)
                    // 2. Stats Calculation (this service)
                    // the queue allows these two processes to scale independently (if necessary)
                    var oldestQueueObject = await _queueService.GetNext();
                    // the queue is FIFO we process in the same order received
                    if (oldestQueueObject != null)
                    {
                        // data is stored on the queue model 'generically' as JSON
                        var oldestTweetModel = JsonSerializer.Deserialize<TweetModel>(oldestQueueObject.DataAsJson);
                        if (oldestTweetModel != null)
                        {
                            var nextStatsObject = new StatsDTO()
                            {
                                AsOfTimestampUTC = oldestQueueObject.InsertTimestampUTC,
                                TweetsReceived = 1,
                                TopHashTags = new List<HashTagDTO>()
                            };

                            // _statsService.GetStats will return a list of exactly 1, the latest stats object, if no parameter is passed in
                            var lastStatsObject = (await _statsService.GetStats()).FirstOrDefault(); 
                            if (lastStatsObject != null)
                            {
                                //each stats object (StatsDTO) represent a running total of what we've seen
                                nextStatsObject.TweetsReceived += lastStatsObject.TweetsReceived;
                                nextStatsObject.TopHashTags = lastStatsObject.TopHashTags;
                                oldestTweetModel.entities.hashtags.ForEach(ht =>
                                {
                                    //normalize all tags to their Upper variant
                                    string tag = ht.tag.ToUpper();
                                    var tagExists = nextStatsObject.TopHashTags.FirstOrDefault(tht => tht.Tag == tag);
                                    if (tagExists != null)
                                    {
                                        tagExists.Count++;
                                    }
                                    else
                                    {
                                        nextStatsObject.TopHashTags = nextStatsObject.TopHashTags.Append(new HashTagDTO() { Tag = tag, Count = 1 });
                                    }
                                });
                            }
                            await _statsService.AddStats(nextStatsObject);
                        }
                    }

                    // dont be greedy, unless we have more work to do
                    int delayInMilliseconds = _generalOptions.StatsCalculatorSleepHasWorkInMilliseconds;
                    if (oldestQueueObject == null)
                    {
                        delayInMilliseconds = _generalOptions.StatsCalculatorSleepNoWorkInMilliseconds;
                    }
                    await Task.Delay(delayInMilliseconds, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
        }
    }
}
