using Orion.API.TradingEconomics.Engine.Interfaces;
using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine
{
    /// <summary>
    /// Manages trade creation and open-trade exit updates.
    /// </summary>
    public sealed class TradeLifecycleEngine : ITradeLifecycleEngine
    {
        /// <inheritdoc />
        public TradePlan CreatePlan(
            SignalResult signal,
            RiskResult risk,
            PositionSizeResult size,
            ExecutionOrder execution,
            ExitPlan exit)
        {
            ArgumentNullException.ThrowIfNull(signal);
            ArgumentNullException.ThrowIfNull(risk);
            ArgumentNullException.ThrowIfNull(size);
            ArgumentNullException.ThrowIfNull(execution);
            ArgumentNullException.ThrowIfNull(exit);

            if (string.Equals(signal.Direction, "NO_TRADE", StringComparison.OrdinalIgnoreCase))
                return TradePlan.Rejected(signal.Reason);

            if (!risk.IsAllowed)
                return TradePlan.Rejected(risk.Reason);

            if (!size.IsAllowed)
                return TradePlan.Rejected(size.Reason);

            if (execution.FilledSize <= 0m)
                return TradePlan.Rejected("Execution failed. No filled size.");

            return new TradePlan
            {
                Status = "OPEN",
                Pair = execution.Pair.Trim().ToUpperInvariant(),
                Direction = execution.Direction.Trim().ToUpperInvariant(),
                EntryPrice = execution.ExecutedPrice,
                PositionSize = execution.FilledSize,
                StopLoss = exit.StopLoss,
                TakeProfit = exit.TakeProfit,
                OpenedAt = execution.Timestamp,
                Reason = $"{signal.Reason} | {risk.Reason} | {size.Reason}"
            };
        }

        /// <inheritdoc />
        public TradePlan Update(
            TradePlan trade,
            OhlcvBar latestCandle)
        {
            ArgumentNullException.ThrowIfNull(trade);
            ArgumentNullException.ThrowIfNull(latestCandle);

            if (!string.Equals(trade.Status, "OPEN", StringComparison.OrdinalIgnoreCase))
                return trade;

            var direction = trade.Direction?.Trim().ToUpperInvariant();

            if (direction == "LONG")
            {
                if (latestCandle.Low <= trade.StopLoss)
                    return trade.Close("STOP_LOSS", trade.StopLoss, latestCandle.TimestampUtc);

                if (latestCandle.High >= trade.TakeProfit)
                    return trade.Close("TAKE_PROFIT", trade.TakeProfit, latestCandle.TimestampUtc);
            }

            if (direction == "SHORT")
            {
                if (latestCandle.High >= trade.StopLoss)
                    return trade.Close("STOP_LOSS", trade.StopLoss, latestCandle.TimestampUtc);

                if (latestCandle.Low <= trade.TakeProfit)
                    return trade.Close("TAKE_PROFIT", trade.TakeProfit, latestCandle.TimestampUtc);
            }

            return trade;
        }
    }
}