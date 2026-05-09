using Orion.API.TradingEconomics.Engine.Interfaces;
using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine
{
    /// <summary>
    /// Engine for determining position exit conditions and calculating exit plans.
    /// </summary>
    public sealed class ExitEngine : IExitEngine
    {
        private const decimal DefaultStopDistancePercent = 0.005m;
        private const decimal DefaultRiskRewardMultiplier = 2m;
        private const decimal HighConfidenceThreshold = 0.7m;
        private const decimal TrailingStopReductionFactor = 0.75m;

        /// <inheritdoc />
        public bool ShouldExit(OpenPosition position, Candle candle, out decimal exitPrice)
        {
            ValidatePosition(position);
            ValidateCandle(candle);

            exitPrice = 0m;
            var direction = NormalizeDirection(position.Direction);

            if (IsLongPosition(direction))
            {
                return CheckLongExit(position, candle, out exitPrice);
            }
            
            return CheckShortExit(position, candle, out exitPrice);
        }

        /// <inheritdoc />
        public ExitPlan Calculate(
            SignalResult signal,
            ExecutionOrder execution,
            RiskResult risk,
            List<NormalizedIndicator>? normalized)
        {
            ValidateSignal(signal);
            ValidateExecution(execution);
            ValidateRisk(risk);

            normalized ??= new List<NormalizedIndicator>();
            var direction = NormalizeDirection(execution.Direction);
            var entryPrice = ValidateAndGetEntryPrice(execution);

            var stopDistance = GetStopDistance(risk, entryPrice);
            var takeProfitDistance = GetTakeProfitDistance(risk, stopDistance);
            
            var stopLoss = CalculateStopLoss(direction, entryPrice, stopDistance);
            var takeProfit = CalculateTakeProfit(direction, entryPrice, takeProfitDistance);
            
            var confidence = GetIndicatorValue(normalized, "CONFIDENCE");
            var trailingStopDistance = CalculateTrailingStopDistance(stopDistance, confidence);

            return CreateExitPlan(execution, direction, entryPrice, stopLoss, takeProfit, trailingStopDistance, stopDistance, takeProfitDistance);
        }

        private static void ValidatePosition(OpenPosition position)
        {
            if (position == null)
                throw new ArgumentNullException(nameof(position));
        }

        private static void ValidateCandle(Candle candle)
        {
            if (candle == null)
                throw new ArgumentNullException(nameof(candle));
        }

        private static void ValidateSignal(SignalResult signal)
        {
            if (signal == null)
                throw new ArgumentNullException(nameof(signal));
        }

        private static void ValidateExecution(ExecutionOrder execution)
        {
            if (execution == null)
                throw new ArgumentNullException(nameof(execution));
        }

        private static void ValidateRisk(RiskResult risk)
        {
            if (risk == null)
                throw new ArgumentNullException(nameof(risk));
        }

        private static string NormalizeDirection(string? direction)
        {
            var normalized = direction?.Trim().ToUpperInvariant();
            
            if (normalized is not "LONG" and not "SHORT")
                throw new ArgumentException("Direction must be LONG or SHORT.", nameof(direction));
            
            return normalized;
        }

        private static bool IsLongPosition(string direction) => direction == "LONG";

        private static bool CheckLongExit(OpenPosition position, Candle candle, out decimal exitPrice)
        {
            if (candle.Low <= position.StopLoss)
            {
                exitPrice = position.StopLoss;
                return true;
            }

            if (candle.High >= position.TakeProfit)
            {
                exitPrice = position.TakeProfit;
                return true;
            }

            exitPrice = 0m;
            return false;
        }

        private static bool CheckShortExit(OpenPosition position, Candle candle, out decimal exitPrice)
        {
            if (candle.High >= position.StopLoss)
            {
                exitPrice = position.StopLoss;
                return true;
            }

            if (candle.Low <= position.TakeProfit)
            {
                exitPrice = position.TakeProfit;
                return true;
            }

            exitPrice = 0m;
            return false;
        }

        private static decimal ValidateAndGetEntryPrice(ExecutionOrder execution)
        {
            if (execution.ExecutedPrice <= 0)
                throw new ArgumentException("Executed price must be greater than zero.", nameof(execution.ExecutedPrice));
            
            return execution.ExecutedPrice;
        }

        private static decimal GetStopDistance(RiskResult risk, decimal entryPrice)
        {
            return risk.StopLossDistance > 0
                ? risk.StopLossDistance
                : entryPrice * DefaultStopDistancePercent;
        }

        private static decimal GetTakeProfitDistance(RiskResult risk, decimal stopDistance)
        {
            return risk.TakeProfitDistance > 0
                ? risk.TakeProfitDistance
                : stopDistance * DefaultRiskRewardMultiplier;
        }

        private static decimal CalculateStopLoss(string direction, decimal entryPrice, decimal stopDistance)
        {
            return IsLongPosition(direction)
                ? entryPrice - stopDistance
                : entryPrice + stopDistance;
        }

        private static decimal CalculateTakeProfit(string direction, decimal entryPrice, decimal takeProfitDistance)
        {
            return IsLongPosition(direction)
                ? entryPrice + takeProfitDistance
                : entryPrice - takeProfitDistance;
        }

        private static decimal CalculateTrailingStopDistance(decimal stopDistance, decimal confidence)
        {
            return confidence >= HighConfidenceThreshold
                ? stopDistance * TrailingStopReductionFactor
                : stopDistance;
        }

        private static decimal GetIndicatorValue(List<NormalizedIndicator> indicators, string name)
        {
            var item = indicators.FirstOrDefault(x =>
                string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase));
            
            return item?.Value ?? 0m;
        }

        private static ExitPlan CreateExitPlan(
            ExecutionOrder execution,
            string direction,
            decimal entryPrice,
            decimal stopLoss,
            decimal takeProfit,
            decimal trailingStopDistance,
            decimal stopDistance,
            decimal takeProfitDistance)
        {
            return new ExitPlan
            {
                Pair = execution.Pair,
                Direction = direction,
                EntryPrice = entryPrice,
                StopLoss = stopLoss,
                TakeProfit = takeProfit,
                TrailingStopDistance = trailingStopDistance,
                RiskRewardRatio = stopDistance > 0 ? takeProfitDistance / stopDistance : 0m,
                CreatedAtUtc = DateTime.UtcNow
            };
        }
    }
}