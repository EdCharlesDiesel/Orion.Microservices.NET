using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Interfaces
{
    public interface IMarketDataProvider
    {
        string Name { get; }

        Task<IReadOnlyList<OhlcvBar>> GetHistoricalCandlesAsync(MarketDataRequest request, CancellationToken cancellationToken = default);

        Task<MarketQuote?> GetLatestQuoteAsync(string pair, CancellationToken cancellationToken = default);
    }
}
