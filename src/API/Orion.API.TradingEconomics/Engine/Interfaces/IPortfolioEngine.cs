using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine.Interfaces
{
    /// <summary>
    /// Evaluates portfolio-level risk before accepting a new trade.
    /// </summary>
    public interface IPortfolioEngine
    {
        /// <summary>
        /// Checks risk, exposure, open-trade limits and correlation before allowing a trade.
        /// </summary>
        PortfolioRiskResult Evaluate(TradePlan newTrade, List<TradePlan> openTrades, AccountContext account);
    }
}