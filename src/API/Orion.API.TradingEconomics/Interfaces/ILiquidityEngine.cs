using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Interfaces
{
    public interface ILiquidityEngine
    {
        Task<LiquidityResult> AnalyzeAsync(LiquidityRequest request,CancellationToken cancellationToken = default);
    }
}
