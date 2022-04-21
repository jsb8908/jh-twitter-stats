using Microsoft.Extensions.Options;
using System.Text;

namespace jh_twitter_stats_api.Services
{
    public class SampleStreamBackgroundService : BackgroundService
    {
        private readonly ILogger<SampleStreamBackgroundService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly TwitterAPIOptions _twitterAPIOptions;
        private readonly IQueueService _queueService;

        public SampleStreamBackgroundService(ILogger<SampleStreamBackgroundService> logger,
                                                IHttpClientFactory httpClient,
                                                IOptions<TwitterAPIOptions> twitterAPIOptions,
                                                IQueueService queueService)
        {
            _logger = logger;
            _httpClientFactory = httpClient;
            _twitterAPIOptions = twitterAPIOptions.Value;
            _queueService = queueService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_twitterAPIOptions.BearerToken}");
                string url = $"{_twitterAPIOptions.BaseUrl}{_twitterAPIOptions.SampleStreamEndpoint}";
                // only ask the api for the fields we need
                var response = await httpClient.GetStreamAsync($"{url}?tweet.fields=id,text,created_at,entities", stoppingToken);
                using var stream = new StreamReader(response, Encoding.UTF8);
                while (stream != null && !stream.EndOfStream && !stoppingToken.IsCancellationRequested)
                {
                    // read a line from Twitter API persistent GET stream
                    var jsonLine = await stream.ReadLineAsync();
                    if (!string.IsNullOrEmpty(jsonLine)) //sanity
                    {
                        // as long as our object shape matches, we can inflate directly into our POCOs
                        var tweetResponse = JsonSerializer.Deserialize<TweetResponse>(jsonLine);
                        // data object is guaranteed to be instantiated if tweetResponse is. See TweetModel.cs
                        if (tweetResponse != null && !string.IsNullOrEmpty(tweetResponse.data.id))
                        {
                            // using a Queue to ensure this background service gives priority to stream data acquisition
                            // this is a crucial coupleing point
                            // there are two primary functions of the application:
                            // 1. Twitter API data acquisition (this service)
                            // 2. Stats Calculation (the StatsCalcultorBackgroundService)
                            // the queue allows these two processes to scale independently (if necessary)
                            await _queueService.Add(tweetResponse.data);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
        }
    }
}
