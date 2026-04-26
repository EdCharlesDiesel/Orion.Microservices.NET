using Orion.API.TradingEconomics.Engine.Interfaces;
using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine
{
    /// <summary>
    /// Evaluates portfolio-level limits before a new trade is accepted.
    /// </summary>
    public sealed class PortfolioEngine(ConfigurationEngine config) : IPortfolioEngine
    {
        private readonly ConfigurationEngine _config = config ?? throw new ArgumentNullException(nameof(config));

        /// <inheritdoc />
        public PortfolioRiskResult Evaluate(TradePlan newTrade, List<TradePlan> openTrades, AccountContext account)
        {
            ArgumentNullException.ThrowIfNull(newTrade);
            ArgumentNullException.ThrowIfNull(openTrades);
            ArgumentNullException.ThrowIfNull(account);

            if (!string.Equals(newTrade.Status, "OPEN", StringComparison.OrdinalIgnoreCase))
                return PortfolioRiskResult.Block("Trade is not open.");

            if (account.Equity <= 0m)
                return PortfolioRiskResult.Block("Invalid account equity.");

            var liveConfig = _config.GetConfig().LiveTrading;

            if (openTrades.Count >= liveConfig.MaxOpenTrades)
                return PortfolioRiskResult.Block("Maximum open trades reached.");

            var newTradeRisk = CalculateTradeRisk(newTrade);

            if (newTradeRisk <= 0m)
                return PortfolioRiskResult.Block("Invalid new trade risk.");

            var existingRisk = openTrades.Sum(CalculateTradeRisk);
            var totalRisk = existingRisk + newTradeRisk;
            var totalRiskPercent = totalRisk / account.Equity * 100m;

            if (totalRiskPercent > liveConfig.MaxDailyLossPercent)
                return PortfolioRiskResult.Block($"Portfolio risk too high: {totalRiskPercent:F2}%.");

            var usdExposure = CalculateCurrencyExposure("USD", openTrades, newTrade);

            if (Math.Abs(usdExposure) > account.Equity * 3m)
                return PortfolioRiskResult.Block($"USD exposure too high: {usdExposure:F2}.");

            var correlationRisk = CalculateCorrelationRisk(newTrade, openTrades);

            if (correlationRisk >= 0.80m)
                return PortfolioRiskResult.Block("Too many correlated trades.");

            return PortfolioRiskResult.Allow(
                $"Portfolio accepted. " +
                $"OpenTrades={openTrades.Count}, " +
                $"TotalRisk={totalRiskPercent:F2}%, " +
                $"UsdExposure={usdExposure:F2}, " +
                $"CorrelationRisk={correlationRisk:F2}");
        }

        private static decimal CalculateTradeRisk(TradePlan trade)
        {
            if (trade.PositionSize <= 0m)
                return 0m;

            var direction = trade.Direction?.Trim().ToUpperInvariant();

            if (direction == "LONG")
                return Math.Abs(trade.EntryPrice - trade.StopLoss) * trade.PositionSize;

            if (direction == "SHORT")
                return Math.Abs(trade.StopLoss - trade.EntryPrice) * trade.PositionSize;

            return 0m;
        }

        private static decimal CalculateCurrencyExposure(
            string currency,
            IEnumerable<TradePlan> openTrades,
            TradePlan newTrade)
        {
            var allTrades = openTrades.Append(newTrade);
            var exposure = 0m;

            foreach (var trade in allTrades)
            {
                var parts = trade.Pair
                    .Trim()
                    .ToUpperInvariant()
                    .Split('/', StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != 2)
                    continue;

                var baseCurrency = parts[0];
                var quoteCurrency = parts[1];
                var direction = trade.Direction?.Trim().ToUpperInvariant();
                var notional = trade.EntryPrice * trade.PositionSize;

                if (direction == "LONG")
                {
                    if (baseCurrency == currency)
                        exposure += notional;

                    if (quoteCurrency == currency)
                        exposure -= notional;
                }

                if (direction == "SHORT")
                {
                    if (baseCurrency == currency)
                        exposure -= notional;

                    if (quoteCurrency == currency)
                        exposure += notional;
                }
            }

            return exposure;
        }

        private static decimal CalculateCorrelationRisk(
            TradePlan newTrade,
            IEnumerable<TradePlan> openTrades)
        {
            var sameDirectionSameCurrency = openTrades.Count(trade =>
                string.Equals(trade.Direction, newTrade.Direction, StringComparison.OrdinalIgnoreCase) &&
                ShareCurrency(trade.Pair, newTrade.Pair));

            return Math.Min(1m, sameDirectionSameCurrency / 3m);
        }

        private static bool ShareCurrency(string firstPair, string secondPair)
        {
            if (string.IsNullOrWhiteSpace(firstPair) || string.IsNullOrWhiteSpace(secondPair))
                return false;

            var first = firstPair
                .Trim()
                .ToUpperInvariant()
                .Split('/', StringSplitOptions.RemoveEmptyEntries);

            var second = secondPair
                .Trim()
                .ToUpperInvariant()
                .Split('/', StringSplitOptions.RemoveEmptyEntries);

            if (first.Length != 2 || second.Length != 2)
                return false;

            return first.Any(second.Contains);
        }
    }
}