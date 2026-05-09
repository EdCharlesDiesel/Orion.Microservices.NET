
using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine.Interfaces
{
    /// <summary>
    /// Validates whether a trading model is suitable for production or paper trading.
    /// </summary>
    public interface IModelValidationEngine
    {
        /// <summary>
        /// Validates model performance using performance metrics and closed trades.
        /// </summary>
        ModelValidationReport Validate(
            PerformanceReport performance,
            List<TradePlan> trades);
    }
}