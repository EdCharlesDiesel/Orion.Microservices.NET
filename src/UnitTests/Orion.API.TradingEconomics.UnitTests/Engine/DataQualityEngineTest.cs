using Orion.API.TradingEconomics.Engine;
using Orion.API.TradingEconomics.Entities;
using Xunit;

namespace Orion.API.TradingEconomics.UnitTests.Engine
{
    public sealed class DataQualityEngineTests
    {
        private readonly DataQualityEngine _engine = new();

        [Fact]
        public void Validate_ShouldFail_WhenInputNull()
        {
            var result = _engine.Validate(null);

            Assert.False(result.IsValid);
            // Assert.Contains(string.Empty, result.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Validate_ShouldFail_WhenPairMissing()
        {
            var result = _engine.Validate(new ForexMarketInput
            {
                Pair = "",
                Candles = CreateValidCandles()
            });

            Assert.False(result.IsValid);
            Assert.Contains("Pair is missing", result.Message);
        }

        [Fact]
        public void Validate_ShouldFail_WhenNoCandles()
        {
            var result = _engine.Validate(new ForexMarketInput
            {
                Pair = "EUR/USD",
                Candles = []
            });

            Assert.False(result.IsValid);
        }

        [Fact]
        public void Validate_ShouldFail_WhenNotEnoughCandles()
        {
            var result = _engine.Validate(new ForexMarketInput
            {
                Pair = "EUR/USD",
                Candles = CreateValidCandles(10)
            });

            Assert.False(result.IsValid);
            Assert.Contains("Minimum required", result.Message);
        }

        [Fact]
        public void Validate_ShouldFail_WhenDuplicateTimestamps()
        {
            var candles = CreateValidCandles();
            candles[1].TimestampUtc = candles[0].TimestampUtc;

            var result = _engine.Validate(new ForexMarketInput
            {
                Pair = "EUR/USD",
                Candles = candles
            });

            Assert.False(result.IsValid);
            Assert.Contains("Duplicate", result.Message);
        }

        [Fact]
        public void Validate_ShouldFail_WhenInvalidPrices()
        {
            var candles = CreateValidCandles();
            candles[0].High = 0;

            var result = _engine.Validate(new ForexMarketInput
            {
                Pair = "EUR/USD",
                Candles = candles
            });

            Assert.False(result.IsValid);
            Assert.Contains("Invalid OHLC", result.Message);
        }

        [Fact]
        public void Validate_ShouldFail_WhenDataIsStale()
        {
            var candles = CreateValidCandles();
            candles[^1].TimestampUtc = DateTime.UtcNow.AddDays(-10);

            var result = _engine.Validate(new ForexMarketInput
            {
                Pair = "EUR/USD",
                Candles = candles
            });

            Assert.False(result.IsValid);
            // Assert.Contains("stale", result.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Validate_ShouldFail_WhenTooManyGaps()
        {
            var candles = CreateValidCandles();

            // Inject gaps > 5 days
            for (int i = 1; i < candles.Count; i += 10)
            {
                candles[i].TimestampUtc = candles[i - 1].TimestampUtc.AddDays(10);
            }

            var result = _engine.Validate(new ForexMarketInput
            {
                Pair = "EUR/USD",
                Candles = candles
            });

            Assert.False(result.IsValid);
            // Assert.Contains("gaps", result.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Validate_ShouldFail_WhenPriceSpikeDetected()
        {
            var candles = CreateValidCandles();
            candles[^1].Close = candles[^2].Close * 2; // >100% spike

            var result = _engine.Validate(new ForexMarketInput
            {
                Pair = "EUR/USD",
                Candles = candles
            });

            Assert.False(result.IsValid);
            // Assert.Contains("spike", result.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Validate_ShouldPass_WithValidData()
        {
            var result = _engine.Validate(new ForexMarketInput
            {
                Pair = "EUR/USD",
                Candles = CreateValidCandles()
            });

            Assert.True(result.IsValid);
        }

        // ✅ CRITICAL: Valid test data generator
        private static List<OhlcvBar> CreateValidCandles(int count = 60)
        {
            var candles = new List<OhlcvBar>();
            var start = DateTime.UtcNow.AddDays(-count);

            decimal price = 1.1000m;

            for (int i = 0; i < count; i++)
            {
                var open = price;
                var close = price + 0.0005m;
                var high = Math.Max(open, close) + 0.0005m;
                var low = Math.Min(open, close) - 0.0005m;

                candles.Add(new OhlcvBar
                {
                    TimestampUtc = start.AddDays(i), // no gaps
                    Open = open,
                    High = high,
                    Low = low,
                    Close = close,
                    Volume = 1000
                });

                price = close;
            }

            return candles;
        }
    }
}