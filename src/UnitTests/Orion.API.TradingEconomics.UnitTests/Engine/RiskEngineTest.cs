using Orion.API.TradingEconomics.Engine;
using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Enum;
using Xunit;

namespace Orion.API.TradingEconomics.UnitTests.Engine
{
    public sealed class RiskEngineTests
    {
        private readonly RiskEngine _engine = new();

        [Fact]
        public void Evaluate_ShouldThrow_WhenSignalIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _engine.Evaluate(null!, CreateMarket(), CreateRegime()));
        }

        [Fact]
        public void Evaluate_ShouldThrow_WhenRegimeIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _engine.Evaluate(CreateSignal(), CreateMarket(), null!));
        }

        [Fact]
        public void Evaluate_ShouldBlock_WhenMarketIsNull()
        {
            var result = _engine.Evaluate(CreateSignal(), null, CreateRegime());

            Assert.False(result.IsAllowed);
            Assert.Equal("Market context is null.", result.Reason);
        }

        [Fact]
        public void Evaluate_ShouldBlock_WhenSignalIsNoTrade()
        {
            var result = _engine.Evaluate(
                new SignalResult { Direction = "NO_TRADE", Reason = "No setup." },
                CreateMarket(),
                CreateRegime());

            Assert.False(result.IsAllowed);
            Assert.Equal("No setup.", result.Reason);
        }

        [Fact]
        public void Evaluate_ShouldBlock_WhenConfidenceIsTooLow()
        {
            var result = _engine.Evaluate(
                CreateSignal(confidence: 40m),
                CreateMarket(),
                CreateRegime());

            Assert.False(result.IsAllowed);
            Assert.Equal("Signal confidence too low: 40%.", result.Reason);
        }

        [Fact]
        public void Evaluate_ShouldBlock_WhenNotEnoughCandles()
        {
            var market = CreateMarket(candleCount: 19);

            var result = _engine.Evaluate(CreateSignal(), market, CreateRegime());

            Assert.False(result.IsAllowed);
            Assert.Equal("Not enough candles for risk evaluation.", result.Reason);
        }

        [Fact]
        public void Evaluate_ShouldBlock_WhenLatestCloseIsInvalid()
        {
            var market = CreateMarket();
            market.Candles[^1].Close = 0m;

            var result = _engine.Evaluate(CreateSignal(), market, CreateRegime());

            Assert.False(result.IsAllowed);
            Assert.Equal("Invalid latest close price.", result.Reason);
        }

        [Fact]
        public void Evaluate_ShouldBlock_WhenSpreadRiskIsTooHigh()
        {
            var market = CreateMarket(spread: 0.0100m);

            var result = _engine.Evaluate(CreateSignal(), market, CreateRegime());

            Assert.False(result.IsAllowed);
            Assert.Equal("Spread risk too high.", result.Reason);
        }

        [Fact]
        public void Evaluate_ShouldBlock_WhenVolatilityRiskIsTooHigh()
        {
            var market = CreateMarket(high: 1.2000m, low: 1.0000m);

            var result = _engine.Evaluate(CreateSignal(), market, CreateRegime());

            Assert.False(result.IsAllowed);
            Assert.Equal("Volatility risk too high.", result.Reason);
        }

        [Fact]
        public void Evaluate_ShouldBlock_WhenSignalConflictsWithRegime()
        {
            var result = _engine.Evaluate(
                CreateSignal(direction: "LONG"),
                CreateMarket(),
                CreateRegime("BEARISH"));

            Assert.False(result.IsAllowed);
            Assert.Equal("Signal conflicts with market regime.", result.Reason);
        }

        [Fact]
        public void Evaluate_ShouldAllow_WhenRiskIsAcceptable()
        {
            var result = _engine.Evaluate(
                CreateSignal(direction: "LONG", confidence: 75m),
                CreateMarket(spread: 0.0001m),
                CreateRegime("BULLISH"));

            Assert.True(result.IsAllowed);
            Assert.Contains("Risk accepted", result.Reason);
        }

        [Fact]
        public void Evaluate_ShouldUseRegimeEnum_WhenNameIsMissing()
        {
            var result = _engine.Evaluate(
                CreateSignal(direction: "LONG", confidence: 75m),
                CreateMarket(spread: 0.0001m),
                new RegimeResult
                {
                    Regime = MarketRegime.RiskOn
                });

            Assert.True(result.IsAllowed);
        }

        private static SignalResult CreateSignal(
            string direction = "LONG",
            decimal confidence = 75m)
        {
            return new SignalResult
            {
                Direction = direction,
                Confidence = confidence,
                Reason = "Signal accepted"
            };
        }

        private static RegimeResult CreateRegime(string name = "BULLISH")
        {
            return new RegimeResult
            {
                Name = name,
                Regime = MarketRegime.RiskOn
            };
        }

        private static NormalizedMarketContext CreateMarket(
            int candleCount = 20,
            decimal spread = 0.0001m,
            decimal high = 1.1010m,
            decimal low = 1.0990m,
            decimal close = 1.1000m)
        {
            return new NormalizedMarketContext
            {
                Pair = "EUR/USD",
                Spread = spread,
                Candles = Enumerable.Range(1, candleCount)
                    .Select(i => new OhlcvBar
                    {
                        TimestampUtc = DateTime.UtcNow.AddMinutes(i),
                        Open = close,
                        High = high,
                        Low = low,
                        Close = close,
                        Volume = 1000m
                    })
                    .ToList()
            };
        }
    }
}