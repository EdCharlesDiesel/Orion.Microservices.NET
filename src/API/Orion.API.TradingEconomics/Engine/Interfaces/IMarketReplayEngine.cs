using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine.Interfaces
{
    /// <summary>
    /// Replays historical candles sequentially or in batches.
    /// </summary>
    public interface IMarketReplayEngine
    {
        /// <summary>
        /// Replays candles sequentially in timestamp order.
        /// </summary>
        IAsyncEnumerable<Candle> ReplayAsync(
            IEnumerable<Candle> candles,
            int delayBetweenCandlesMs = 0,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Replays candles in timestamp-ordered batches.
        /// </summary>
        IAsyncEnumerable<IReadOnlyList<Candle>> ReplayBatchesAsync(
            IEnumerable<Candle> candles,
            int batchSize = 100,
            int delayBetweenBatchesMs = 0,
            CancellationToken cancellationToken = default);
    }
}