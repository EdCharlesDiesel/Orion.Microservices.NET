using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine.Interfaces
{
    /// <summary>
    /// Calculates trade position size from signal, risk, market context, and account balance.
    /// </summary>
    public interface IPositionSizingEngine
    {
        /// <summary>
        /// Calculates risk-adjusted position size using ATR-based stop distance.
        /// </summary>
        PositionSizeResult Calculate(SignalResult signal, RiskResult risk, NormalizedMarketContext market, AccountContext account);
    }
}