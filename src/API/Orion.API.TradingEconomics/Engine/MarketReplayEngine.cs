using System.Runtime.CompilerServices;
using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine
{
    public class MarketReplayEngine
    {

        /// <summary>
        /// Replays candles sequentially asynchronously, simulating real-time market data flow
        /// </summary>
        /// <param name="candles">Historical candles to replay</param>
        /// <param name="delayBetweenCandles">Optional delay between candles to simulate real-time (default: 0ms)</param>
        /// <returns>Async enumerable of candles</returns>
        public async IAsyncEnumerable<Candle> ReplayAsync(List<Candle> candles, [EnumeratorCancellation] int delayBetweenCandlesMs = 0)
        {
            if (candles == null)
                throw new ArgumentNullException(nameof(candles));

            if (!candles.Any())
                yield break;

            // Ensure candles are sorted by time
            var orderedCandles = candles
                .OrderBy(c => c.Time)
                .ToList();

            foreach (var candle in orderedCandles)
            {
                // Simulate processing delay if specified
                if (delayBetweenCandlesMs > 0)
                {
                    await Task.Delay(delayBetweenCandlesMs).ConfigureAwait(false);
                }

                yield return candle;
            }
        }

        /// <summary>
        /// Replays candles with cancellation support
        /// </summary>
        public async IAsyncEnumerable<Candle> ReplayAsync(List<Candle> candles, CancellationToken cancellationToken, [EnumeratorCancellation] int delayBetweenCandlesMs = 0)
        {
            if (candles == null)
                throw new ArgumentNullException(nameof(candles));

            if (!candles.Any())
                yield break;

            var orderedCandles = candles
                .OrderBy(c => c.Time)
                .ToList();

            foreach (var candle in orderedCandles)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (delayBetweenCandlesMs > 0)
                {
                    await Task.Delay(delayBetweenCandlesMs, cancellationToken).ConfigureAwait(false);
                }

                yield return candle;
            }
        }

        /// <summary>
        /// Replays candles in batches for performance optimization
        /// </summary>
        public async IAsyncEnumerable<IReadOnlyList<Candle>> ReplayBatchesAsync(List<Candle> candles, int batchSize = 100, [EnumeratorCancellation] int delayBetweenBatchesMs = 0)
        {
            if (candles == null)
                throw new ArgumentNullException(nameof(candles));

            if (batchSize <= 0)
                throw new ArgumentException("Batch size must be greater than 0", nameof(batchSize));

            if (!candles.Any())
                yield break;

            var orderedCandles = candles
                .OrderBy(c => c.Time)
                .ToList();

            for (int i = 0; i < orderedCandles.Count; i += batchSize)
            {
                var batch = orderedCandles
                    .Skip(i)
                    .Take(batchSize)
                    .ToList();

                if (delayBetweenBatchesMs > 0)
                {
                    await Task.Delay(delayBetweenBatchesMs).ConfigureAwait(false);
                }

                yield return batch;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="candles"></param>
        /// <returns></returns>
        public async IAsyncEnumerable<Candle> ReplayAsync(IEnumerable<Candle> candles)
        {
            foreach (var candle in candles.OrderBy(x => x.Time))
            {
                yield return candle;

                // simulate real-time flow
                await Task.Delay(1);
            }
        }

    }
}
