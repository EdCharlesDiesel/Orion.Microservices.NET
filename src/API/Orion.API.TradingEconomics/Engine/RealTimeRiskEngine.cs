using Orion.API.TradingEconomics.Engine.Interfaces;
using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Enum;

namespace Orion.API.TradingEconomics.Engine
{
    /// <summary>
    /// Performs real-time trade risk checks against account and market limits.
    /// </summary>
    public sealed class RealTimeRiskEngine : IRealTimeRiskEngine
    {
        private readonly decimal _maxAccountDrawdownPercent;
        private readonly decimal _maxPositionRiskPercent;
        private readonly decimal _maxSpreadPercent;
        private readonly decimal _maxDailyLossPercent;

        public RealTimeRiskEngine(
            decimal maxAccountDrawdownPercent = 10m,
            decimal maxPositionRiskPercent = 2m,
            decimal maxSpreadPercent = 0.15m,
            decimal maxDailyLossPercent = 5m)
        {
            _maxAccountDrawdownPercent = maxAccountDrawdownPercent;
            _maxPositionRiskPercent = maxPositionRiskPercent;
            _maxSpreadPercent = maxSpreadPercent;
            _maxDailyLossPercent = maxDailyLossPercent;
        }

        /// <inheritdoc />
        public RealTimeRiskResult Evaluate(
            AccountSnapshot account,
            ExecutionOrder execution,
            ExitPlan exitPlan,
            MarketQuote quote)
        {
            ArgumentNullException.ThrowIfNull(account);
            ArgumentNullException.ThrowIfNull(execution);
            ArgumentNullException.ThrowIfNull(exitPlan);
            ArgumentNullException.ThrowIfNull(quote);

            if (account.Balance <= 0m)
                throw new ArgumentException("Account balance must be greater than zero.", nameof(account));

            if (account.Equity <= 0m)
                throw new ArgumentException("Account equity must be greater than zero.", nameof(account));

            if (quote.Bid <= 0m || quote.Ask <= 0m || quote.Ask <= quote.Bid)
                throw new ArgumentException("Invalid market quote bid/ask.", nameof(quote));

            if (execution.ExecutedPrice <= 0m)
                throw new ArgumentException("Execution price must be greater than zero.", nameof(execution));

            if (execution.FilledSize <= 0m)
                throw new ArgumentException("Execution filled size must be greater than zero.", nameof(execution));

            var direction = execution.Direction?.Trim().ToUpperInvariant();

            if (direction is not "LONG" and not "SHORT")
                throw new ArgumentException("Execution direction must be LONG or SHORT.", nameof(execution));

            var mid = (quote.Bid + quote.Ask) / 2m;
            var spreadPercent = (quote.Ask - quote.Bid) / mid * 100m;

            var drawdownPercent = Math.Max(
                0m,
                (account.Balance - account.Equity) / account.Balance * 100m);

            var dailyLossPercent = account.RealizedPnlToday < 0m
                ? Math.Abs(account.RealizedPnlToday) / account.Balance * 100m
                : 0m;

            var stopDistance = Math.Abs(execution.ExecutedPrice - exitPlan.StopLoss);
            var positionRiskAmount = stopDistance * execution.FilledSize;
            var positionRiskPercent = positionRiskAmount / account.Equity * 100m;

            var violations = new List<string>();

            if (drawdownPercent >= _maxAccountDrawdownPercent)
                violations.Add($"Account drawdown limit breached: {drawdownPercent:F2}%.");

            if (dailyLossPercent >= _maxDailyLossPercent)
                violations.Add($"Daily loss limit breached: {dailyLossPercent:F2}%.");

            if (positionRiskPercent >= _maxPositionRiskPercent)
                violations.Add($"Position risk limit breached: {positionRiskPercent:F2}%.");

            if (spreadPercent >= _maxSpreadPercent)
                violations.Add($"Spread too wide: {spreadPercent:F4}%.");

            var action = violations.Count > 0
                ? RiskAction.BlockTrade
                : RiskAction.AllowTrade;

            return new RealTimeRiskResult
            {
                Pair = execution.Pair?.Trim().ToUpperInvariant() ?? string.Empty,
                Direction = direction,
                AccountBalance = account.Balance,
                AccountEquity = account.Equity,
                DrawdownPercent = Math.Round(drawdownPercent, 4),
                DailyLossPercent = Math.Round(dailyLossPercent, 4),
                PositionRiskAmount = Math.Round(positionRiskAmount, 2),
                PositionRiskPercent = Math.Round(positionRiskPercent, 4),
                SpreadPercent = Math.Round(spreadPercent, 4),
                Action = action,
                IsAllowed = action == RiskAction.AllowTrade,
                Violations = violations,
                TimestampUtc = DateTime.UtcNow
            };
        }
    }
}