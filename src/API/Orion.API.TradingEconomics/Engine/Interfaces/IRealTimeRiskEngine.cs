using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine.Interfaces
{
    /// <summary>
    /// Evaluates live execution risk before allowing or blocking a trade.
    /// </summary>
    public interface IRealTimeRiskEngine
    {
        /// <summary>
        /// Checks account, position, daily loss, and spread risk.
        /// </summary>
        RealTimeRiskResult Evaluate(
            AccountSnapshot account,
            ExecutionOrder execution,
            ExitPlan exitPlan,
            MarketQuote quote);
    }
}