using Orion.API.TradingEconomics.Engine;
using Orion.API.TradingEconomics.Entities;
using Xunit;

namespace Orion.API.TradingEconomics.UnitTests.Engine
{
    public sealed class FxPricingEngineTests
    {
        private static readonly List<CurrencyModel> Models =
        [
            new CurrencyModel
            {
                Currency = "EUR",
                CarryWeight = 23m,
                GrowthWeight = 26m,      
                InflationWeight = 2.3m,
                RiskWeight = 5m          
            }
        ];

        private readonly FxPricingEngine _engine =
            new(
                new CurrencyStrengthModel(Models),
                new FxRelativePricer(),
                new FxPriceSimulator());

        [Fact]
        public void Run_ShouldThrow_WhenStatesNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _engine.Run(null!, new Dictionary<string, decimal>()));
        }

        [Fact]
        public void Run_ShouldThrow_WhenInitialPricesNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _engine.Run(new List<MacroState>(), null!));
        }

        [Fact]
        public void Run_ShouldReturnEmpty_WhenStatesEmpty()
        {
            var result = _engine.Run(
                [],
                new Dictionary<string, decimal> { ["EUR/USD"] = 1.1m });

            Assert.Empty(result);
        }

        [Fact]
        public void Run_ShouldThrow_WhenInitialPricesEmpty()
        {
            Assert.Throws<ArgumentException>(() =>
                _engine.Run(
                    [new MacroState()],
                    new Dictionary<string, decimal>()));
        }

        [Fact]
        public void Price_ShouldThrow_WhenPairInvalid()
        {
            Assert.Throws<ArgumentException>(() =>
                _engine.Price("", "LONG", 1m));
        }

        [Fact]
        public void Price_ShouldThrow_WhenDirectionInvalid()
        {
            Assert.Throws<ArgumentException>(() =>
                _engine.Price("EURUSD", "", 1m));
        }

        [Fact]
        public void Price_ShouldThrow_WhenSizeInvalid()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                _engine.Price("EURUSD", "LONG", 0m));
        }

        [Fact]
        public void Price_ShouldThrow_WhenDirectionNotSupported()
        {
            Assert.Throws<ArgumentException>(() =>
                _engine.Price("EURUSD", "BUY", 1m));
        }

        [Fact]
        public void Price_ShouldNormalizePair()
        {
            var result = _engine.Price("eur/usd", "long", 1000m);

            Assert.Equal("EUR/USD", result.Pair);
            Assert.Equal("LONG", result.Direction);
            Assert.Equal("EUR", result.BaseCurrency);
            Assert.Equal("USD", result.QuoteCurrency);
        }

        [Fact]
        public void Price_ShouldHandleDifferentFormats()
        {
            var result = _engine.Price("eur-usd", "short", 1000m);

            Assert.Equal("EUR/USD", result.Pair);
            Assert.Equal("SHORT", result.Direction);
        }
    }
}