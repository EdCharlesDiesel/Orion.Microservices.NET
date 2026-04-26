using Orion.API.TradingEconomics.Engine;
using Orion.API.TradingEconomics.Entities;
using Xunit;

namespace Orion.API.TradingEconomics.UnitTests.Engine
{
    public class DataQualityEngineTests
    {
        private readonly DataQualityEngine _engine = new();

        [Fact]
        public void Validate_NullInput_ReturnsFail()
        {
            var result = _engine.Validate(null);
            Assert.False(result.IsValid);
            Assert.Contains("null", result.Message);
        }

        [Fact]
        public void Validate_EmptyPair_ReturnsFail()
        {
            var input = new ForexMarketInput { Pair = "", Candles = CreateValidCandles(50) };
            var result = _engine.Validate(input);
            Assert.False(result.IsValid);
            Assert.Contains("Pair", result.Message);
        }

        [Fact]
        public void Validate_NullCandles_ReturnsFail()
        {
            var input = new ForexMarketInput { Pair = "EURUSD", Candles = null! };
            var result = _engine.Validate(input);
            Assert.False(result.IsValid);
            Assert.Contains("No candle data", result.Message);
        }

        [Fact]
        public void Validate_EmptyCandles_ReturnsFail()
        {
            var input = new ForexMarketInput { Pair = "EURUSD", Candles = new List<OhlcvBar>() };
            var result = _engine.Validate(input);
            Assert.False(result.IsValid);
            Assert.Contains("No candle data", result.Message);
        }

        [Fact]
        public void Validate_LessThanMinimumCandles_ReturnsFail()
        {
            var input = new ForexMarketInput { Pair = "EURUSD", Candles = CreateValidCandles(49) };
            var result = _engine.Validate(input);
            Assert.False(result.IsValid);
            Assert.Contains("50", result.Message);
        }

        [Fact]
        public void Validate_DuplicateTimestamps_ReturnsFail()
        {
            var timestamp = DateTime.UtcNow;
            var candles = new List<OhlcvBar>
            {
                CreateValidCandle(1, timestamp),
                CreateValidCandle(2, timestamp),
                CreateValidCandle(3, timestamp.AddDays(1))
            };

            var input = new ForexMarketInput { Pair = "EURUSD", Candles = candles };
            var result = _engine.Validate(input);
            Assert.False(result.IsValid);
            Assert.Contains("Duplicate", result.Message);
        }

        [Theory]
        [InlineData(0, 100, 100, 100)] // Open <= 0
        [InlineData(100, 0, 100, 100)] // High <= 0
        [InlineData(100, 100, 0, 100)] // Low <= 0
        [InlineData(100, 100, 100, 0)] // Close <= 0
        [InlineData(100, 50, 100, 100)] // High < Low
        [InlineData(100, 90, 100, 110)] // High < Close
        public void Validate_InvalidPrices_ReturnsFail(decimal open, decimal high, decimal low, decimal close)
        {
            var candles = CreateValidCandles(50);
            candles[0] = new OhlcvBar
            {
                TimestampUtc = DateTime.UtcNow,
                Open = open,
                High = high,
                Low = low,
                Close = close,
                Volume = 1000
            };

            var input = new ForexMarketInput { Pair = "EURUSD", Candles = candles };
            var result = _engine.Validate(input);
            Assert.False(result.IsValid);
            Assert.Contains("Invalid OHLC", result.Message);
        }

        [Fact]
        public void Validate_StaleData_ReturnsFail()
        {
            var candles = CreateValidCandles(50);
            candles[^1] = CreateValidCandle(50, DateTime.UtcNow.AddDays(-8));

            var input = new ForexMarketInput { Pair = "EURUSD", Candles = candles };
            var result = _engine.Validate(input);
            Assert.False(result.IsValid);
            Assert.Contains("stale", result.Message);
        }

        [Fact]
        public void Validate_TooManyGaps_ReturnsFail()
        {
            var candles = CreateValidCandles(50);
            for (int i = 10; i < 20; i++)
            {
                candles[i] = CreateValidCandle(i, candles[i - 1].TimestampUtc.AddDays(6));
            }

            var input = new ForexMarketInput { Pair = "EURUSD", Candles = candles };
            var result = _engine.Validate(input);
            Assert.False(result.IsValid);
            Assert.Contains("gaps", result.Message);
        }

        [Fact]
        public void Validate_PriceSpike_ReturnsFail()
        {
            var candles = CreateValidCandles(50);
            candles[40] = CreateValidCandle(40, DateTime.UtcNow.AddHours(-10), 100);
            candles[41] = CreateValidCandle(41, DateTime.UtcNow.AddHours(-9), 120);

            var input = new ForexMarketInput { Pair = "EURUSD", Candles = candles };
            var result = _engine.Validate(input);
            Assert.False(result.IsValid);
            Assert.Contains("spike", result.Message);
        }

        [Fact]
        public void Validate_ValidData_ReturnsPass()
        {
            var input = new ForexMarketInput { Pair = "EURUSD", Candles = CreateValidCandles(100) };
            var result = _engine.Validate(input);
            Assert.True(result.IsValid);
            Assert.Contains("passed", result.Message);
        }

        [Fact]
        public void ValidateCandles_NullInput_ReturnsFail()
        {
            var result = _engine.ValidateCandles(null);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void ValidateCandles_ValidData_ReturnsPass()
        {
            var candles = CreateValidCandles(100);
            var result = _engine.ValidateCandles(candles);
            Assert.True(result.IsValid);
        }

        private static List<OhlcvBar> CreateValidCandles(int count)
        {
            var candles = new List<OhlcvBar>();
            var baseTime = DateTime.UtcNow.AddDays(-count);

            for (int i = 0; i < count; i++)
            {
                candles.Add(CreateValidCandle(i, baseTime.AddDays(i)));
            }

            return candles;
        }

        private static OhlcvBar CreateValidCandle(int index, DateTime timestamp, decimal basePrice = 100)
        {
            var price = basePrice + index * 0.1m;
            return new OhlcvBar
            {
                TimestampUtc = timestamp,
                Open = price,
                High = price + 0.5m,
                Low = price - 0.5m,
                Close = price + 0.2m,
                Volume = 1000
            };
        }
    }
}