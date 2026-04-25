using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Interfaces
{
    public interface IHedgingEngine
    {
        Task<HedgingResult> AnalyzeAsync(HedgingRequest request,CancellationToken cancellationToken = default);
    }
}
