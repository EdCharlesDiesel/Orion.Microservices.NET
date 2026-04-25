using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Interfaces
{
    public interface IMarketDataEngine
    {
        Task<IReadOnlyList<OhlcvBar>> GetHistoricalCandlesAsync(
            MarketDataRequest request,
            CancellationToken cancellationToken = default);

        Task<MarketQuote?> GetLatestQuoteAsync(
            string pair,
            CancellationToken cancellationToken = default);

        Task<MarketDataHealth> CheckHealthAsync(
            string pair,
            CancellationToken cancellationToken = default);
    }
}
