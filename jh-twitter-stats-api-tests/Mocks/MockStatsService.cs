using jh_twitter_stats_api.DTOs;
using jh_twitter_stats_api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jh_twitter_stats_api_tests.Mocks
{
    public class MockStatsService : IStatsService
    {
        public Task AddStats(StatsDTO dto)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<StatsDTO>> GetStats(DateTime? asOf = null)
        {
            var stats = new List<StatsDTO>()
            {
                new StatsDTO()
                       {
                           AsOfTimestampUTC = DateTime.UtcNow,
                           TopHashTags = new List<HashTagDTO>()
                           {
                                new HashTagDTO()
                                {
                                    Tag = "Test1",
                                    Count = 1
                                },
                                new HashTagDTO()
                                {
                                    Tag = "Test2",
                                    Count = 2
                                }
                           },
                           TweetsReceived = 100
                       },
                new StatsDTO()
                    {
                        AsOfTimestampUTC = DateTime.UtcNow.AddDays(-2),
                        TopHashTags = new List<HashTagDTO>()
                            {
                                new HashTagDTO()
                                {
                                    Tag = "Test3",
                                    Count = 10
                                },
                                new HashTagDTO()
                                {
                                    Tag = "Test4",
                                    Count = 20
                                }
                            },
                        TweetsReceived = 200
                    }
            };

            if (asOf != null && asOf.HasValue)
            {
                stats = stats.Where(s => s.AsOfTimestampUTC.Date > asOf.Value).ToList();
            }
            return stats;
        }
    }
}
