using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Enum;


namespace Orion.API.TradingEconomics.Engine
{
    public sealed class RealTimeRiskEngine
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

        public RealTimeRiskResult Evaluate(
            AccountSnapshot account,
            ExecutionOrder execution,
            ExitPlan exitPlan,
            MarketQuote quote)
        {
            if (account == null)
                throw new ArgumentNullException(nameof(account));

            if (execution == null)
                throw new ArgumentNullException(nameof(execution));

            if (exitPlan == null)
                throw new ArgumentNullException(nameof(exitPlan));

            if (quote == null)
                throw new ArgumentNullException(nameof(quote));

            if (account.Balance <= 0)
                throw new ArgumentException("Account balance must be greater than zero.", nameof(account.Balance));

            if (account.Equity <= 0)
                throw new ArgumentException("Account equity must be greater than zero.", nameof(account.Equity));

            if (quote.Bid <= 0 || quote.Ask <= 0)
                throw new ArgumentException("Invalid market quote bid/ask.", nameof(quote));

            var direction = execution.Direction?.Trim().ToUpperInvariant();

            if (direction is not "LONG" and not "SHORT")
                throw new ArgumentException("Execution direction must be LONG or SHORT.", nameof(execution.Direction));

            var mid = (quote.Bid + quote.Ask) / 2m;
            var spread = quote.Ask - quote.Bid;
            var spreadPercent = mid > 0 ? spread / mid * 100m : 0m;

            var drawdownPercent = account.Balance > 0
                ? (account.Balance - account.Equity) / account.Balance * 100m
                : 0m;

            var dailyLossPercent = account.Balance > 0
                ? Math.Abs(account.RealizedPnlToday < 0 ? account.RealizedPnlToday : 0m) / account.Balance * 100m
                : 0m;

            var stopDistance = Math.Abs(execution.ExecutedPrice - exitPlan.StopLoss);

            var positionRiskAmount = stopDistance * execution.FilledSize;

            var positionRiskPercent = account.Equity > 0
                ? positionRiskAmount / account.Equity * 100m
                : 0m;

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
                Pair = execution.Pair,
                Direction = direction,
                AccountBalance = account.Balance,
                AccountEquity = account.Equity,
                DrawdownPercent = drawdownPercent,
                DailyLossPercent = dailyLossPercent,
                PositionRiskAmount = positionRiskAmount,
                PositionRiskPercent = positionRiskPercent,
                SpreadPercent = spreadPercent,
                Action = action,
                IsAllowed = action == RiskAction.AllowTrade,
                Violations = violations,
                TimestampUtc = DateTime.UtcNow
            };
        }
    }
}