using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine
{
    public sealed class TradeLifecycleEngine
    {
        public TradePlan CreatePlan(
            SignalResult signal,
            RiskResult risk,
            PositionSizeResult size,
            ExecutionOrder execution,
            ExitPlan exit)
        {
            if (signal.Direction == "NO_TRADE")
                return TradePlan.Rejected(signal.Reason);

            if (!risk.IsAllowed)
                return TradePlan.Rejected(risk.Reason);

            if (!size.IsAllowed)
                return TradePlan.Rejected(size.Reason);

            if (execution.FilledSize <= 0)
                return TradePlan.Rejected("Execution failed. No filled size.");

            return new TradePlan
            {
                Status = "OPEN",
                Pair = execution.Pair,
                Direction = execution.Direction,
                EntryPrice = execution.ExecutedPrice,
                PositionSize = execution.FilledSize,
                StopLoss = exit.StopLoss,
                TakeProfit = exit.TakeProfit,
                OpenedAt = execution.Timestamp,
                Reason =
                    $"{signal.Reason} | {risk.Reason} | {size.Reason}"
            };
        }

        public TradePlan Update(
            TradePlan trade,
            OhlcvBar latestCandle)
        {
            if (trade.Status != "OPEN")
                return trade;

            if (trade.Direction == "LONG")
            {
                if (latestCandle.Low <= trade.StopLoss)
                    return trade.Close("STOP_LOSS", trade.StopLoss, latestCandle.TimestampUtc);

                if (latestCandle.High >= trade.TakeProfit)
                    return trade.Close("TAKE_PROFIT", trade.TakeProfit, latestCandle.TimestampUtc);
            }

            if (trade.Direction == "SHORT")
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