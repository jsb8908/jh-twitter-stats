using Microsoft.AspNetCore.Mvc;

namespace jh_twitter_stats_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatsController : ControllerBase
    {
        private readonly ILogger<StatsController> _logger;
        private readonly IStatsService _statsService;

        public StatsController(ILogger<StatsController> logger,
                              IStatsService statsService)
        {
            _logger = logger;
            _statsService = statsService;
        }

        [HttpGet("GetCurrentStats")]
        public async Task<IEnumerable<StatsDTO>> GetStats()
        {
            return await _statsService.GetStats();
        }

        [HttpGet("GetStatsAsOf/{asOfDate:DateTime}")]
        public async Task<IEnumerable<StatsDTO>> GetStats(DateTime asOfDate)
        {
            return (await _statsService.GetStats(asOfDate)).OrderByDescending(s => s.AsOfTimestampUTC).Take(10); // limit the return
        }

        [HttpGet("GetTotalTweets")]
        public async Task<long> GetTotalTweets()
        {
            var latestStats = await _statsService.GetStats();
            if (!latestStats.Any())
            {
                return 0;
            }

            return latestStats.First().TweetsReceived;
        }


        [HttpGet("GetTopHashTags/{top:int?}")]
        public async Task<IEnumerable<HashTagDTO>> GetTopHashTags(int top = 10)
        {
            // we only need the latest Stat object
            var latestStats = await _statsService.GetStats();
            if (!latestStats.Any())
            {
                // always return a non-null list to the caller
                return new List<HashTagDTO>();
            }
            
            // return the list in descending order by the most popular hash tags
            return latestStats.First().TopHashTags
                                      .OrderByDescending(tht => tht.Count)
                                      .Take(top);
        }
    }
}