using jh_twitter_stats_api;
using jh_twitter_stats_api.Controllers;
using jh_twitter_stats_api.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace jh_twitter_stats_api_tests
{
    [TestClass]
    public class StatsControllerTest
    {
        private IStatsService _statsService;
        private StatsController _statsController;

        [TestInitialize]
        public void Initialize()
        {
            _statsService = new Mocks.MockStatsService();
            var options = Options.Create(new GeneralOptions() { MaxHashTagsReturnSize = 10, MaxStatsReturnSize = 10 });
            _statsController = new StatsController(options,_statsService);
        }

        [TestMethod]
        public async Task Test_GetStatsWithNoInput()
        {
            var stats = await _statsController.GetStats();

            Assert.IsNotNull(stats, "Stats should return non null list");

            Assert.IsTrue(stats.All(s => s.TopHashTags != null), "TopHashTags should be a non null list");

            var allHashTagLists = stats.SelectMany(s => s.TopHashTags);

            Assert.IsTrue(allHashTagLists.All(ht => ht.Count > 0), "HashTags should always return positive counts");

        }

        [TestMethod]
        public async Task Test_GetStatsWithInput()
        {
            var stats = await _statsController.GetStats();
            var statsWithAsOfDate = await _statsController.GetStats(DateTime.UtcNow.AddDays(-1));

            Assert.AreNotEqual(stats.Count(), statsWithAsOfDate.Count(), "Stats should be filtered by AsOfDate");
        }

        [TestMethod]
        public async Task Test_TotalTweets()
        {
            var totalTweets = await _statsController.GetTotalTweets();

            Assert.IsTrue(totalTweets > 0, "TotalTweet should be a positive number");
        }

        [TestMethod]
        public async Task Test_GetTopHashTagsWithNoInput()
        {
            var hashTags = await _statsController.GetTopHashTags();

            Assert.IsNotNull(hashTags, "HashTags should return non null list");

            Assert.IsTrue(hashTags.Select(ht => ht.Count).ToList().IsInDescendingOrder(), "HashTags should be returned in descending order by their count");

        }

        [TestMethod]
        public async Task Test_GetTopHashTagsWithInput()
        {
            var topHashTags = await _statsController.GetTopHashTags();
            var topNHashTags = await _statsController.GetTopHashTags(1);

            Assert.AreNotEqual(topHashTags.Count(), topNHashTags.Count(), "HashTags list should be truncated");
        }
    }
}