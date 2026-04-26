using Orion.API.TradingEconomics.Engine.Interfaces;
using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine
{
    /// <summary>
    /// Scores model quality using consistency, drawdown, edge and overfit risk.
    /// </summary>
    public sealed class ModelValidationEngine : IModelValidationEngine
    {
        /// <inheritdoc />
        public ModelValidationReport Validate(
            PerformanceReport performance,
            List<TradePlan> trades)
        {
            if (performance == null)
                return ModelValidationReport.Fail("Performance report is null.");

            if (trades == null || trades.Count == 0)
                return ModelValidationReport.Fail("No trades available for validation.");

            var closedTrades = trades
                .Where(x => string.Equals(x.Status, "CLOSED", StringComparison.OrdinalIgnoreCase))
                .OrderBy(x => x.ClosedAt)
                .ToList();

            if (closedTrades.Count < 30)
                return ModelValidationReport.Fail("At least 30 closed trades are required for validation.");

            var consistencyScore = CalculateConsistencyScore(closedTrades);
            var drawdownScore = CalculateDrawdownScore(performance);
            var edgeScore = CalculateEdgeScore(performance);
            var overfitScore = CalculateOverfitScore(closedTrades);

            var finalScore =
                consistencyScore * 0.30m +
                drawdownScore * 0.25m +
                edgeScore * 0.30m +
                overfitScore * 0.15m;

            var verdict = finalScore switch
            {
                >= 0.80m => "PRODUCTION_READY",
                >= 0.65m => "PAPER_TRADE_ONLY",
                >= 0.50m => "NEEDS_MORE_TESTING",
                _ => "REJECTED"
            };

            return new ModelValidationReport
            {
                IsValid = finalScore >= 0.65m,
                Score = Math.Round(finalScore, 2),
                Verdict = verdict,
                Reason =
                    $"Consistency={consistencyScore:F2}, " +
                    $"Drawdown={drawdownScore:F2}, " +
                    $"Edge={edgeScore:F2}, " +
                    $"Overfit={overfitScore:F2}, " +
                    $"Final={finalScore:F2}"
            };
        }

        private static decimal CalculateConsistencyScore(IReadOnlyList<TradePlan> trades)
        {
            var chunks = trades
                .Select((trade, index) => new { trade, index })
                .GroupBy(x => x.index / 10)
                .Select(x => x.Select(y => y.trade).ToList())
                .Where(x => x.Count >= 5)
                .ToList();

            if (chunks.Count == 0)
                return 0m;

            var profitableChunks = chunks.Count(x => x.Sum(y => y.ProfitLoss) > 0m);

            return Clamp01((decimal)profitableChunks / chunks.Count);
        }

        private static decimal CalculateDrawdownScore(PerformanceReport performance)
        {
            if (performance.NetProfit <= 0m)
                return 0m;

            if (performance.MaxDrawdown <= 0m)
                return 1m;

            var drawdownRatio = performance.MaxDrawdown / performance.NetProfit;

            if (drawdownRatio <= 0.20m) return 1.00m;
            if (drawdownRatio <= 0.35m) return 0.80m;
            if (drawdownRatio <= 0.50m) return 0.60m;
            if (drawdownRatio <= 0.75m) return 0.35m;

            return 0.10m;
        }

        private static decimal CalculateEdgeScore(PerformanceReport performance)
        {
            var score = 0m;

            if (performance.ProfitFactor >= 1.2m)
                score += 0.30m;

            if (performance.ProfitFactor >= 1.5m)
                score += 0.20m;

            if (performance.Expectancy > 0m)
                score += 0.25m;

            if (performance.WinRate >= 50m)
                score += 0.15m;

            if (performance.AverageRiskReward >= 1.2m)
                score += 0.10m;

            return Clamp01(score);
        }

        private static decimal CalculateOverfitScore(IReadOnlyCollection<TradePlan> trades)
        {
            var pnl = trades.Select(x => x.ProfitLoss).ToList();

            if (pnl.Count < 30)
                return 0m;

            var largestWin = pnl.Max();
            var totalProfit = pnl.Where(x => x > 0m).Sum();

            if (totalProfit <= 0m)
                return 0m;

            var largestWinContribution = largestWin / totalProfit;

            if (largestWinContribution <= 0.15m) return 1.00m;
            if (largestWinContribution <= 0.25m) return 0.75m;
            if (largestWinContribution <= 0.40m) return 0.50m;

            return 0.20m;
        }

        private static decimal Clamp01(decimal value)
        {
            return Math.Max(0m, Math.Min(1m, value));
        }
    }
}