using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Interfaces
{
    /// <summary>
    /// Interface for market data retrieval service
    /// </summary>
    public interface IMarketDataService
    {
        /// <summary>
        /// Gets historical OHLCV data for a trading pair
        /// </summary>
        /// <param name="pair">Trading pair (e.g., "EUR/USD")</param>
        /// <param name="timeframe">Timeframe (1m, 5m, 15m, 1h, 4h, 1d, 1w)</param>
        /// <param name="count">Number of candles to retrieve</param>
        /// <returns>List of OHLCV data</returns>
        Task<List<OhlcvBar>> GetHistoricalDataAsync(string pair, string timeframe = "1d", int count = 100);

        /// <summary>
        /// Gets latest price for a trading pair
        /// </summary>
        Task<decimal> GetLatestPriceAsync(string pair);

        /// <summary>
        /// Gets multi-timeframe data for a pair
        /// </summary>
        Task<Dictionary<string, List<OhlcvBar>>> GetMultiTimeframeDataAsync(string pair,string[] timeframes,int count = 100);

        /// <summary>
        /// Gets data for multiple pairs
        /// </summary>
        Task<Dictionary<string, List<OhlcvBar>>> GetMultiplePairsDataAsync(string[] pairs,string timeframe = "1d",int count = 100);

        /// <summary>
        /// Gets current spread for a pair
        /// </summary>
        Task<decimal> GetSpreadAsync(string pair);

        /// <summary>
        /// Gets trading sessions status
        /// </summary>
        Task<MarketSession> GetCurrentSessionAsync(string pair);

        /// <summary>
        /// Validates if market is open for a pair
        /// </summary>
        Task<bool> IsMarketOpenAsync(string pair);
    }
}
