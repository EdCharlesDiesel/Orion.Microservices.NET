using Orion.API.TradingEconomics.Engine;
using Orion.API.TradingEconomics.Entities;
using Xunit;

namespace Orion.API.TradingEconomics.UnitTests.Engine
{
    public sealed class RealBacktestEngineTests
    {
        [Fact]
        public void Constructor_ShouldThrow_WhenMarketReplayEngineIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new RealBacktestEngine(null!, new ExitEngine()));
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenExitEngineIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new RealBacktestEngine(new MarketReplayEngine(), null!));
        }

        [Fact]
        public async Task RunAsync_ShouldThrow_WhenCandlesIsNull()
        {
            var engine = CreateEngine();

            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                engine.RunAsync(null!, CreatePositions()));
        }

        [Fact]
        public async Task RunAsync_ShouldThrow_WhenPositionsIsNull()
        {
            var engine = CreateEngine();

            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                engine.RunAsync(CreateCandles(), null!));
        }

        [Fact]
        public async Task RunAsync_ShouldReturnEmpty_WhenCandlesAreEmpty()
        {
            var engine = CreateEngine();

            var result = await engine.RunAsync([], CreatePositions());

            Assert.Empty(result);
        }

        [Fact]
        public async Task RunAsync_ShouldReturnEmpty_WhenPositionsAreEmpty()
        {
            var engine = CreateEngine();

            var result = await engine.RunAsync(CreateCandles(), []);

            Assert.Empty(result);
        }

        [Fact]
        public async Task RunAsync_ShouldIgnoreInvalidPositionSize()
        {
            var engine = CreateEngine();

            var result = await engine.RunAsync(CreateCandles(), [
                    new PortfolioPosition
                    {
                        Pair = "EUR/USD",
                        Direction = "LONG",
                        PositionSize = 0m
                    }
                ], TestContext.Current.CancellationToken);

            Assert.Empty(result);
        }

        [Fact]
        public async Task RunAsync_ShouldReturnTrade_WhenLongTakeProfitIsHit()
        {
            var engine = CreateEngine();

            var candles = new List<Candle>
            {
                new()
                {
                    Time = new DateTime(2024, 1, 1),
                    Open = 1.1000m,
                    High = 1.1100m,
                    Low = 1.0900m,
                    Close = 1.1000m
                },
                new()
                {
                    Time = new DateTime(2024, 1, 2),
                    Open = 1.1000m,
                    High = 1.1500m,
                    Low = 1.1000m,
                    Close = 1.1400m
                }
            };

            var result = await engine.RunAsync(candles, CreatePositions());

            Assert.NotEmpty(result);
            Assert.Equal("EUR/USD", result[0].Pair);
            Assert.True(result[0].PnL > 0m);
        }

        [Fact]
        public async Task RunAsync_ShouldRespectCancellationToken()
        {
            var engine = CreateEngine();

            using var cts = new CancellationTokenSource();
            await cts.CancelAsync();

            await Assert.ThrowsAsync<OperationCanceledException>(() =>
                engine.RunAsync(CreateCandles(), CreatePositions(), cts.Token));
        }

        private static RealBacktestEngine CreateEngine()
        {
            return new RealBacktestEngine(
                new MarketReplayEngine(),
                new ExitEngine());
        }

        private static List<PortfolioPosition> CreatePositions()
        {
            return
            [
                new PortfolioPosition
                {
                    Pair = "EUR/USD",
                    Direction = "LONG",
                    PositionSize = 10_000m
                }
            ];
        }

        private static List<Candle> CreateCandles()
        {
            return
            [
                new Candle
                {
                    Time = new DateTime(2024, 1, 1),
                    Open = 1.1000m,
                    High = 1.1100m,
                    Low = 1.0900m,
                    Close = 1.1000m
                },
                new Candle
                {
                    Time = new DateTime(2024, 1, 2),
                    Open = 1.1000m,
                    High = 1.1200m,
                    Low = 1.0800m,
                    Close = 1.1100m
                }
            ];
        }
    }
}