using Orion.API.TradingEconomics.Engine;
using Orion.API.TradingEconomics.Entities;
using Xunit;

namespace Orion.API.TradingEconomics.UnitTests.Engine
{
    public class CorrelationEngineTests
    {
        private readonly CorrelationEngine _engine;

        public CorrelationEngineTests()
        {
            _engine = new CorrelationEngine();
        }

        [Fact]
        public async Task AnalyzeAsync_WithNullRequest_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(
                () => _engine.AnalyzeAsync(null));
        }

        [Fact]
        public async Task AnalyzeAsync_WithEmptyPrimaryPair_ThrowsArgumentException()
        {
            // Arrange
            var request = new CorrelationRequest
            {
                PrimaryPair = "",
                Series = new List<DataSeries>
                {
                    new DataSeries { Symbol = "EURUSD", Values = new List<decimal> { 1.0m, 1.1m } }
                }
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _engine.AnalyzeAsync(request));
            Assert.Contains("Primary pair", exception.Message);
        }

        [Fact]
        public async Task AnalyzeAsync_WithLessThanTwoSeries_ThrowsArgumentException()
        {
            // Arrange
            var request = new CorrelationRequest
            {
                PrimaryPair = "EURUSD",
                Series = new List<DataSeries>
                {
                    new DataSeries { Symbol = "EURUSD", Values = new List<decimal> { 1.0m, 1.1m } }
                }
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _engine.AnalyzeAsync(request));
            Assert.Contains("at least two series", exception.Message);
        }

        [Fact]
        public async Task AnalyzeAsync_WithMissingPrimarySeries_ThrowsArgumentException()
        {
            // Arrange
            var request = new CorrelationRequest
            {
                PrimaryPair = "EURUSD",
                Series = new List<DataSeries>
                {
                    new DataSeries { Symbol = "GBPUSD", Values = new List<decimal> { 1.2m, 1.3m } },
                    new DataSeries { Symbol = "USDJPY", Values = new List<decimal> { 110.0m, 111.0m } }
                }
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _engine.AnalyzeAsync(request));
            Assert.Contains("Primary pair series was not supplied", exception.Message);
        }

        [Fact]
        public async Task AnalyzeAsync_WithPerfectPositiveCorrelation_ReturnsCorrectResult()
        {
            // Arrange
            var request = new CorrelationRequest
            {
                PrimaryPair = "EURUSD",
                Series = new List<DataSeries>
                {
                    new DataSeries { Symbol = "EURUSD", Values = new List<decimal> { 1.0m, 2.0m, 3.0m, 4.0m, 5.0m } },
                    new DataSeries { Symbol = "GBPUSD", Values = new List<decimal> { 1.0m, 2.0m, 3.0m, 4.0m, 5.0m } }
                }
            };

            // Act
            var result = await _engine.AnalyzeAsync(request);

            // Assert
            Assert.Single(result.Correlations);
            Assert.Equal(1.0m, result.Correlations[0].Correlation);
            Assert.Equal("VERY_STRONG", result.Correlations[0].Strength);
            Assert.Equal("POSITIVE", result.Correlations[0].Direction);
        }

        [Fact]
        public async Task AnalyzeAsync_WithPerfectNegativeCorrelation_ReturnsCorrectResult()
        {
            // Arrange
            var request = new CorrelationRequest
            {
                PrimaryPair = "EURUSD",
                Series = new List<DataSeries>
                {
                    new DataSeries { Symbol = "EURUSD", Values = new List<decimal> { 1.0m, 2.0m, 3.0m, 4.0m, 5.0m } },
                    new DataSeries { Symbol = "GBPUSD", Values = new List<decimal> { 5.0m, 4.0m, 3.0m, 2.0m, 1.0m } }
                }
            };

            // Act
            var result = await _engine.AnalyzeAsync(request);

            // Assert
            Assert.Single(result.Correlations);
            Assert.Equal(-1.0m, result.Correlations[0].Correlation);
            Assert.Equal("VERY_STRONG", result.Correlations[0].Strength);
            Assert.Equal("NEGATIVE", result.Correlations[0].Direction);
        }

