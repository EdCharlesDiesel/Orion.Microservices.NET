using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine
{
    public sealed class OrderManagementEngine
    {
        private readonly ConfigurationEngine _config;

        public OrderManagementEngine(ConfigurationEngine config)
        {
            _config = config;
        }

        public OrderRequest CreateOrder(
            TradePlan trade,
            PositionSizeResult size,
            AccountContext account)
        {
            if (trade.Status != "OPEN")
                return OrderRequest.Rejected("Trade is not open.");

            if (size.PositionSize <= 0)
                return OrderRequest.Rejected("Invalid position size.");

            if (!_config.IsPairEnabled(trade.Pair))
                return OrderRequest.Rejected($"Pair is disabled: {trade.Pair}");

            var liveConfig = _config.GetConfig().LiveTrading;

            if (!liveConfig.Enabled)
                return OrderRequest.Rejected("Live trading is disabled.");

            return new OrderRequest
            {
                Status = liveConfig.PaperTradingOnly ? "PAPER_ORDER" : "LIVE_ORDER",
                Pair = trade.Pair,
                Direction = trade.Direction,
                OrderType = "MARKET",
                Quantity = size.PositionSize,
                EntryPrice = trade.EntryPrice,
                StopLoss = trade.StopLoss,
                TakeProfit = trade.TakeProfit,
                CreatedAt = DateTime.UtcNow,
                Reason = trade.Reason
            };
        }

        public OrderState ValidateFill(
            OrderRequest order,
            ExecutionOrder execution)
        {
            if (order.Status == "REJECTED")
                return OrderState.Rejected(order.Reason);

            if (execution.FilledSize <= 0)
                return OrderState.Rejected("Order was not filled.");

            var fillRatio = execution.FilledSize / order.Quantity;
            var executionConfig = _config.GetExecutionConfig();

            if (fillRatio < executionConfig.MinFillRatio)
            {
                return OrderState.Rejected(
                    $"Fill ratio too low: {fillRatio:P2}.");
            }

            return new OrderState
            {
                Status = "FILLED",
                Pair = execution.Pair,
                Direction = execution.Direction,
                RequestedQuantity = order.Quantity,
                FilledQuantity = execution.FilledSize,
                AverageFillPrice = execution.ExecutedPrice,
                FilledAt = execution.Timestamp,
                Reason = "Order filled successfully."
            };
        }

        public OrderState Cancel(OrderRequest order, string reason)
        {
            return new OrderState
            {
                Status = "CANCELLED",
                Pair = order.Pair,
                Direction = order.Direction,
                RequestedQuantity = order.Quantity,
                FilledQuantity = 0,
                AverageFillPrice = 0,
                FilledAt = null,
                Reason = reason
            };
        }
    }
}