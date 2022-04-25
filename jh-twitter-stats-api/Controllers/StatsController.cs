using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace jh_twitter_stats_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatsController : ControllerBase
    {
        private readonly GeneralOptions _generalOptions;
        private readonly IStatsService _statsService;

        public StatsController(IOptions<GeneralOptions> generalOptions,
                              IStatsService statsService)
        {
            _generalOptions = generalOptions.Value;
            _statsService = statsService;
        }

        [HttpGet("GetCurrentStats")]
        public async Task<IEnumerable<StatsDTO>> GetStats()
        {
            // list has one Stat object, the most recent one
            return await _statsService.GetStats();
        }

        [HttpGet("GetStatsAsOf/{asOfDate:DateTime}")]
        public async Task<IEnumerable<StatsDTO>> GetStats(DateTime asOfDate)
        {
            // limit the size of the returned list....sanity check the app setting parameter
            int toTake = _generalOptions.MaxStatsReturnSize > 0 ? _generalOptions.MaxStatsReturnSize : 10;
            // return the list in descending order...most recent first
            return (await _statsService.GetStats(asOfDate)).OrderByDescending(s => s.AsOfTimestampUTC).Take(toTake); 
        }

        [HttpGet("GetTotalTweets")]
        public async Task<long> GetTotalTweets()
        {
            // grab the latest stat object
            var latestStats = await _statsService.GetStats();
            if (!latestStats.Any())
            {
                return 0;
            }

            // First() is valid (not null) here because of the Any() above
            return latestStats.First().TweetsReceived;
        }


        [HttpGet("GetTopHashTags/{top:int?}")]
        public async Task<IEnumerable<HashTagDTO>> GetTopHashTags(int top = -1)
        {
            // we only need the latest Stat object
            var latestStats = await _statsService.GetStats();
            if (!latestStats.Any())
            {
                // always return a non-null list to the caller
                return new List<HashTagDTO>();
            }

            // limit the size of the returned list...sanity check the app setting
            top = top > 0 ? top : (_generalOptions.MaxHashTagsReturnSize > 0 ? _generalOptions.MaxHashTagsReturnSize : 10);

            // return the list in descending order by the most popular hash tags
            return latestStats.First().TopHashTags
                                      .OrderByDescending(tht => tht.Count)
                                      .Take(top);
        }
    }
}