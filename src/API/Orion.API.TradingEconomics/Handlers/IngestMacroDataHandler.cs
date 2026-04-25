
using MediatR;
using Orion.API.TradingEconomics.Commands;
using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Interfaces;

namespace Orion.API.TradingEconomics.Handlers
{
    public class IngestMacroDataHandler: IRequestHandler<IngestMacroDataCommand, int>
    {
        private readonly ITradingEconomicsClient _client;
        private readonly IRepository<EconomicIndicator> _repo;
        private readonly IIngestionValidator _validator;

        public IngestMacroDataHandler(
            ITradingEconomicsClient client,
            IRepository<EconomicIndicator> repo,
            IIngestionValidator validator)
        {
            _client = client;
            _repo = repo;
            _validator = validator;
        }

        public async Task<int> Handle(IngestMacroDataCommand request, CancellationToken ct)
        {
            // 1. Fetch raw data
            var rawData = await _client.GetIndicatorsAsync(request.Country);

            if (rawData == null || !rawData.Any())
                return 0;

            // 2. Validate + clean
            var validData = rawData
                .Where(_validator.IsValid)
                .DistinctBy(x => new { x.Country, x.Indicator, x.Date })
                .ToList();

            // 3. Idempotency check (avoid duplicates)
            var existing = await _repo.GetAllAsync();

            var newRecords = validData
                .Where(x => !existing.Any(e =>
                    e.Country == x.Country &&
                    e.Indicator == x.Indicator &&
                    e.Date == x.Date))
                .ToList();

            if (!newRecords.Any())
                return 0;

            // 4. Persist in batch
            await _repo.AddRangeAsync(newRecords);

            return newRecords.Count;
        }
    }
    }
