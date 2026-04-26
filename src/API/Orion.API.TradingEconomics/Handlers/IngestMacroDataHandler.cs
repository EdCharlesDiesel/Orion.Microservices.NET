
using MediatR;
using Orion.API.TradingEconomics.Commands;
using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Interfaces;

namespace Orion.API.TradingEconomics.Handlers
{
    public class IngestMacroDataHandler(
        ITradingEconomicsClient client,
        IRepository<EconomicIndicator> repo,
        IIngestionValidator validator)
        : IRequestHandler<IngestMacroDataCommand, int>
    {
        public async Task<int> Handle(IngestMacroDataCommand request, CancellationToken ct)
        {
            // 1. Fetch raw data
            var rawData = await client.GetIndicatorsAsync(request.Country);

            if (rawData == null || !rawData.Any())
                return 0;

            // 2. Validate + clean
            var validData = rawData
                .Where(validator.IsValid)
                .DistinctBy(x => new { x.Country, x.Indicator, x.Date })
                .ToList();

            // 3. Idempotency check (avoid duplicates)
            var existing = await repo.GetAllAsync();

            var newRecords = validData
                .Where(x => !existing.Any(e =>
                    e.Country == x.Country &&
                    e.Indicator == x.Indicator &&
                    e.Date == x.Date))
                .ToList();

            if (!newRecords.Any())
                return 0;

            // 4. Persist in batch
            await repo.AddRangeAsync(newRecords);

            return newRecords.Count;
        }
    }
    }
