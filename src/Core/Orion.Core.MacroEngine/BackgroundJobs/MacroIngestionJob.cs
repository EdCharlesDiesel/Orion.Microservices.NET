using Orion.Core.MacroEngine.Interfaces;
using Orion.Core.MacroEngine.Models;
using Quartz;

namespace Orion.Core.MacroEngine.BackgroundJobs
{
    public class MacroIngestionJob : IJob
    {
        private readonly ITradingEconomicsClient _client;
        private readonly IRepository<EconomicIndicator> _repo;

        public MacroIngestionJob(ITradingEconomicsClient client, IRepository<EconomicIndicator> repo)
        {
            _client = client;
            _repo = repo;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var countries = new[] { "United States", "Euro Area", "Japan" };

            foreach (var country in countries)
            {
                var data = await _client.GetIndicatorsAsync(country);
                await _repo.AddRangeAsync(data);
            }
        }
    }
}