        [Fact]
        public async Task AnalyzeAsync_WithMultipleSeries_ReturnsAllCorrelations()
        {
            // Arrange
            var request = new CorrelationRequest
            {
                PrimaryPair = "EURUSD",
                Series = new List<DataSeries>
                {
                    new DataSeries { Symbol = "EURUSD", Values = new List<decimal> { 1.0m, 2.0m, 3.0m, 4.0m, 5.0m } },
                    new DataSeries { Symbol = "GBPUSD", Values = new List<decimal> { 1.0m, 2.0m, 3.0m, 4.0m, 5.0m } },
                    new DataSeries { Symbol = "USDJPY", Values = new List<decimal> { 5.0m, 4.0m, 3.0m, 2.0m, 1.0m } }
                }
            };

            // Act
            var result = await _engine.AnalyzeAsync(request);

            // Assert
            Assert.Equal(2, result.Correlations.Count);
        }

        [Fact]
        public async Task AnalyzeAsync_WithLookbackPeriod_LimitsDataPoints()
        {
            // Arrange
            var request = new CorrelationRequest
            {
                PrimaryPair = "EURUSD",
                LookbackPeriods = 3,
                Series = new List<DataSeries>
                {
                    new DataSeries { Symbol = "EURUSD", Values = new List<decimal> { 1.0m, 2.0m, 3.0m, 4.0m, 5.0m, 6.0m } },
                    new DataSeries { Symbol = "GBPUSD", Values = new List<decimal> { 1.0m, 2.0m, 3.0m, 4.0m, 5.0m, 6.0m } }
                }
            };

            // Act
            var result = await _engine.AnalyzeAsync(request);

            // Assert
            Assert.Single(result.Correlations);
            Assert.Equal(1.0m, result.Correlations[0].Correlation);
        }

        [Fact]
        public async Task AnalyzeAsync_WithInsufficientValues_SkipsSeries()
        {
            // Arrange
            var request = new CorrelationRequest
            {
                PrimaryPair = "EURUSD",
                Series = new List<DataSeries>
                {
                    new DataSeries { Symbol = "EURUSD", Values = new List<decimal> { 1.0m, 2.0m, 3.0m } },
                    new DataSeries { Symbol = "GBPUSD", Values = new List<decimal> { 1.0m } }, // Only 1 value
                    new DataSeries { Symbol = "USDJPY", Values = new List<decimal> { 1.0m, 2.0m, 3.0m } }
                }
            };

            // Act
            var result = await _engine.AnalyzeAsync(request);

            // Assert
            Assert.Single(result.Correlations);
            Assert.Equal("USDJPY", result.Correlations[0].Symbol);
        }

        [Theory]
        [InlineData(0.95, "VERY_STRONG")]
        [InlineData(0.75, "STRONG")]
        [InlineData(0.55, "MODERATE")]
        [InlineData(0.35, "WEAK")]
        [InlineData(0.15, "VERY_WEAK")]
        public void GetCorrelationStrength_ReturnsCorrectStrength(decimal correlation, string expectedStrength)
        {
            // Act
            var result = GetStrengthViaAnalysis(correlation);

            // Assert
            Assert.Equal(expectedStrength, result);
        }

        [Theory]
        [InlineData(0.5, "POSITIVE")]
        [InlineData(-0.5, "NEGATIVE")]
        [InlineData(0.05, "NEUTRAL")]
        [InlineData(-0.05, "NEUTRAL")]
        public void GetCorrelationDirection_ReturnsCorrectDirection(decimal correlation, string expectedDirection)
        {
            // Act
            var result = GetDirectionViaAnalysis(correlation);

            // Assert
            Assert.Equal(expectedDirection, result);
        }

