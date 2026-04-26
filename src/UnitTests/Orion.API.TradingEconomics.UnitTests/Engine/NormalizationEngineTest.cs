using Orion.API.TradingEconomics.Engine;
using Orion.API.TradingEconomics.Entities;
using Xunit;

namespace Orion.API.TradingEconomics.UnitTests.Engine
{
    public sealed class NormalizationEngineTests
    {
        [Fact]
        public void Constructor_ShouldThrow_WhenOptionsIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new NormalizationEngine(null!));
        }

        [Fact]
        public void Normalize_ShouldThrow_WhenRawIsNull()
        {
            var engine = CreateEngine();

            Assert.Throws<ArgumentNullException>(() =>
                engine.Normalize(null!));
        }

        [Fact]
        public void Normalize_ShouldReturnEmpty_WhenInputIsEmpty()
        {
            var engine = CreateEngine();

            var result = engine.Normalize([]);

            Assert.Empty(result);
        }

        [Fact]
        public void Normalize_ShouldReturnEmpty_WhenNotEnoughData()
        {
            var engine = CreateEngine(minimumWindowSize: 3);

            var raw = new List<EconomicIndicator>
            {
                CreateIndicator(new DateTime(2024, 1, 1), 100m),
                CreateIndicator(new DateTime(2024, 2, 1), 101m)
            };

            var result = engine.Normalize(raw);

            Assert.Empty(result);
        }

        [Fact]
        public void Normalize_ShouldCreateNormalizedIndicators_WhenValidDataExists()
        {
            var engine = CreateEngine(minimumWindowSize: 3);

            var raw = new List<EconomicIndicator>
            {
                CreateIndicator(new DateTime(2024, 1, 1), 100m, forecast: 95m),
                CreateIndicator(new DateTime(2024, 2, 1), 110m, previous: 100m, forecast: 100m),
                CreateIndicator(new DateTime(2024, 3, 1), 121m, previous: 110m, forecast: 120m)
            };

            var result = engine.Normalize(raw);

            Assert.Single(result);

            var item = result[0];

            Assert.Equal("US", item.Country);
            Assert.Equal("CPI", item.Indicator);
            Assert.Equal(121m, item.Value);
            Assert.Equal(110m, item.Previous);
            Assert.Equal(120m, item.Forecast);
            Assert.Equal(0.1m, Math.Round(item.MoM, 4));
            Assert.Equal(0.0083m, Math.Round(item.Surprise, 4));
            Assert.True(item.RollingStdDev > 0);
        }

        [Fact]
        public void Normalize_ShouldIgnoreNullValues()
        {
            var engine = CreateEngine(minimumWindowSize: 2);

            var raw = new List<EconomicIndicator>
            {
                CreateIndicator(new DateTime(2024, 1, 1), null),
                CreateIndicator(new DateTime(2024, 2, 1), 100m),
                CreateIndicator(new DateTime(2024, 3, 1), 110m)
            };

            var result = engine.Normalize(raw);

            Assert.Single(result);
            Assert.Equal(110m, result[0].Value);
        }

        [Fact]
        public void Normalize_ShouldClampZScore_WhenWinsorizeIsEnabled()
        {
            var engine = new NormalizationEngine(new NormalizationOptions
            {
                MinimumWindowSize = 2,
                WinsorizeOutliers = true,
                WinsorizeZLimit = 0.5m
            });

            var raw = new List<EconomicIndicator>
            {
                CreateIndicator(new DateTime(2024, 1, 1), 1m),
                CreateIndicator(new DateTime(2024, 2, 1), 100m)
            };

            var result = engine.Normalize(raw);

            Assert.Single(result);
            Assert.True(result[0].ZScore <= 0.5m);
        }

        private static NormalizationEngine CreateEngine(int minimumWindowSize = 2)
        {
            return new NormalizationEngine(new NormalizationOptions
            {
                MinimumWindowSize = minimumWindowSize,
                WinsorizeOutliers = false,
                WinsorizeZLimit = 3m
            });
        }

        private static EconomicIndicator CreateIndicator(
            DateTime date,
            decimal? value,
            decimal? previous = null,
            decimal? forecast = null)
        {
            return new EconomicIndicator
            {
                Country = "US",
                Indicator = "CPI",
                Date = date,
                Value = value,
                Previous = previous,
                Forecast = forecast,
                Frequency = "Monthly"
            };
        }
    }
}