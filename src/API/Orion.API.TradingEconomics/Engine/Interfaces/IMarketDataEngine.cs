using Orion.API.TradingEconomics.DTO;
using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine.Interfaces
{
    namespace Orion.API.TradingEconomics.Engine.Interfaces
    {
        /// <summary>
        /// Provides access to macro and market data used by the trading system.
        /// </summary>
        public interface IMarketDataEngine
        {
            Task<MacroData> GetMacroDataAsync(CancellationToken cancellationToken = default);

            Task<MacroData> RefreshMacroDataAsync(CancellationToken cancellationToken = default);

            Dictionary<string, Dictionary<string, string>> GetFredSeriesMappings();

            Task<FredStatusResponse> CheckStatusAsync(CancellationToken cancellationToken = default);

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
}
