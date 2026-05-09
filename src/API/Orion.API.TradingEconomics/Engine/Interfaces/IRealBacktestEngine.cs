using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine.Interfaces
{
    /// <summary>
    /// Runs a candle-based backtest against portfolio positions.
    /// </summary>
    public interface IRealBacktestEngine
    {
        /// <summary>
        /// Replays candles and returns completed trade results.
        /// </summary>
        Task<List<TradeResult>> RunAsync(List<Candle> candles, List<PortfolioPosition> positions, CancellationToken cancellationToken = default);
    }
}