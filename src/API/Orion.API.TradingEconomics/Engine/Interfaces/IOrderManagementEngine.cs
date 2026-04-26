using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine.Interfaces
{
    /// <summary>
    /// Creates, validates, and cancels trade orders.
    /// </summary>
    public interface IOrderManagementEngine
    {
        /// <summary>
        /// Creates an order request from an open trade plan.
        /// </summary>
        OrderRequest CreateOrder(
            TradePlan trade,
            PositionSizeResult size,
            AccountContext account);

        /// <summary>
        /// Validates an execution fill against the original order.
        /// </summary>
        OrderState ValidateFill(
            OrderRequest order,
            ExecutionOrder execution);

        /// <summary>
        /// Cancels an existing order request.
        /// </summary>
        OrderState Cancel(OrderRequest order, string reason);
    }
}