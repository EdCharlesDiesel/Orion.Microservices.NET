using Orion.API.TradingEconomics.DTO;
using Orion.API.TradingEconomics.Engine.Interfaces;
using Orion.API.TradingEconomics.Engine.Interfaces.Orion.API.TradingEconomics.Engine.Interfaces;
using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Interfaces;

namespace Orion.API.TradingEconomics.Engine
{
    /// <summary>
    /// Coordinates macro-data retrieval and market-data health checks.
    /// </summary>
    public sealed class MarketDataEngine(
        IFredService fredService,
        ILogger<MarketDataEngine> logger) : IMarketDataEngine
    {
        /// <inheritdoc />
        public async Task<MacroData> GetMacroDataAsync(
            CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Getting macro data");

            return await fredService.GetMacroDataAsync(cancellationToken);
        }

        /// <inheritdoc />
        public async Task<MacroData> RefreshMacroDataAsync(
            CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Refreshing macro data");

            return await fredService.RefreshMacroDataAsync(cancellationToken);
        }

        /// <inheritdoc />
        public Dictionary<string, Dictionary<string, string>> GetFredSeriesMappings()
        {
            return fredService.GetFredSeriesMappings();
        }

        /// <inheritdoc />
        public async Task<FredStatusResponse> CheckStatusAsync(
            CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Checking FRED service status");

            return await fredService.CheckStatusAsync(cancellationToken);
        }

        /// <inheritdoc />
        public Task<IReadOnlyList<OhlcvBar>> GetHistoricalCandlesAsync(
            MarketDataRequest request,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);

            if (string.IsNullOrWhiteSpace(request.Pair))
                throw new ArgumentException("Pair is required.", nameof(request));

            logger.LogInformation("Historical candle data requested for {Pair}", request.Pair);

            return Task.FromResult<IReadOnlyList<OhlcvBar>>(Array.Empty<OhlcvBar>());
        }

        /// <inheritdoc />
        public Task<MarketQuote?> GetLatestQuoteAsync(
            string pair,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(pair))
                throw new ArgumentException("Pair is required.", nameof(pair));

            logger.LogInformation("Latest quote requested for {Pair}", pair);

            return Task.FromResult<MarketQuote?>(null);
        }

        /// <inheritdoc />
        public Task<MarketDataHealth> CheckHealthAsync(
            string pair,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(pair))
                throw new ArgumentException("Pair is required.", nameof(pair));

            logger.LogInformation("Checking market data health for {Pair}", pair);

            return Task.FromResult(new MarketDataHealth
            {
                Pair = pair.Trim().ToUpperInvariant(),
                IsHealthy = true,
                Message = "FRED macro data service is available. Quote/candle provider is not configured.",
                CheckedAtUtc = DateTime.UtcNow
            });
        }
    }
}