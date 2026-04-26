using Orion.API.TradingEconomics.Engine.Interfaces;
using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine
{
    /// <summary>
    /// Calculates trading performance analytics from closed trade plans.
    /// </summary>
    public sealed class PerformanceAnalyticsEngine : IPerformanceAnalyticsEngine
    {
        /// <inheritdoc />
        public PerformanceReport Analyze(List<TradePlan>? trades)
        {
            if (trades == null || trades.Count == 0)
                return PerformanceReport.Empty("No trades to analyze.");

            var closedTrades = trades
                .Where(x => string.Equals(x.Status, "CLOSED", StringComparison.OrdinalIgnoreCase))
                .OrderBy(x => x.ClosedAt)
                .ToList();

            if (closedTrades.Count == 0)
                return PerformanceReport.Empty("No closed trades to analyze.");

            var winningTrades = closedTrades.Where(x => x.ProfitLoss > 0m).ToList();
            var losingTrades = closedTrades.Where(x => x.ProfitLoss < 0m).ToList();

            var grossProfit = winningTrades.Sum(x => x.ProfitLoss);
            var grossLoss = Math.Abs(losingTrades.Sum(x => x.ProfitLoss));
            var netProfit = closedTrades.Sum(x => x.ProfitLoss);

            var winRate = (decimal)winningTrades.Count / closedTrades.Count * 100m;

            var averageWin = winningTrades.Count > 0
                ? winningTrades.Average(x => x.ProfitLoss)
                : 0m;

            var averageLoss = losingTrades.Count > 0
                ? Math.Abs(losingTrades.Average(x => x.ProfitLoss))
                : 0m;

            var profitFactor = grossLoss > 0m
                ? grossProfit / grossLoss
                : grossProfit > 0m ? 999m : 0m;

            var expectancy = CalculateExpectancy(winRate, averageWin, averageLoss);
            var maxDrawdown = CalculateMaxDrawdown(closedTrades);

            var averageRiskReward = closedTrades
                .Where(x => x.EntryPrice > 0m && x.StopLoss > 0m && x.TakeProfit > 0m)
                .Select(CalculateRiskReward)
                .DefaultIfEmpty(0m)
                .Average();

            return new PerformanceReport
            {
                TotalTrades = closedTrades.Count,
                WinningTrades = winningTrades.Count,
                LosingTrades = losingTrades.Count,
                WinRate = Math.Round(winRate, 2),
                GrossProfit = Math.Round(grossProfit, 2),
                GrossLoss = Math.Round(grossLoss, 2),
                NetProfit = Math.Round(netProfit, 2),
                AverageWin = Math.Round(averageWin, 2),
                AverageLoss = Math.Round(averageLoss, 2),
                ProfitFactor = Math.Round(profitFactor, 2),
                Expectancy = Math.Round(expectancy, 2),
                MaxDrawdown = Math.Round(maxDrawdown, 2),
                AverageRiskReward = Math.Round(averageRiskReward, 2),
                Verdict = BuildVerdict(winRate, profitFactor, expectancy, maxDrawdown)
            };
        }

        private static decimal CalculateExpectancy(
            decimal winRatePercent,
            decimal averageWin,
            decimal averageLoss)
        {
            var winProbability = winRatePercent / 100m;
            var lossProbability = 1m - winProbability;

            return winProbability * averageWin - lossProbability * averageLoss;
        }

        private static decimal CalculateMaxDrawdown(IReadOnlyCollection<TradePlan> closedTrades)
        {
            var equity = 0m;
            var peak = 0m;
            var maxDrawdown = 0m;

            foreach (var trade in closedTrades.OrderBy(x => x.ClosedAt))
            {
                equity += trade.ProfitLoss;

                if (equity > peak)
                    peak = equity;

                var drawdown = peak - equity;

                if (drawdown > maxDrawdown)
                    maxDrawdown = drawdown;
            }

            return maxDrawdown;
        }

        private static decimal CalculateRiskReward(TradePlan trade)
        {
            var direction = trade.Direction?.Trim().ToUpperInvariant();

            if (direction == "LONG")
            {
                var risk = trade.EntryPrice - trade.StopLoss;
                var reward = trade.TakeProfit - trade.EntryPrice;

                return risk > 0m ? reward / risk : 0m;
            }

            if (direction == "SHORT")
            {
                var risk = trade.StopLoss - trade.EntryPrice;
                var reward = trade.EntryPrice - trade.TakeProfit;

                return risk > 0m ? reward / risk : 0m;
            }

            return 0m;
        }

        private static string BuildVerdict(
            decimal winRate,
            decimal profitFactor,
            decimal expectancy,
            decimal maxDrawdown)
        {
            if (profitFactor >= 1.8m && expectancy > 0m && winRate >= 55m)
                return "STRONG_EDGE";

            if (profitFactor >= 1.3m && expectancy > 0m)
                return "POSITIVE_EDGE";

            if (profitFactor < 1.0m || expectancy <= 0m)
                return "NO_EDGE";

            if (maxDrawdown > 0m && profitFactor < 1.2m)
                return "UNSTABLE_EDGE";

            return "NEUTRAL";
        }
    }
}