using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Interfaces
{
    public interface ICorrelationEngine
    {
        Task<CorrelationResult> AnalyzeAsync(CorrelationRequest request,CancellationToken cancellationToken = default);
    }
}
