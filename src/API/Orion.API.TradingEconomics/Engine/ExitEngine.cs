using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine
{
    public sealed class ExitEngine
    {
        public bool ShouldExit(OpenPosition pos, Candle candle, out decimal exitPrice)
        {
            if (pos == null)
                throw new ArgumentNullException(nameof(pos));

            if (candle == null)
                throw new ArgumentNullException(nameof(candle));

            exitPrice = 0m;

            var direction = pos.Direction?.Trim().ToUpperInvariant();

            if (direction is not "LONG" and not "SHORT")
                throw new ArgumentException("Position direction must be LONG or SHORT.", nameof(pos.Direction));

            if (direction == "LONG")
            {
                if (candle.Low <= pos.StopLoss)
                {
                    exitPrice = pos.StopLoss;
                    return true;
                }

                if (candle.High >= pos.TakeProfit)
                {
                    exitPrice = pos.TakeProfit;
                    return true;
                }
            }
            else
            {
                if (candle.High >= pos.StopLoss)
                {
                    exitPrice = pos.StopLoss;
                    return true;
                }

                if (candle.Low <= pos.TakeProfit)
                {
                    exitPrice = pos.TakeProfit;
                    return true;
                }
            }

            return false;
        }

        public ExitPlan Calculate(
            SignalResult signal,
            ExecutionOrder execution,
            RiskResult risk,
            List<NormalizedIndicator> normalized)
        {
            if (signal == null)
                throw new ArgumentNullException(nameof(signal));

            if (execution == null)
                throw new ArgumentNullException(nameof(execution));

            if (risk == null)
                throw new ArgumentNullException(nameof(risk));

            normalized ??= new List<NormalizedIndicator>();

            var direction = execution.Direction?.Trim().ToUpperInvariant();

            if (direction is not "LONG" and not "SHORT")
                throw new ArgumentException("Execution direction must be LONG or SHORT.", nameof(execution.Direction));

            if (execution.ExecutedPrice <= 0)
                throw new ArgumentException("Executed price must be greater than zero.", nameof(execution.ExecutedPrice));

            var entryPrice = execution.ExecutedPrice;

            var stopDistance = risk.StopLossDistance > 0
                ? risk.StopLossDistance
                : entryPrice * 0.005m;

            var takeProfitDistance = risk.TakeProfitDistance > 0
                ? risk.TakeProfitDistance
                : stopDistance * 2m;

            var stopLoss = direction == "LONG"
                ? entryPrice - stopDistance
                : entryPrice + stopDistance;

            var takeProfit = direction == "LONG"
                ? entryPrice + takeProfitDistance
                : entryPrice - takeProfitDistance;

            var confidence = GetIndicatorValue(normalized, "CONFIDENCE");

            var trailingStopDistance = confidence >= 0.7m
                ? stopDistance * 0.75m
                : stopDistance;

            return new ExitPlan
            {
                Pair = execution.Pair,
                Direction = direction,
                EntryPrice = entryPrice,
                StopLoss = stopLoss,
                TakeProfit = takeProfit,
                TrailingStopDistance = trailingStopDistance,
                RiskRewardRatio = stopDistance > 0
                    ? takeProfitDistance / stopDistance
                    : 0m,
                CreatedAtUtc = DateTime.UtcNow
            };
        }

        private static decimal GetIndicatorValue(
            List<NormalizedIndicator> indicators,
            string name)
        {
            var item = indicators.FirstOrDefault(x =>
                string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase));

            return item?.Value ?? 0m;
        }
    }
}