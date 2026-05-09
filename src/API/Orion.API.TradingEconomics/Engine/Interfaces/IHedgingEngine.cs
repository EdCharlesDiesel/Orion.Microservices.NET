using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine.Interfaces
{
    /// <summary>
    /// Analyzes portfolio exposure and recommends hedging actions.
    /// </summary>
    public interface IHedgingEngine
    {
        /// <summary>
        /// Calculates net exposure, hedge requirement, and hedge recommendations.
        /// </summary>
        Task<HedgingResult> AnalyzeAsync(HedgingRequest request, CancellationToken cancellationToken = default);
    }
}