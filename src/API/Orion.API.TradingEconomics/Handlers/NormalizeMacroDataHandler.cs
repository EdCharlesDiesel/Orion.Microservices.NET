using MediatR;
using Orion.API.TradingEconomics.Commands;
using Orion.API.TradingEconomics.Engine.Interfaces;
using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Interfaces;

namespace Orion.API.TradingEconomics.Handlers
{


    public sealed class NormalizeMacroDataHandler(
        IRepository<EconomicIndicator> rawRepo,
        IRepository<NormalizedIndicator> normalizedRepo,
        INormalizationEngine engine,
        ICacheService cache)
        : IRequestHandler<NormalizeMacroDataCommand, int>
    {
        private const string CacheKey = "macro:normalized:latest";

        public async Task<int> Handle(
            NormalizeMacroDataCommand request,
            CancellationToken ct)
        {
            if (!request.ForceRefresh)
            {
                var cached = await cache.GetAsync<List<NormalizedIndicator>>(CacheKey, ct);

                if (cached is { Count: > 0 })
                    return cached.Count;
            }

            var raw = await rawRepo.GetAllAsync();

            var normalized = engine.Normalize(raw).ToList();

            if (normalized.Count == 0)
                return 0;

            await normalizedRepo.AddRangeAsync(normalized);

            await cache.SetAsync(
                CacheKey,
                normalized,
                TimeSpan.FromMinutes(30),
                ct);

            return normalized.Count;
        }
    }
}
