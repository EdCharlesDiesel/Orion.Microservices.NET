using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine.Interfaces
{
    public interface ICorrelationEngine
    {
        Task<CorrelationResult> AnalyzeAsync(CorrelationRequest request,CancellationToken cancellationToken = default);
    }
}
