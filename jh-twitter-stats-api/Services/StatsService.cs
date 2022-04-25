namespace jh_twitter_stats_api.Services
{
    /// <summary>
    /// Abstracts away the repository and storage of our Stats Model
    /// Handles any Business Logic and transformation that we need
    /// </summary>
    public class StatsService : IStatsService
    {
        private readonly IStatModelRepository _statsModelRepository;
        
        public StatsService(IStatModelRepository statsModelRepository) => _statsModelRepository = statsModelRepository;

        /// <summary>
        /// Retreive all Stats object greater than the given date.
        /// If no date given, return the latest one available.
        /// NOTE: Only date is considered here. Alterntively, we could include time as well.
        /// </summary>
        /// <param name="asOf"></param>
        /// <returns></returns>
        public async Task<IEnumerable<StatsDTO>> GetStats(DateTime? asOf = null)
        {
            // always return a non-null list to the caller
            List<StatsDTO> stats = new List<StatsDTO>();
            if (asOf != null && asOf.HasValue)
            {
                //if caller has given us an asOf date, filter out older ones
                var models = await _statsModelRepository.GetAsOf(asOf.Value);
                stats = models.Select(m => StatsDTO.FromModel(m)).ToList();
            }
            else
            {
                //no asOf Date, just return the latest Stat object that we have
                var model = await _statsModelRepository.GetLatest();
                if (model != null)
                {
                    stats.Add(StatsDTO.FromModel(model));
                }
            }
            return stats;
        }

        /// <summary>
        /// Add a Stats object to our data store
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task AddStats(StatsDTO dto)
        {
            await _statsModelRepository.Add(StatsModel.FromDTO(dto));
        }
    }
}
