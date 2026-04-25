using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Interfaces
{
    /// <summary>
    /// Interface for volatility service
    /// </summary>
    public interface IVolatilityService
    {


        Task<decimal> GetVolatilityAsync(string pair);
        Task<decimal> GetAtrAsync(string pair, int window = 14);
        Task<VolatilityMetrics> GetVolatilityMetricsAsync(string pair);
        Task<decimal> GetVolatilityAdjustedSizeAsync(string pair, decimal baseSize, decimal maxVolatility = 0.02m);
        Task<decimal> GetAtrBasedStopDistanceAsync(string pair, decimal atrMultiplier = 1.5m);
        Task<Dictionary<string, decimal>> GetVolatilityRankingAsync(IEnumerable<string> pairs);
    }
}

