using Orion.API.TradingEconomics.Engine.Interfaces;
using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine
{
    /// <summary>
    /// Handles order creation, fill validation, and cancellation.
    /// </summary>
    public sealed class OrderManagementEngine(ConfigurationEngine config) : IOrderManagementEngine
    {
        private readonly ConfigurationEngine _config = config ?? throw new ArgumentNullException(nameof(config));

        /// <inheritdoc />
        public OrderRequest CreateOrder(
            TradePlan trade,
            PositionSizeResult size,
            AccountContext account)
        {
            ArgumentNullException.ThrowIfNull(trade);
            ArgumentNullException.ThrowIfNull(size);
            ArgumentNullException.ThrowIfNull(account);

            if (!string.Equals(trade.Status, "OPEN", StringComparison.OrdinalIgnoreCase))
                return OrderRequest.Rejected("Trade is not open.");

            if (string.IsNullOrWhiteSpace(trade.Pair))
                return OrderRequest.Rejected("Pair is required.");

            if (string.IsNullOrWhiteSpace(trade.Direction))
                return OrderRequest.Rejected("Direction is required.");

            if (size.PositionSize <= 0m)
                return OrderRequest.Rejected("Invalid position size.");

            if (!_config.IsPairEnabled(trade.Pair))
                return OrderRequest.Rejected($"Pair is disabled: {trade.Pair}");

            var liveConfig = _config.GetConfig().LiveTrading;

            if (!liveConfig.Enabled)
                return OrderRequest.Rejected("Live trading is disabled.");

            return new OrderRequest
            {
                Status = liveConfig.PaperTradingOnly ? "PAPER_ORDER" : "LIVE_ORDER",
                Pair = trade.Pair.Trim().ToUpperInvariant(),
                Direction = trade.Direction.Trim().ToUpperInvariant(),
                OrderType = "MARKET",
                Quantity = size.PositionSize,
                EntryPrice = trade.EntryPrice,
                StopLoss = trade.StopLoss,
                TakeProfit = trade.TakeProfit,
                CreatedAt = DateTime.UtcNow,
                Reason = trade.Reason
            };
        }

        /// <inheritdoc />
        public OrderState ValidateFill(
            OrderRequest order,
            ExecutionOrder execution)
        {
            ArgumentNullException.ThrowIfNull(order);
            ArgumentNullException.ThrowIfNull(execution);

            if (string.Equals(order.Status, "REJECTED", StringComparison.OrdinalIgnoreCase))
                return OrderState.Rejected(order.Reason);

            if (order.Quantity <= 0m)
                return OrderState.Rejected("Invalid order quantity.");

            if (execution.FilledSize <= 0m)
                return OrderState.Rejected("Order was not filled.");

            var fillRatio = execution.FilledSize / order.Quantity;
            var executionConfig = _config.GetExecutionConfig();

            if (fillRatio < executionConfig.MinFillRatio)
                return OrderState.Rejected($"Fill ratio too low: {fillRatio:P2}.");

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

        /// <inheritdoc />
        public OrderState Cancel(OrderRequest order, string reason)
        {
            ArgumentNullException.ThrowIfNull(order);

            return new OrderState
            {
                Status = "CANCELLED",
                Pair = order.Pair,
                Direction = order.Direction,
                RequestedQuantity = order.Quantity,
                FilledQuantity = 0m,
                AverageFillPrice = 0m,
                FilledAt = null,
                Reason = string.IsNullOrWhiteSpace(reason)
                    ? "Order cancelled."
                    : reason
            };
        }
    }
}