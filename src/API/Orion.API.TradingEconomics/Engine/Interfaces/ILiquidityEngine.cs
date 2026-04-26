using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine.Interfaces
{
    public interface ILiquidityEngine
    {
        Task<LiquidityResult> AnalyzeAsync(LiquidityRequest request,CancellationToken cancellationToken = default);
    }
}
