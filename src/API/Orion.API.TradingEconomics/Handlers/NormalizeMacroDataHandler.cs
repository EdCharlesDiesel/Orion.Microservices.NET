using MediatR;
using Orion.API.TradingEconomics.Commands;
using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Interfaces;

namespace Orion.API.TradingEconomics.Handlers
{


    public sealed class NormalizeMacroDataHandler        : IRequestHandler<NormalizeMacroDataCommand, int>
    {
        private const string CacheKey = "macro:normalized:latest";

        private readonly IRepository<EconomicIndicator> _rawRepo;
        private readonly IRepository<NormalizedIndicator> _normalizedRepo;
        private readonly INormalizationEngine _engine;
        private readonly ICacheService _cache;

        public NormalizeMacroDataHandler(
            IRepository<EconomicIndicator> rawRepo,
            IRepository<NormalizedIndicator> normalizedRepo,
            INormalizationEngine engine,
            ICacheService cache)
        {
            _rawRepo = rawRepo;
            _normalizedRepo = normalizedRepo;
            _engine = engine;
            _cache = cache;
        }

        public async Task<int> Handle(
            NormalizeMacroDataCommand request,
            CancellationToken ct)
        {
            if (!request.ForceRefresh)
            {
                var cached = await _cache.GetAsync<List<NormalizedIndicator>>(CacheKey, ct);

                if (cached is { Count: > 0 })
                    return cached.Count;
            }

            var raw = await _rawRepo.GetAllAsync();

            var normalized = _engine.Normalize(raw).ToList();

            if (normalized.Count == 0)
                return 0;

            await _normalizedRepo.AddRangeAsync(normalized);

            await _cache.SetAsync(
                CacheKey,
                normalized,
                TimeSpan.FromMinutes(30),
                ct);

            return normalized.Count;
        }
    }
}
