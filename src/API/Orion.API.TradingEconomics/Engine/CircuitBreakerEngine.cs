using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine
{
    public sealed class CircuitBreakerEngine(ConfigurationEngine config)
    {
        public CircuitBreakerResult Evaluate(AccountContext account, List<TradePlan> todayTrades, List<TradePlan> openTrades, DataQualityResult dataQuality)
        {
            if (account.Equity <= 0)
                return CircuitBreakerResult.Tripped("Invalid account equity.");

            if (dataQuality is { IsValid: false })
                return CircuitBreakerResult.Tripped($"Data quality failed: {dataQuality.Reason}");

            var liveConfig = config.GetConfig().LiveTrading;

            var dailyPnL = todayTrades
                .Where(x => x.Status == "CLOSED")
                .Sum(x => x.ProfitLoss);

            var dailyLossPercent = dailyPnL < 0
                ? Math.Abs(dailyPnL) / account.Equity * 100m
                : 0m;

            if (dailyLossPercent >= liveConfig.MaxDailyLossPercent)
            {
                return CircuitBreakerResult.Tripped(
                    $"Max daily loss reached: {dailyLossPercent:F2}%.");
            }

            if (openTrades.Count >= liveConfig.MaxOpenTrades)
            {
                return CircuitBreakerResult.Tripped(
                    $"Max open trades reached: {openTrades.Count}.");
            }

            var openRisk = openTrades.Sum(CalculateOpenRisk);
            var openRiskPercent = openRisk / account.Equity * 100m;

            if (openRiskPercent >= liveConfig.MaxDailyLossPercent)
            {
                return CircuitBreakerResult.Tripped(
                    $"Open portfolio risk too high: {openRiskPercent:F2}%.");
            }

            return CircuitBreakerResult.Clear(
                $"Circuit clear. DailyLoss={dailyLossPercent:F2}%, OpenRisk={openRiskPercent:F2}%.");
        }

        private static decimal CalculateOpenRisk(TradePlan trade)
        {
            if (trade.Status != "OPEN")
                return 0;

            if (trade.PositionSize <= 0)
                return 0;

            return trade.Direction switch
            {
                "LONG" => Math.Abs(trade.EntryPrice - trade.StopLoss) * trade.PositionSize,
                "SHORT" => Math.Abs(trade.StopLoss - trade.EntryPrice) * trade.PositionSize,
                _ => 0
            };
        }
    }
}
