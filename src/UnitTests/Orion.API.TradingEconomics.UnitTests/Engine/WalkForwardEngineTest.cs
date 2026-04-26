using Orion.API.TradingEconomics.Engine;
using Xunit;

namespace Orion.API.TradingEconomics.UnitTests.Engine
{
    public sealed class WalkForwardEngineTests
    {
        [Fact]
        public void Constructor_ShouldThrow_WhenBacktestEngineIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new WalkForwardEngine(null!));
        }

        [Fact]
        public async Task RunAsync_ShouldThrow_WhenEndDateIsBeforeStartDate()
        {
            var engine = CreateEngine();

            await Assert.ThrowsAsync<ArgumentException>(() =>
                engine.RunAsync(new DateTime(2024, 1, 2), new DateTime(2024, 1, 1), TestContext.Current.CancellationToken));
        }

        [Fact]
        public async Task RunAsync_ShouldReturnEmpty_WhenRangeIsLessThanWindow()
        {
            var engine = CreateEngine();

            var result = await engine.RunAsync(new DateTime(2024, 1, 1), new DateTime(2024, 2, 1), TestContext.Current.CancellationToken);

            Assert.Empty(result);
        }

        [Fact]
        public async Task RunAsync_ShouldReturnWalkForwardWindows()
        {
            var engine = CreateEngine();

            var result = await engine.RunAsync(new DateTime(2024, 1, 1), new DateTime(2024, 7, 1), TestContext.Current.CancellationToken);

            Assert.NotEmpty(result);

            Assert.Equal(new DateTime(2024, 1, 1), result[0].TrainStart);
            Assert.Equal(new DateTime(2024, 3, 1), result[0].TrainEnd);
            Assert.Equal(new DateTime(2024, 3, 1), result[0].TestStart);
            Assert.Equal(new DateTime(2024, 3, 31), result[0].TestEnd);
        }

        [Fact]
        public async Task RunAsync_ShouldRespectCancellationToken()
        {
            var engine = CreateEngine();

            using var cts = new CancellationTokenSource();
            await cts.CancelAsync();

            await Assert.ThrowsAsync<OperationCanceledException>(() =>
                engine.RunAsync(
                    new DateTime(2024, 1, 1),
                    new DateTime(2024, 7, 1),
                    cts.Token));
        }

        private static WalkForwardEngine CreateEngine()
        {
            return new WalkForwardEngine(new BacktestEngine());
        }
    }
}