using Orion.API.TradingEconomics.Engine;
using Orion.API.TradingEconomics.Entities;
using Xunit;

namespace Orion.API.TradingEconomics.UnitTests.Engine
{
    public sealed class ModelValidationEngineTests
    {
        private readonly ModelValidationEngine _engine = new();

        [Fact]
        public void Validate_ShouldFail_WhenPerformanceIsNull()
        {
            var result = _engine.Validate(null!, CreateTrades(30, 100m));

            Assert.False(result.IsValid);
            Assert.Equal("Performance report is null.", result.Reason);
        }

        [Fact]
        public void Validate_ShouldFail_WhenTradesAreNull()
        {
            var result = _engine.Validate(CreateGoodPerformance(), null!);

            Assert.False(result.IsValid);
            Assert.Equal("No trades available for validation.", result.Reason);
        }

        [Fact]
        public void Validate_ShouldFail_WhenTradesAreEmpty()
        {
            var result = _engine.Validate(CreateGoodPerformance(), []);

            Assert.False(result.IsValid);
            Assert.Equal("No trades available for validation.", result.Reason);
        }

        [Fact]
        public void Validate_ShouldFail_WhenClosedTradesAreLessThanThirty()
        {
            var result = _engine.Validate(CreateGoodPerformance(), CreateTrades(29, 100m));

            Assert.False(result.IsValid);
            Assert.Equal("At least 30 closed trades are required for validation.", result.Reason);
        }

        [Fact]
        public void Validate_ShouldReturnProductionReady_WhenModelIsStrong()
        {
            var result = _engine.Validate(CreateGoodPerformance(), CreateTrades(30, 100m));

            Assert.True(result.IsValid);
            Assert.Equal("PRODUCTION_READY", result.Verdict);
            Assert.True(result.Score >= 0.80m);
        }

        [Fact]
        public void Validate_ShouldReturnRejected_WhenPerformanceIsPoor()
        {
            var trades = CreateTrades(30, -100m);
            var performance = new PerformanceReport
            {
                NetProfit = -1000m,
                MaxDrawdown = 2000m,
                ProfitFactor = 0.8m,
                Expectancy = -10m,
                WinRate = 30m,
                AverageRiskReward = 0.8m
            };

            var result = _engine.Validate(performance, trades);

            Assert.False(result.IsValid);
            Assert.Equal("REJECTED", result.Verdict);
        }

        [Fact]
        public void Validate_ShouldIgnoreOpenTrades()
        {
            var trades = CreateTrades(30, 100m);
            trades.Add(new TradePlan
            {
                Status = "OPEN",
                ProfitLoss = 100_000m,
                ClosedAt = DateTime.UtcNow.AddDays(1)
            });

            var result = _engine.Validate(CreateGoodPerformance(), trades);

            Assert.True(result.IsValid);
            Assert.Equal("PRODUCTION_READY", result.Verdict);
        }

        [Fact]
        public void Validate_ShouldHandleLowercaseClosedStatus()
        {
            var trades = CreateTrades(30, 100m);

            foreach (var trade in trades)
                trade.Status = "closed";

            var result = _engine.Validate(CreateGoodPerformance(), trades);

            Assert.True(result.IsValid);
        }

        private static PerformanceReport CreateGoodPerformance()
        {
            return new PerformanceReport
            {
                NetProfit = 10_000m,
                MaxDrawdown = 1_000m,
                ProfitFactor = 1.8m,
                Expectancy = 50m,
                WinRate = 60m,
                AverageRiskReward = 1.5m
            };
        }

        private static List<TradePlan> CreateTrades(int count, decimal profitLoss)
        {
            return Enumerable.Range(1, count)
                .Select(x => new TradePlan
                {
                    Status = "CLOSED",
                    ProfitLoss = profitLoss,
                    ClosedAt = DateTime.UtcNow.AddDays(-count + x)
                })
                .ToList();
        }
    }
}