        [Theory]
        [InlineData(0.85, "HIGH_CORRELATION_RISK")]
        [InlineData(0.65, "MODERATE_CORRELATION_RISK")]
        [InlineData(0.35, "LOW_CORRELATION_RISK")]
        [InlineData(0.15, "DIVERSIFIED")]
        public void GetRiskSummary_ReturnsCorrectRisk(decimal avgCorrelation, string expectedRisk)
        {
            // Arrange
            var request = new CorrelationRequest
            {
                PrimaryPair = "EURUSD",
                Series = new List<DataSeries>
                {
                    new DataSeries { Symbol = "EURUSD", Values = new List<decimal> { 1.0m, 2.0m, 3.0m } },
                    new DataSeries { Symbol = "GBPUSD", Values = CreateCorrelatedSeries(avgCorrelation) }
                }
            };

            // Act
            var result = _engine.AnalyzeAsync(request).Result;

            // Assert
            Assert.Equal(expectedRisk, result.RiskSummary);
        }

        [Fact]
        public async Task AnalyzeAsync_WithCancellationToken_CancelsOperation()
        {
            // Arrange
            using var cts = new CancellationTokenSource();
            var request = new CorrelationRequest
            {
                PrimaryPair = "EURUSD",
                Series = new List<DataSeries>
                {
                    new DataSeries { Symbol = "EURUSD", Values = Enumerable.Range(1, 1000).Select(i => (decimal)i).ToList() },
                    new DataSeries { Symbol = "GBPUSD", Values = Enumerable.Range(1, 1000).Select(i => (decimal)i).ToList() }
                }
            };

            cts.Cancel();

            // Act & Assert
            await Assert.ThrowsAsync<OperationCanceledException>(
                () => _engine.AnalyzeAsync(request, cts.Token));
        }

        [Fact]
        public async Task AnalyzeAsync_TimestampIsUtc()
        {
            // Arrange
            var request = new CorrelationRequest
            {
                PrimaryPair = "EURUSD",
                Series = new List<DataSeries>
                {
                    new DataSeries { Symbol = "EURUSD", Values = new List<decimal> { 1.0m, 2.0m } },
                    new DataSeries { Symbol = "GBPUSD", Values = new List<decimal> { 1.0m, 2.0m } }
                }
            };

            // Act
            var result = await _engine.AnalyzeAsync(request);

            // Assert
            Assert.Equal(DateTimeKind.Utc, result.TimestampUtc.Kind);
        }

        // Helper methods for testing private methods
        private string GetStrengthViaAnalysis(decimal correlation)
        {
            var request = new CorrelationRequest
            {
                PrimaryPair = "EURUSD",
                Series = new List<DataSeries>
                {
                    new DataSeries { Symbol = "EURUSD", Values = new List<decimal> { 1.0m, 2.0m } },
                    new DataSeries { Symbol = "GBPUSD", Values = new List<decimal> { 1.0m + correlation, 2.0m + correlation } }
                }
            };

            var result = _engine.AnalyzeAsync(request).Result;
            return result.Correlations.First().Strength;
        }

        private string GetDirectionViaAnalysis(decimal correlation)
        {
            var request = new CorrelationRequest
            {
                PrimaryPair = "EURUSD",
                Series = new List<DataSeries>
                {
                    new DataSeries { Symbol = "EURUSD", Values = new List<decimal> { 1.0m, 2.0m, 3.0m } },
                    new DataSeries { Symbol = "GBPUSD", Values = new List<decimal> { 1.0m, 2.0m + correlation, 3.0m + correlation } }
                }
            };

            var result = _engine.AnalyzeAsync(request).Result;
            return result.Correlations.First().Direction;
        }

        private List<decimal> CreateCorrelatedSeries(decimal targetCorrelation)
        {
            // Simple approximation for testing
            return new List<decimal> { 1.0m, 2.0m, 3.0m };
        }
    }
}