using Orion.API.TradingEconomics.Engine;
using Orion.API.TradingEconomics.Entities;
using Xunit;

namespace Orion.API.TradingEconomics.UnitTests.Engine
{
    public sealed class MarketReplayEngineTests
    {
        private readonly MarketReplayEngine _engine = new();

        [Fact]
        public async Task ReplayAsync_ShouldThrow_WhenCandlesIsNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await foreach (var _ in _engine.ReplayAsync(null!))
                {
                }
            });
        }

        [Fact]
        public async Task ReplayAsync_ShouldThrow_WhenDelayIsNegative()
        {
            var candles = CreateCandles();

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
            {
                await foreach (var _ in _engine.ReplayAsync(candles, -1))
                {
                }
            });
        }

        [Fact]
        public async Task ReplayAsync_ShouldReturnCandlesOrderedByTime()
        {
            var candles = CreateCandles();

            var result = new List<Candle>();

            await foreach (var candle in _engine.ReplayAsync(candles))
            {
                result.Add(candle);
            }

            Assert.Equal(3, result.Count);
            Assert.True(result[0].Time < result[1].Time);
            Assert.True(result[1].Time < result[2].Time);
        }

        [Fact]
        public async Task ReplayAsync_ShouldReturnEmpty_WhenCandlesAreEmpty()
        {
            var result = new List<Candle>();

            await foreach (var candle in _engine.ReplayAsync([]))
            {
                result.Add(candle);
            }

            Assert.Empty(result);
        }

        [Fact]
        public async Task ReplayAsync_ShouldRespectCancellationToken()
        {
            using var cts = new CancellationTokenSource();
            await cts.CancelAsync();

            var candles = CreateCandles();

            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            {
                await foreach (var _ in _engine.ReplayAsync(candles, cancellationToken: cts.Token))
                {
                }
            });
        }

        [Fact]
        public async Task ReplayBatchesAsync_ShouldThrow_WhenCandlesIsNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await foreach (var _ in _engine.ReplayBatchesAsync(null!))
                {
                }
            });
        }

        [Fact]
        public async Task ReplayBatchesAsync_ShouldThrow_WhenBatchSizeIsInvalid()
        {
            var candles = CreateCandles();

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
            {
                await foreach (var _ in _engine.ReplayBatchesAsync(candles, 0))
                {
                }
            });
        }

        [Fact]
        public async Task ReplayBatchesAsync_ShouldThrow_WhenDelayIsNegative()
        {
            var candles = CreateCandles();

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
            {
                await foreach (var _ in _engine.ReplayBatchesAsync(candles, delayBetweenBatchesMs: -1))
                {
                }
            });
        }

        [Fact]
        public async Task ReplayBatchesAsync_ShouldReturnOrderedBatches()
        {
            var candles = CreateCandles();

            var result = new List<IReadOnlyList<Candle>>();

            await foreach (var batch in _engine.ReplayBatchesAsync(candles, batchSize: 2))
            {
                result.Add(batch);
            }

            Assert.Equal(2, result.Count);
            Assert.Equal(2, result[0].Count);
            Assert.Single(result[1]);
            Assert.True(result[0][0].Time < result[0][1].Time);
            Assert.True(result[0][1].Time < result[1][0].Time);
        }

        private static List<Candle> CreateCandles()
        {
            return
            [
                new Candle { Time = new DateTime(2024, 1, 3), Close = 1.3m },
                new Candle { Time = new DateTime(2024, 1, 1), Close = 1.1m },
                new Candle { Time = new DateTime(2024, 1, 2), Close = 1.2m }
            ];
        }
    }
}