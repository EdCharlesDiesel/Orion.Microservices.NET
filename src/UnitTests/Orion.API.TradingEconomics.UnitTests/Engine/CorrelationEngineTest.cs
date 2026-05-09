using Orion.API.TradingEconomics.Engine;
using Orion.API.TradingEconomics.Entities;
using Xunit;

namespace Orion.API.TradingEconomics.UnitTests.Engine
{
    public sealed class CorrelationEngineTests
    {
        private readonly CorrelationEngine _engine = new();

        [Fact]
        public async Task ShouldThrow_WhenRequestNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _engine.AnalyzeAsync(null!));
        }

        [Fact]
        public async Task ShouldThrow_WhenPrimaryMissing()
        {
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _engine.AnalyzeAsync(new CorrelationRequest()));
        }

        [Fact]
        public async Task ShouldCalculatePerfectPositiveCorrelation()
        {
            var request = new CorrelationRequest
            {
                PrimaryPair = "A",
                Series =
                [
                    Create("A", [1,2,3,4,5]),
                    Create("B", [2,4,6,8,10])
                ]
            };

            var result = await _engine.AnalyzeAsync(request);

            Assert.Single(result.Correlations);
            Assert.Equal(1.0m, result.Correlations[0].Correlation);
        }

        [Fact]
        public async Task ShouldCalculateNegativeCorrelation()
        {
            var request = new CorrelationRequest
            {
                PrimaryPair = "A",
                Series =
                [
                    Create("A", [1,2,3,4,5]),
                    Create("B", [5,4,3,2,1])
                ]
            };

            var result = await _engine.AnalyzeAsync(request);

            Assert.Equal(-1.0m, result.Correlations[0].Correlation);
            Assert.Equal("NEGATIVE", result.Correlations[0].Direction);
        }

        [Fact]
        public async Task ShouldSkipInvalidSeries()
        {
            var request = new CorrelationRequest
            {
                PrimaryPair = "A",
                Series =
                [
                    Create("A", [1,2,3]),
                    new CorrelationSeries { Symbol = "B", Values = [] }
                ]
            };

            var result = await _engine.AnalyzeAsync(request);

            Assert.Empty(result.Correlations);
        }

        private static CorrelationSeries Create(string symbol, List<decimal> values)
        {
            return new CorrelationSeries
            {
                Symbol = symbol,
                Values = values
            };
        }
    }
}