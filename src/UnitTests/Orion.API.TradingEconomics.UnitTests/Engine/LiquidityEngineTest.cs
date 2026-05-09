using Orion.API.TradingEconomics.Engine;
using Orion.API.TradingEconomics.Entities;
using Xunit;

namespace Orion.API.TradingEconomics.UnitTests.Engine
{
    public sealed class LiquidityEngineTests
    {
        private readonly LiquidityEngine _engine = new();

        [Fact]
        public async Task AnalyzeAsync_ShouldReturnLiquidityResult_WhenRequestIsValid()
        {
            var request = new LiquidityRequest
            {
                Pair = "eurusd",
                RequestedSize = 100_000m,
                Bids =
                [
                    new OrderBookLevel { Price = 1.0999m, Volume = 600_000m },
                    new OrderBookLevel { Price = 1.0998m, Volume = 500_000m }
                ],
                Asks =
                [
                    new OrderBookLevel { Price = 1.1001m, Volume = 600_000m },
                    new OrderBookLevel { Price = 1.1002m, Volume = 500_000m }
                ]
            };

            var result = await _engine.AnalyzeAsync(request);

            Assert.Equal("EURUSD", result.Pair);
            Assert.Equal(1.0999m, result.BestBid);
            Assert.Equal(1.1001m, result.BestAsk);
            Assert.True(result.Spread > 0);
            Assert.True(result.TotalDepth > 0);
            Assert.Equal("INSTITUTIONAL", result.LiquidityGrade);
            Assert.Equal("LIQUIDITY_ACCEPTABLE", result.RiskSummary);
        }

        [Fact]
        public async Task AnalyzeAsync_ShouldThrowArgumentNullException_WhenRequestIsNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _engine.AnalyzeAsync(null!));
        }

        [Fact]
        public async Task AnalyzeAsync_ShouldThrowArgumentException_WhenPairIsMissing()
        {
            var request = new LiquidityRequest
            {
                Pair = "",
                Bids = [new OrderBookLevel { Price = 1.1m, Volume = 1000m }],
                Asks = [new OrderBookLevel { Price = 1.2m, Volume = 1000m }]
            };

            await Assert.ThrowsAsync<ArgumentException>(() =>
                _engine.AnalyzeAsync(request));
        }

        [Fact]
        public async Task AnalyzeAsync_ShouldThrowArgumentException_WhenBidsAreMissing()
        {
            var request = new LiquidityRequest
            {
                Pair = "EURUSD",
                Bids = [],
                Asks = [new OrderBookLevel { Price = 1.2m, Volume = 1000m }]
            };

            await Assert.ThrowsAsync<ArgumentException>(() =>
                _engine.AnalyzeAsync(request));
        }

        [Fact]
        public async Task AnalyzeAsync_ShouldThrowArgumentException_WhenAsksAreMissing()
        {
            var request = new LiquidityRequest
            {
                Pair = "EURUSD",
                Bids = [new OrderBookLevel { Price = 1.1m, Volume = 1000m }],
                Asks = []
            };

            await Assert.ThrowsAsync<ArgumentException>(() =>
                _engine.AnalyzeAsync(request));
        }

        [Fact]
        public async Task AnalyzeAsync_ShouldThrowArgumentException_WhenBestAskIsNotGreaterThanBestBid()
        {
            var request = new LiquidityRequest
            {
                Pair = "EURUSD",
                Bids = [new OrderBookLevel { Price = 1.2m, Volume = 1000m }],
                Asks = [new OrderBookLevel { Price = 1.1m, Volume = 1000m }]
            };

            await Assert.ThrowsAsync<ArgumentException>(() =>
                _engine.AnalyzeAsync(request));
        }

        [Fact]
        public async Task AnalyzeAsync_ShouldReturnThinLiquidity_WhenSpreadAndDepthArePoor()
        {
            var request = new LiquidityRequest
            {
                Pair = "USDZAR",
                RequestedSize = 100_000m,
                Bids =
                [
                    new OrderBookLevel { Price = 18.0000m, Volume = 100m }
                ],
                Asks =
                [
                    new OrderBookLevel { Price = 18.2000m, Volume = 100m }
                ]
            };

            var result = await _engine.AnalyzeAsync(request);

            Assert.Equal("THIN", result.LiquidityGrade);
            Assert.Equal("HIGH_LIQUIDITY_RISK", result.RiskSummary);
        }

        [Fact]
        public async Task AnalyzeAsync_ShouldRespectCancellationToken()
        {
            using var cts = new CancellationTokenSource();
            await cts.CancelAsync();

            var request = new LiquidityRequest
            {
                Pair = "EURUSD",
                Bids = [new OrderBookLevel { Price = 1.1m, Volume = 1000m }],
                Asks = [new OrderBookLevel { Price = 1.2m, Volume = 1000m }]
            };

            await Assert.ThrowsAsync<OperationCanceledException>(() =>
                _engine.AnalyzeAsync(request, cts.Token));
        }
    }
}