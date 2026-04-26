using Orion.API.TradingEconomics.Engine;
using Orion.API.TradingEconomics.Entities;
using Xunit;

namespace Orion.API.TradingEconomics.UnitTests.Engine
{
    public sealed class HedgingEngineTests
    {
        private readonly HedgingEngine _engine = new();

        [Fact]
        public async Task AnalyzeAsync_ShouldThrow_WhenRequestIsNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _engine.AnalyzeAsync(null!));
        }

        [Fact]
        public async Task AnalyzeAsync_ShouldReturnNoHedgeRequired_WhenOpenPositionsAreNull()
        {
            var request = new HedgingRequest
            {
                OpenPositions = null!
            };

            var result = await _engine.AnalyzeAsync(request);

            Assert.Equal("NO_HEDGE_REQUIRED", result.HedgeStatus);
            Assert.Equal("No open positions supplied.", result.RiskSummary);
        }

        [Fact]
        public async Task AnalyzeAsync_ShouldReturnNoHedgeRequired_WhenOpenPositionsAreEmpty()
        {
            var request = new HedgingRequest
            {
                OpenPositions = []
            };

            var result = await _engine.AnalyzeAsync(request);

            Assert.Equal("NO_HEDGE_REQUIRED", result.HedgeStatus);
            Assert.Equal("No open positions supplied.", result.RiskSummary);
        }

        [Fact]
        public async Task AnalyzeAsync_ShouldThrow_WhenNoValidPositionsExist()
        {
            var request = new HedgingRequest
            {
                OpenPositions =
                [
                    new OpenPosition
                    {
                        Pair = "",
                        Direction = "LONG",
                        NotionalUsd = 100_000m
                    }
                ]
            };

            await Assert.ThrowsAsync<ArgumentException>(() =>
                _engine.AnalyzeAsync(request));
        }

        [Fact]
        public async Task AnalyzeAsync_ShouldCalculateExposure()
        {
            var request = new HedgingRequest
            {
                MaxHedgePercent = 50m,
                OpenPositions =
                [
                    new OpenPosition
                    {
                        Pair = "EURUSD",
                        Direction = "LONG",
                        NotionalUsd = 100_000m
                    },
                    new OpenPosition
                    {
                        Pair = "GBPUSD",
                        Direction = "SHORT",
                        NotionalUsd = 40_000m
                    }
                ]
            };

            var result = await _engine.AnalyzeAsync(request);

            Assert.Equal(100_000m, result.NetLongExposureUsd);
            Assert.Equal(40_000m, result.NetShortExposureUsd);
            Assert.Equal(60_000m, result.NetExposureUsd);
            Assert.Equal(140_000m, result.GrossExposureUsd);
            Assert.Equal(60_000m, result.HedgeRequiredUsd);
            Assert.Equal("HEDGE_REQUIRED_MODERATE", result.HedgeStatus);
        }

        [Fact]
        public async Task AnalyzeAsync_ShouldCreateShortRecommendations_WhenNetExposureIsLong()
        {
            var request = new HedgingRequest
            {
                MaxHedgePercent = 100m,
                OpenPositions =
                [
                    new OpenPosition
                    {
                        Pair = "EURUSD",
                        Direction = "LONG",
                        NotionalUsd = 100_000m
                    }
                ],
                HedgeCandidates =
                [
                    new HedgeCandidate
                    {
                        Pair = "USDCHF",
                        CorrelationToPortfolio = -0.80m,
                        LiquidityScore = 90m,
                        VolatilityPercent = 1m
                    }
                ]
            };

            var result = await _engine.AnalyzeAsync(request);

            Assert.Single(result.Recommendations);
            Assert.Equal("USDCHF", result.Recommendations[0].Pair);
            Assert.Equal("SHORT", result.Recommendations[0].Direction);
            Assert.True(result.Recommendations[0].HedgeSizeUsd > 0);
        }

        [Fact]
        public async Task AnalyzeAsync_ShouldCreateLongRecommendations_WhenNetExposureIsShort()
        {
            var request = new HedgingRequest
            {
                MaxHedgePercent = 100m,
                OpenPositions =
                [
                    new OpenPosition
                    {
                        Pair = "EURUSD",
                        Direction = "SHORT",
                        NotionalUsd = 100_000m
                    }
                ],
                HedgeCandidates =
                [
                    new HedgeCandidate
                    {
                        Pair = "USDCHF",
                        CorrelationToPortfolio = 0.80m,
                        LiquidityScore = 90m,
                        VolatilityPercent = 1m
                    }
                ]
            };

            var result = await _engine.AnalyzeAsync(request);

            Assert.Single(result.Recommendations);
            Assert.Equal("LONG", result.Recommendations[0].Direction);
        }

        [Fact]
        public async Task AnalyzeAsync_ShouldLimitRecommendationsToTopThree()
        {
            var request = new HedgingRequest
            {
                MaxHedgePercent = 100m,
                OpenPositions =
                [
                    new OpenPosition
                    {
                        Pair = "EURUSD",
                        Direction = "LONG",
                        NotionalUsd = 100_000m
                    }
                ],
                HedgeCandidates =
                [
                    CreateCandidate("USDCHF", 0.90m),
                    CreateCandidate("USDJPY", 0.80m),
                    CreateCandidate("AUDUSD", 0.70m),
                    CreateCandidate("NZDUSD", 0.60m)
                ]
            };

            var result = await _engine.AnalyzeAsync(request);

            Assert.Equal(3, result.Recommendations.Count);
        }

        [Fact]
        public async Task AnalyzeAsync_ShouldIgnoreWeakCorrelationCandidates()
        {
            var request = new HedgingRequest
            {
                MaxHedgePercent = 100m,
                OpenPositions =
                [
                    new OpenPosition
                    {
                        Pair = "EURUSD",
                        Direction = "LONG",
                        NotionalUsd = 100_000m
                    }
                ],
                HedgeCandidates =
                [
                    CreateCandidate("USDCHF", 0.10m)
                ]
            };

            var result = await _engine.AnalyzeAsync(request);

            Assert.Empty(result.Recommendations);
            Assert.Equal("HIGH_UNHEDGED_DIRECTIONAL_EXPOSURE", result.RiskSummary);
        }

        [Fact]
        public async Task AnalyzeAsync_ShouldRespectCancellationToken()
        {
            using var cts = new CancellationTokenSource();
            await cts.CancelAsync();

            var request = new HedgingRequest
            {
                MaxHedgePercent = 100m,
                OpenPositions =
                [
                    new OpenPosition
                    {
                        Pair = "EURUSD",
                        Direction = "LONG",
                        NotionalUsd = 100_000m
                    }
                ]
            };

            await Assert.ThrowsAsync<OperationCanceledException>(() =>
                _engine.AnalyzeAsync(request, cts.Token));
        }

        private static HedgeCandidate CreateCandidate(string pair, decimal correlation)
        {
            return new HedgeCandidate
            {
                Pair = pair,
                CorrelationToPortfolio = correlation,
                LiquidityScore = 90m,
                VolatilityPercent = 1m
            };
        }
    }
}