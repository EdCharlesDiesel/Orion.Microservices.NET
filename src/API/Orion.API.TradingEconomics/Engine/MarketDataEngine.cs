using Orion.API.TradingEconomics.DTO;
using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Interfaces;

namespace Orion.API.TradingEconomics.Engine
{
    public sealed class MarketDataEngine(IFredService fredService, ILogger<MarketDataEngine> logger) : IMarketDataEngine

    {
        public async Task<MacroData> GetMacroDataAsync(CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Getting macro data");
            return await fredService.GetMacroDataAsync(cancellationToken);
        }

        public async Task<MacroData> RefreshMacroDataAsync(CancellationToken cancellationToken = default)
        {
            return await fredService.RefreshMacroDataAsync(cancellationToken);
        }

        public Dictionary<string, Dictionary<string, string>> GetFredSeriesMappings()
        {
            return fredService.GetFredSeriesMappings();
        }

        public async Task<FredStatusResponse> CheckStatusAsync(CancellationToken cancellationToken = default)
        {
            return await fredService.CheckStatusAsync(cancellationToken);
        }

        public Task<IReadOnlyList<OhlcvBar>> GetHistoricalCandlesAsync(MarketDataRequest request,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<MarketQuote?> GetLatestQuoteAsync(string pair, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<MarketDataHealth> CheckHealthAsync(string pair, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}