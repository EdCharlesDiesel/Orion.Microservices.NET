using System.Runtime.CompilerServices;
using Orion.API.TradingEconomics.Engine.Interfaces;
using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Interfaces;

namespace Orion.API.TradingEconomics.Engine
{
    /// <summary>
    /// Replays historical candles to simulate real-time market data flow.
    /// </summary>
    public sealed class MarketReplayEngine : IMarketReplayEngine
    {
        /// <inheritdoc />
        public async IAsyncEnumerable<Candle> ReplayAsync(
            IEnumerable<Candle> candles,
            int delayBetweenCandlesMs = 0,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(candles);

            if (delayBetweenCandlesMs < 0)
                throw new ArgumentOutOfRangeException(nameof(delayBetweenCandlesMs), "Delay cannot be negative.");

            var orderedCandles = candles
                .OrderBy(x => x.Time)
                .ToList();

            foreach (var candle in orderedCandles)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (delayBetweenCandlesMs > 0)
                    await Task.Delay(delayBetweenCandlesMs, cancellationToken).ConfigureAwait(false);

                yield return candle;
            }
        }

        /// <inheritdoc />
        public async IAsyncEnumerable<IReadOnlyList<Candle>> ReplayBatchesAsync(
            IEnumerable<Candle> candles,
            int batchSize = 100,
            int delayBetweenBatchesMs = 0,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(candles);

            if (batchSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(batchSize), "Batch size must be greater than 0.");

            if (delayBetweenBatchesMs < 0)
                throw new ArgumentOutOfRangeException(nameof(delayBetweenBatchesMs), "Delay cannot be negative.");

            var orderedCandles = candles
                .OrderBy(x => x.Time)
                .ToList();

            for (var i = 0; i < orderedCandles.Count; i += batchSize)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (delayBetweenBatchesMs > 0)
                    await Task.Delay(delayBetweenBatchesMs, cancellationToken).ConfigureAwait(false);

                yield return orderedCandles
                    .Skip(i)
                    .Take(batchSize)
                    .ToList();
            }
        }
    }
}