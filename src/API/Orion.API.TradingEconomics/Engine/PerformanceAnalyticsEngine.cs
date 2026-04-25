using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine
{
    public sealed class PerformanceAnalyticsEngine
    {
        public PerformanceReport Analyze(List<TradePlan>? trades)
        {
            if (trades == null || trades.Count == 0)
                return PerformanceReport.Empty("No trades to analyze.");

            var closedTrades = trades
                .Where(x => x.Status == "CLOSED")
                .ToList();

            if (closedTrades.Count == 0)
                return PerformanceReport.Empty("No closed trades to analyze.");

            var winningTrades = closedTrades.Where(x => x.ProfitLoss > 0).ToList();
            var losingTrades = closedTrades.Where(x => x.ProfitLoss < 0).ToList();

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

            var profitFactor = grossLoss > 0
                ? grossProfit / grossLoss
                : grossProfit > 0
                    ? 999m
                    : 0m;

            var expectancy = CalculateExpectancy(
                winRate,
                averageWin,
                averageLoss);

            var maxDrawdown = CalculateMaxDrawdown(closedTrades);

            var averageRiskReward = closedTrades
                .Where(x => x.EntryPrice > 0 && x.StopLoss > 0 && x.TakeProfit > 0)
                .Select(CalculateRiskReward)
                .DefaultIfEmpty(0m)
                .Average();

            return new PerformanceReport
            {
                TotalTrades = closedTrades.Count,
                WinningTrades = winningTrades.Count,
                LosingTrades = losingTrades.Count,
                WinRate = decimal.Round(winRate, 2),
                GrossProfit = decimal.Round(grossProfit, 2),
                GrossLoss = decimal.Round(grossLoss, 2),
                NetProfit = decimal.Round(netProfit, 2),
                AverageWin = decimal.Round(averageWin, 2),
                AverageLoss = decimal.Round(averageLoss, 2),
                ProfitFactor = decimal.Round(profitFactor, 2),
                Expectancy = decimal.Round(expectancy, 2),
                MaxDrawdown = decimal.Round(maxDrawdown, 2),
                AverageRiskReward = decimal.Round(averageRiskReward, 2),
                Verdict = BuildVerdict(
                    winRate,
                    profitFactor,
                    expectancy,
                    maxDrawdown)
            };
        }

        private static decimal CalculateExpectancy(
            decimal winRatePercent,
            decimal averageWin,
            decimal averageLoss)
        {
            var winProbability = winRatePercent / 100m;
            var lossProbability = 1m - winProbability;

            return winProbability * averageWin -
                   lossProbability * averageLoss;
        }

        private static decimal CalculateMaxDrawdown(List<TradePlan> closedTrades)
        {
            decimal equity = 0;
            decimal peak = 0;
            decimal maxDrawdown = 0;

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
            if (trade.Direction == "LONG")
            {
                var risk = trade.EntryPrice - trade.StopLoss;
                var reward = trade.TakeProfit - trade.EntryPrice;

                return risk > 0 ? reward / risk : 0;
            }

            if (trade.Direction == "SHORT")
            {
                var risk = trade.StopLoss - trade.EntryPrice;
                var reward = trade.EntryPrice - trade.TakeProfit;

                return risk > 0 ? reward / risk : 0;
            }

            return 0;
        }

        private static string BuildVerdict(
            decimal winRate,
            decimal profitFactor,
            decimal expectancy,
            decimal maxDrawdown)
        {
            if (profitFactor >= 1.8m && expectancy > 0 && winRate >= 55m)
                return "STRONG_EDGE";

            if (profitFactor >= 1.3m && expectancy > 0)
                return "POSITIVE_EDGE";

            if (profitFactor < 1.0m || expectancy <= 0)
                return "NO_EDGE";

            if (maxDrawdown > 0 && profitFactor < 1.2m)
                return "UNSTABLE_EDGE";

            return "NEUTRAL";
        }
    }
}