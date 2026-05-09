using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine.Interfaces
{
    /// <summary>
    /// Creates and updates trade lifecycle plans.
    /// </summary>
    public interface ITradeLifecycleEngine
    {
        /// <summary>
        /// Creates an open trade plan from approved signal, risk, size, execution and exit data.
        /// </summary>
        TradePlan CreatePlan(SignalResult signal, RiskResult risk, PositionSizeResult size, ExecutionOrder execution, ExitPlan exit);

        /// <summary>
        /// Updates an open trade and closes it when stop loss or take profit is hit.
        /// </summary>
        TradePlan Update(TradePlan trade, OhlcvBar latestCandle);
    }
}