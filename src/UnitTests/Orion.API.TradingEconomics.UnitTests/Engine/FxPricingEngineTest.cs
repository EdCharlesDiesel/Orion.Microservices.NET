using Orion.API.TradingEconomics.Engine;
using Orion.API.TradingEconomics.Entities;
using Xunit;

namespace Orion.API.TradingEconomics.UnitTests.Engine
{
    public sealed class FxPricingEngineTests
    {
        private static FxPricingEngine CreateEngine()
        {
            return new FxPricingEngine(
                new CurrencyStrengthModel(),
                new FxRelativePricer(),
                new FxPriceSimulator());
        }

        [Fact]
        public void Price_Should_Return_Normalized_Result_For_EurUsd()
        {
            var engine = CreateEngine();

            var result = engine.Price("EURUSD", "LONG", 1000m);

            Assert.Equal("EUR/USD", result.Pair);
            Assert.Equal("LONG", result.Direction);
            Assert.Equal(1000m, result.PositionSize);
            Assert.Equal("EUR", result.BaseCurrency);
            Assert.Equal("USD", result.QuoteCurrency);
        }

        [Fact]
        public void Price_Should_Return_Normalized_Result_For_SlashedPair()
        {
            var engine = CreateEngine();

            var result = engine.Price("EUR/USD", "short", 500m);

            Assert.Equal("EUR/USD", result.Pair);
            Assert.Equal("SHORT", result.Direction);
            Assert.Equal(500m, result.PositionSize);
        }

        [Fact]
        public void Price_Should_Throw_When_Pair_Is_Invalid()
        {
            var engine = CreateEngine();

            Assert.Throws<ArgumentException>(() =>
                engine.Price("EUR", "LONG", 1000m));
        }

        [Fact]
        public void Price_Should_Throw_When_Direction_Is_Invalid()
        {
            var engine = CreateEngine();

            Assert.Throws<ArgumentException>(() =>
                engine.Price("EURUSD", "BUY", 1000m));
        }

        [Fact]
        public void Price_Should_Throw_When_PositionSize_Is_Zero()
        {
            var engine = CreateEngine();

            Assert.Throws<ArgumentOutOfRangeException>(() =>
                engine.Price("EURUSD", "LONG", 0m));
        }

        [Fact]
        public void Run_Should_Throw_When_States_Is_Null()
        {
            var engine = CreateEngine();

            Assert.Throws<ArgumentNullException>(() =>
                engine.Run(null!, new Dictionary<string, decimal> { ["EUR/USD"] = 1.10m }));
        }

        [Fact]
        public void Run_Should_Throw_When_InitialPrices_Is_Null()
        {
            var engine = CreateEngine();

            Assert.Throws<ArgumentNullException>(() =>
                engine.Run(new List<MacroState>(), null!));
        }

        [Fact]
        public void Run_Should_Return_Empty_When_States_Are_Empty()
        {
            var engine = CreateEngine();

            var result = engine.Run(
                new List<MacroState>(),
                new Dictionary<string, decimal> { ["EUR/USD"] = 1.10m });

            Assert.Empty(result);
        }

        [Fact]
        public void Run_Should_Throw_When_InitialPrices_Are_Empty()
        {
            var engine = CreateEngine();

            Assert.Throws<ArgumentException>(() =>
                engine.Run(
                    new List<MacroState> { new MacroState() },
                    new Dictionary<string, decimal>()));
        }
    }
}