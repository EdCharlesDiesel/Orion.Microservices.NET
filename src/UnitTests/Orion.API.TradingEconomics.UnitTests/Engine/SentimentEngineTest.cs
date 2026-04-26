using Orion.API.TradingEconomics.Engine;
using Orion.API.TradingEconomics.Entities;
using Xunit;

namespace Orion.API.TradingEconomics.UnitTests.Engine
{
    public sealed class SentimentEngineTests
    {
        private readonly SentimentEngine _engine = new();

        [Fact]
        public async Task AnalyzeAsync_ShouldThrow_WhenRequestIsNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _engine.AnalyzeAsync(null!));
        }

        [Fact]
        public async Task AnalyzeAsync_ShouldThrow_WhenPairIsMissing()
        {
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _engine.AnalyzeAsync(new SentimentRequest { Pair = "" }));
        }

        [Fact]
        public async Task AnalyzeAsync_ShouldReturnNeutral_WhenNoItemsSupplied()
        {
            var result = await _engine.AnalyzeAsync(new SentimentRequest
            {
                Pair = "eurusd",
                Items = []
            });

            Assert.Equal("EURUSD", result.Pair);
            Assert.Equal(0m, result.Score);
            Assert.Equal("NEUTRAL", result.Bias);
            Assert.Equal("No sentiment items supplied.", result.Reasons[0]);
        }

        [Fact]
        public async Task AnalyzeAsync_ShouldReturnBullish_WhenBullishWordsDominate()
        {
            var result = await _engine.AnalyzeAsync(new SentimentRequest
            {
                Pair = "EURUSD",
                Items =
                [
                    new SentimentItem
                    {
                        Source = "News",
                        Title = "Strong growth beat",
                        Text = "Risk-on rally with resilient expansion",
                        Weight = 1m
                    }
                ]
            });

            Assert.Equal("STRONGLY_BULLISH", result.Bias);
            Assert.True(result.Score > 0m);
            Assert.True(result.Confidence > 0m);
        }

        [Fact]
        public async Task AnalyzeAsync_ShouldReturnBearish_WhenBearishWordsDominate()
        {
            var result = await _engine.AnalyzeAsync(new SentimentRequest
            {
                Pair = "EURUSD",
                Items =
                [
                    new SentimentItem
                    {
                        Source = "News",
                        Title = "Weak recession risk",
                        Text = "Risk-off slowdown and crisis",
                        Weight = 1m
                    }
                ]
            });

            Assert.Equal("STRONGLY_BEARISH", result.Bias);
            Assert.True(result.Score < 0m);
        }

        [Fact]
        public async Task AnalyzeAsync_ShouldUseDefaultWeight_WhenWeightIsInvalid()
        {
            var result = await _engine.AnalyzeAsync(new SentimentRequest
            {
                Pair = "EURUSD",
                Items =
                [
                    new SentimentItem
                    {
                        Source = "News",
                        Title = "Strong growth",
                        Text = "",
                        Weight = 0m
                    }
                ]
            });

            Assert.Equal("BULLISH", result.Bias);
            Assert.Equal(0.5m, result.Score);
        }

        [Fact]
        public async Task AnalyzeAsync_ShouldLimitReasonsToTen()
        {
            var items = Enumerable.Range(1, 20)
                .Select(i => new SentimentItem
                {
                    Source = "News",
                    Title = $"Strong growth {i}",
                    Text = "beat rally",
                    Weight = 1m
                })
                .ToList();

            var result = await _engine.AnalyzeAsync(new SentimentRequest
            {
                Pair = "EURUSD",
                Items = items
            });

            Assert.Equal(10, result.Reasons.Count);
        }

        [Fact]
        public async Task AnalyzeAsync_ShouldReturnNeutral_WhenNoDirectionalWords()
        {
            var result = await _engine.AnalyzeAsync(new SentimentRequest
            {
                Pair = "EURUSD",
                Items =
                [
                    new SentimentItem
                    {
                        Source = "News",
                        Title = "Central bank update",
                        Text = "Officials released statement",
                        Weight = 1m
                    }
                ]
            });

            Assert.Equal("NEUTRAL", result.Bias);
            Assert.Equal("No strong directional sentiment detected.", result.Reasons[0]);
        }

        [Fact]
        public async Task AnalyzeAsync_ShouldRespectCancellationToken()
        {
            using var cts = new CancellationTokenSource();
            await cts.CancelAsync();

            var request = new SentimentRequest
            {
                Pair = "EURUSD",
                Items =
                [
                    new SentimentItem
                    {
                        Source = "News",
                        Title = "Strong growth",
                        Text = "beat rally",
                        Weight = 1m
                    }
                ]
            };

            await Assert.ThrowsAsync<OperationCanceledException>(() =>
                _engine.AnalyzeAsync(request, cts.Token));
        }
    }
}