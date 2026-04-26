using Orion.API.TradingEconomics.Engine;
using Orion.API.TradingEconomics.Entities;
using Xunit;

namespace Orion.API.TradingEconomics.UnitTests.Engine
{
    public sealed class PerformanceAnalyticsEngineTests
    {
        private readonly PerformanceAnalyticsEngine _engine = new();

        [Fact]
        public void Analyze_ShouldReturnEmpty_WhenTradesIsNull()
        {
            var result = _engine.Analyze(null);

            Assert.Equal("No trades to analyze.", result.Verdict);
            Assert.Equal(0, result.TotalTrades);
        }

        [Fact]
        public void Analyze_ShouldReturnEmpty_WhenTradesAreEmpty()
        {
            var result = _engine.Analyze([]);

            Assert.Equal("No trades to analyze.", result.Verdict);
            Assert.Equal(0, result.TotalTrades);
        }

        [Fact]
        public void Analyze_ShouldReturnEmpty_WhenNoClosedTradesExist()
        {
            var result = _engine.Analyze(
            [
                CreateTrade("OPEN", 100m)
            ]);

            Assert.Equal("No closed trades to analyze.", result.Verdict);
            Assert.Equal(0, result.TotalTrades);
        }

        [Fact]
        public void Analyze_ShouldCalculatePerformanceMetrics()
        {
            var trades = new List<TradePlan>
            {
                CreateTrade("CLOSED", 100m, daysAgo: 4),
                CreateTrade("CLOSED", -50m, daysAgo: 3),
                CreateTrade("CLOSED", 200m, daysAgo: 2),
                CreateTrade("CLOSED", -25m, daysAgo: 1)
            };

            var result = _engine.Analyze(trades);

            Assert.Equal(4, result.TotalTrades);
            Assert.Equal(2, result.WinningTrades);
            Assert.Equal(2, result.LosingTrades);
            Assert.Equal(50m, result.WinRate);
            Assert.Equal(300m, result.GrossProfit);
            Assert.Equal(75m, result.GrossLoss);
            Assert.Equal(225m, result.NetProfit);
            Assert.Equal(150m, result.AverageWin);
            Assert.Equal(37.5m, result.AverageLoss);
            Assert.Equal(4m, result.ProfitFactor);
            Assert.Equal(56.25m, result.Expectancy);
            Assert.Equal(50m, result.MaxDrawdown);
            Assert.Equal("POSITIVE_EDGE", result.Verdict);
        }

        [Fact]
        public void Analyze_ShouldIgnoreOpenTrades()
        {
            var trades = new List<TradePlan>
            {
                CreateTrade("CLOSED", 100m),
                CreateTrade("OPEN", 9999m)
            };

            var result = _engine.Analyze(trades);

            Assert.Equal(1, result.TotalTrades);
            Assert.Equal(100m, result.NetProfit);
        }

        [Fact]
        public void Analyze_ShouldHandleLowercaseClosedStatus()
        {
            var result = _engine.Analyze(
            [
                CreateTrade("closed", 100m)
            ]);

            Assert.Equal(1, result.TotalTrades);
            Assert.Equal(100m, result.NetProfit);
        }

        [Fact]
        public void Analyze_ShouldReturnStrongEdge_WhenMetricsAreStrong()
        {
            var trades = new List<TradePlan>
            {
                CreateTrade("CLOSED", 100m),
                CreateTrade("CLOSED", 150m),
                CreateTrade("CLOSED", 200m),
                CreateTrade("CLOSED", -50m)
            };

            var result = _engine.Analyze(trades);

            Assert.Equal("STRONG_EDGE", result.Verdict);
        }

        [Fact]
        public void Analyze_ShouldReturnNoEdge_WhenPerformanceIsPoor()
        {
            var trades = new List<TradePlan>
            {
                CreateTrade("CLOSED", -100m),
                CreateTrade("CLOSED", 50m),
                CreateTrade("CLOSED", -100m)
            };

            var result = _engine.Analyze(trades);

            Assert.Equal("NO_EDGE", result.Verdict);
        }

        [Fact]
        public void Analyze_ShouldCalculateRiskReward_ForLongTrades()
        {
            var trades = new List<TradePlan>
            {
                CreateTrade(
                    "CLOSED",
                    100m,
                    direction: "LONG",
                    entry: 1.1000m,
                    stop: 1.0900m,
                    target: 1.1200m)
            };

            var result = _engine.Analyze(trades);

            Assert.Equal(2m, result.AverageRiskReward);
        }

        [Fact]
        public void Analyze_ShouldCalculateRiskReward_ForShortTrades()
        {
            var trades = new List<TradePlan>
            {
                CreateTrade(
                    "CLOSED",
                    100m,
                    direction: "SHORT",
                    entry: 1.1000m,
                    stop: 1.1100m,
                    target: 1.0800m)
            };

            var result = _engine.Analyze(trades);

            Assert.Equal(2m, result.AverageRiskReward);
        }

        private static TradePlan CreateTrade(
            string status,
            decimal profitLoss,
            int daysAgo = 0,
            string direction = "LONG",
            decimal entry = 1.1000m,
            decimal stop = 1.0900m,
            decimal target = 1.1200m)
        {
            return new TradePlan
            {
                Status = status,
                ProfitLoss = profitLoss,
                ClosedAt = DateTime.UtcNow.AddDays(-daysAgo),
                Direction = direction,
                EntryPrice = entry,
                StopLoss = stop,
                TakeProfit = target
            };
        }
    }
}