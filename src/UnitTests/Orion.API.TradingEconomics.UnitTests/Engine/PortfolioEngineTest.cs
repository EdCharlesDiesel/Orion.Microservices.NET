using System.Globalization;
using Microsoft.Extensions.Configuration;
using Orion.API.TradingEconomics.Engine;
using Orion.API.TradingEconomics.Entities;
using Xunit;

namespace Orion.API.TradingEconomics.UnitTests.Engine
{
    public sealed class PortfolioEngineTests
    {
        [Fact]
        public void Constructor_ShouldThrow_WhenConfigIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new PortfolioEngine(null!));
        }

        [Fact]
        public void Evaluate_ShouldThrow_WhenNewTradeIsNull()
        {
            var engine = CreateEngine();

            Assert.Throws<ArgumentNullException>(() =>
                engine.Evaluate(null!, [], CreateAccount()));
        }

        [Fact]
        public void Evaluate_ShouldThrow_WhenOpenTradesIsNull()
        {
            var engine = CreateEngine();

            Assert.Throws<ArgumentNullException>(() =>
                engine.Evaluate(CreateTrade(), null!, CreateAccount()));
        }

        [Fact]
        public void Evaluate_ShouldThrow_WhenAccountIsNull()
        {
            var engine = CreateEngine();

            Assert.Throws<ArgumentNullException>(() =>
                engine.Evaluate(CreateTrade(), [], null!));
        }

        [Fact]
        public void Evaluate_ShouldBlock_WhenTradeIsNotOpen()
        {
            var engine = CreateEngine();

            var result = engine.Evaluate(
                CreateTrade(status: "CLOSED"),
                [],
                CreateAccount());

            Assert.False(result.Allowed);
            Assert.Equal("Trade is not open.", result.Reason);
        }

        [Fact]
        public void Evaluate_ShouldBlock_WhenAccountEquityIsInvalid()
        {
            var engine = CreateEngine();

            var result = engine.Evaluate(
                CreateTrade(),
                [],
                new AccountContext { Equity = 0m });

            Assert.False(result.Allowed);
            Assert.Equal("Invalid account equity.", result.Reason);
        }

        [Fact]
        public void Evaluate_ShouldBlock_WhenMaxOpenTradesReached()
        {
            var engine = CreateEngine(maxOpenTrades: 1);

            var result = engine.Evaluate(
                CreateTrade(),
                [CreateTrade(pair: "GBP/USD")],
                CreateAccount());

            Assert.False(result.Allowed);
            Assert.Equal("Maximum open trades reached.", result.Reason);
        }

        [Fact]
        public void Evaluate_ShouldBlock_WhenNewTradeRiskIsInvalid()
        {
            var engine = CreateEngine();

            var result = engine.Evaluate(
                CreateTrade(positionSize: 0m),
                [],
                CreateAccount());

            Assert.False(result.Allowed);
            Assert.Equal("Invalid new trade risk.", result.Reason);
        }

        [Fact]
        public void Evaluate_ShouldBlock_WhenPortfolioRiskIsTooHigh()
        {
            var engine = CreateEngine(maxDailyLossPercent: 1m);

            var result = engine.Evaluate(
                CreateTrade(positionSize: 100_000m, entry: 1.1000m, stop: 1.0000m),
                [],
                CreateAccount(equity: 10_000m));

            Assert.False(result.Allowed);
            Assert.Contains("Portfolio risk too high", result.Reason);
        }

        [Fact]
        public void Evaluate_ShouldBlock_WhenUsdExposureIsTooHigh()
        {
            var engine = CreateEngine(maxDailyLossPercent: 100m);

            var result = engine.Evaluate(
                CreateTrade(pair: "EUR/USD", positionSize: 100_000m, entry: 1.1000m, stop: 1.0999m),
                [],
                CreateAccount(equity: 10_000m));

            Assert.False(result.Allowed);
            Assert.Contains("USD exposure too high", result.Reason);
        }

        [Fact]
        public void Evaluate_ShouldBlock_WhenCorrelationRiskIsTooHigh()
        {
            var engine = CreateEngine(maxDailyLossPercent: 100m);

            var openTrades = new List<TradePlan>
            {
                CreateTrade(pair: "GBP/USD", entry: 1.1000m, stop: 1.0999m),
                CreateTrade(pair: "AUD/USD", entry: 1.1000m, stop: 1.0999m),
                CreateTrade(pair: "NZD/USD", entry: 1.1000m, stop: 1.0999m)
            };

            var result = engine.Evaluate(
                CreateTrade(pair: "EUR/USD", entry: 1.1000m, stop: 1.0999m),
                openTrades,
                CreateAccount(equity: 1_000_000m));

            Assert.False(result.Allowed);
            Assert.Equal("Too many correlated trades.", result.Reason);
        }

        [Fact]
        public void Evaluate_ShouldAllow_WhenPortfolioRiskIsAcceptable()
        {
            var engine = CreateEngine(maxDailyLossPercent: 10m);

            var result = engine.Evaluate(
                CreateTrade(pair: "EUR/GBP", positionSize: 10_000m, entry: 1.1000m, stop: 1.0950m),
                [],
                CreateAccount(equity: 100_000m));

            Assert.True(result.Allowed);
            Assert.Contains("Portfolio accepted", result.Reason);
        }

        [Fact]
        public void Evaluate_ShouldHandleLowercaseTradeStatusAndDirection()
        {
            var engine = CreateEngine(maxDailyLossPercent: 10m);

            var result = engine.Evaluate(
                CreateTrade(status: "open", direction: "long", pair: "EUR/GBP"),
                [],
                CreateAccount(equity: 100_000m));

            Assert.True(result.Allowed);
        }

        private static PortfolioEngine CreateEngine(
            int maxOpenTrades = 5,
            decimal maxDailyLossPercent = 5m)
        {
            var values = new Dictionary<string, string?>
            {
                ["TradingSystem:LiveTrading:MaxOpenTrades"] = maxOpenTrades.ToString(),
                ["TradingSystem:LiveTrading:MaxDailyLossPercent"] = maxDailyLossPercent.ToString(CultureInfo.CurrentCulture)
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(values)
                .Build();

            return new PortfolioEngine(new ConfigurationEngine(configuration));
        }

        private static TradePlan CreateTrade(
            string status = "OPEN",
            string direction = "LONG",
            string pair = "EUR/USD",
            decimal entry = 1.1000m,
            decimal stop = 1.0900m,
            decimal positionSize = 10_000m)
        {
            return new TradePlan
            {
                Status = status,
                Direction = direction,
                Pair = pair,
                EntryPrice = entry,
                StopLoss = stop,
                PositionSize = positionSize
            };
        }

        private static AccountContext CreateAccount(decimal equity = 100_000m)
        {
            return new AccountContext
            {
                Equity = equity
            };
        }
    }
}