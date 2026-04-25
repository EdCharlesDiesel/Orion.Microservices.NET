using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine
{


    public sealed class PortfolioEngine
    {
        private readonly ConfigurationEngine _config;

        public PortfolioEngine(ConfigurationEngine config)
        {
            _config = config;
        }

        public PortfolioRiskResult Evaluate(
            TradePlan newTrade,
            List<TradePlan> openTrades,
            AccountContext account)
        {
            if (newTrade.Status != "OPEN")
                return PortfolioRiskResult.Block("Trade is not open.");

            if (account.Equity <= 0)
                return PortfolioRiskResult.Block("Invalid account equity.");

            var liveConfig = _config.GetConfig().LiveTrading;

            if (openTrades.Count >= liveConfig.MaxOpenTrades)
                return PortfolioRiskResult.Block("Maximum open trades reached.");

            var newTradeRisk = CalculateTradeRisk(newTrade);

            if (newTradeRisk <= 0)
                return PortfolioRiskResult.Block("Invalid new trade risk.");

            var existingRisk = openTrades.Sum(CalculateTradeRisk);
            var totalRisk = existingRisk + newTradeRisk;

            var totalRiskPercent = totalRisk / account.Equity * 100m;

            if (totalRiskPercent > liveConfig.MaxDailyLossPercent)
            {
                return PortfolioRiskResult.Block(
                    $"Portfolio risk too high: {totalRiskPercent:F2}%.");
            }

            var usdExposure = CalculateCurrencyExposure("USD", openTrades, newTrade);

            if (Math.Abs(usdExposure) > account.Equity * 3m)
            {
                return PortfolioRiskResult.Block(
                    $"USD exposure too high: {usdExposure:F2}.");
            }

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
            if (trade.PositionSize <= 0)
                return 0;

            if (trade.Direction == "LONG")
                return Math.Abs(trade.EntryPrice - trade.StopLoss) * trade.PositionSize;

            if (trade.Direction == "SHORT")
                return Math.Abs(trade.StopLoss - trade.EntryPrice) * trade.PositionSize;

            return 0;
        }

        private static decimal CalculateCurrencyExposure(
            string currency,
            List<TradePlan> openTrades,
            TradePlan newTrade)
        {
            var allTrades = openTrades.Append(newTrade);

            decimal exposure = 0;

            foreach (var trade in allTrades)
            {
                var parts = trade.Pair.Split('/');

                if (parts.Length != 2)
                    continue;

                var baseCurrency = parts[0];
                var quoteCurrency = parts[1];

                var notional = trade.EntryPrice * trade.PositionSize;

                if (trade.Direction == "LONG")
                {
                    if (baseCurrency == currency)
                        exposure += notional;

                    if (quoteCurrency == currency)
                        exposure -= notional;
                }

                if (trade.Direction == "SHORT")
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
            List<TradePlan> openTrades)
        {
            if (openTrades.Count == 0)
                return 0;

            var sameDirectionSameCurrency = 0;

            foreach (var trade in openTrades)
            {
                if (trade.Direction != newTrade.Direction)
                    continue;

                if (ShareCurrency(trade.Pair, newTrade.Pair))
                    sameDirectionSameCurrency++;
            }

            return Math.Min(1m, sameDirectionSameCurrency / 3m);
        }

        private static bool ShareCurrency(string firstPair, string secondPair)
        {
            var first = firstPair.Split('/');
            var second = secondPair.Split('/');

            if (first.Length != 2 || second.Length != 2)
                return false;

            return first.Any(second.Contains);
        }
    }
}
