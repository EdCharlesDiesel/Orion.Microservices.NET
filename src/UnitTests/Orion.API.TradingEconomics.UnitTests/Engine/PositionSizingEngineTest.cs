using Orion.API.TradingEconomics.Engine;
using Orion.API.TradingEconomics.Entities;
using Xunit;

namespace Orion.API.TradingEconomics.UnitTests.Engine
{
    public sealed class PositionSizingEngineTests
    {
        private readonly PositionSizingEngine _engine = new();

        [Fact]
        public void Calculate_ShouldThrow_WhenSignalIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _engine.Calculate(null!, CreateRisk(), CreateMarket(), CreateAccount()));
        }

        [Fact]
        public void Calculate_ShouldThrow_WhenRiskIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _engine.Calculate(CreateSignal(), null!, CreateMarket(), CreateAccount()));
        }

        [Fact]
        public void Calculate_ShouldReturnNone_WhenSignalIsNoTrade()
        {
            var result = _engine.Calculate(
                new SignalResult { Direction = "NO_TRADE" },
                CreateRisk(),
                CreateMarket(),
                CreateAccount());

            Assert.False(result.IsAllowed);
            Assert.Equal("No trade signal.", result.Reason);
        }

        [Fact]
        public void Calculate_ShouldReturnNone_WhenRiskIsNotAllowed()
        {
            var result = _engine.Calculate(
                CreateSignal(),
                new RiskResult { IsAllowed = false, Reason = "Risk blocked." },
                CreateMarket(),
                CreateAccount());

            Assert.False(result.IsAllowed);
            Assert.Equal("Risk blocked.", result.Reason);
        }

        [Fact]
        public void Calculate_ShouldReturnNone_WhenBalanceIsInvalid()
        {
            var result = _engine.Calculate(
                CreateSignal(),
                CreateRisk(),
                CreateMarket(),
                new AccountContext { Balance = 0m });

            Assert.False(result.IsAllowed);
            Assert.Equal("Invalid account balance.", result.Reason);
        }

        [Fact]
        public void Calculate_ShouldReturnNone_WhenNotEnoughCandles()
        {
            var market = CreateMarket();
            market.Candles = CreateCandles(14);

            var result = _engine.Calculate(
                CreateSignal(),
                CreateRisk(),
                market,
                CreateAccount());

            Assert.False(result.IsAllowed);
            Assert.Equal("Not enough candles for position sizing.", result.Reason);
        }

        [Fact]
        public void Calculate_ShouldReturnNone_WhenConfidenceTooLow()
        {
            var result = _engine.Calculate(
                new SignalResult { Direction = "LONG", Confidence = 40m },
                CreateRisk(),
                CreateMarket(),
                CreateAccount());

            Assert.False(result.IsAllowed);
            Assert.Equal("Signal confidence too low.", result.Reason);
        }

        [Fact]
        public void Calculate_ShouldReturnAllowedPositionSize()
        {
            var result = _engine.Calculate(
                CreateSignal(confidence: 85m),
                CreateRisk(score: 0.20m),
                CreateMarket(),
                CreateAccount(balance: 10_000m));

            Assert.True(result.IsAllowed);
            Assert.Equal("EUR/USD", result.Pair);
            Assert.Equal("LONG", result.Direction);
            Assert.True(result.PositionSize > 0m);
            Assert.True(result.RiskAmount > 0m);
            Assert.True(result.StopDistance > 0m);
        }

        [Fact]
        public void Calculate_ShouldUsePairSpecificAtrMultiplier()
        {
            var eurusd = _engine.Calculate(
                CreateSignal(confidence: 75m),
                CreateRisk(score: 0.40m),
                CreateMarket(pair: "EUR/USD"),
                CreateAccount());

            var usdzar = _engine.Calculate(
                CreateSignal(confidence: 75m),
                CreateRisk(score: 0.40m),
                CreateMarket(pair: "USD/ZAR"),
                CreateAccount());

            Assert.True(usdzar.StopDistance > eurusd.StopDistance);
        }

        private static SignalResult CreateSignal(
            string direction = "LONG",
            decimal confidence = 75m)
        {
            return new SignalResult
            {
                Direction = direction,
                Confidence = confidence
            };
        }

        private static RiskResult CreateRisk(decimal score = 0.40m)
        {
            return new RiskResult
            {
                IsAllowed = true,
                Score = score,
                Reason = "Allowed"
            };
        }

        private static NormalizedMarketContext CreateMarket(string pair = "EUR/USD")
        {
            return new NormalizedMarketContext
            {
                Pair = pair,
                Candles = CreateCandles(15)
            };
        }

        private static AccountContext CreateAccount(decimal balance = 10_000m)
        {
            return new AccountContext
            {
                Balance = balance
            };
        }

        private static List<OhlcvBar> CreateCandles(int count)
        {
            return Enumerable.Range(1, count)
                .Select(i => new OhlcvBar
                {
                    TimestampUtc = DateTime.UtcNow.AddMinutes(i),
                    Open = 1.1000m,
                    High = 1.1050m,
                    Low = 1.0950m,
                    Close = 1.1000m,
                    Volume = 1000m
                })
                .ToList();
        }
    }
}