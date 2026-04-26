

using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine.Interfaces
{
    /// <summary>
    /// Coordinates full live trading pipeline from data → trade decision.
    /// </summary>
    public interface ILiveTradingOrchestrator
    {
        /// <summary>
        /// Executes full trading flow and returns final decision.
        /// </summary>
        LiveTradingResult Run(ForexMarketInput input, AccountContext account, OrderBook orderBook);
    }
}