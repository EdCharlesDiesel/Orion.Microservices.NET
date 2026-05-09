using Orion.API.TradingEconomics.Engine;
using Orion.API.TradingEconomics.Entities;
using Xunit;

namespace Orion.API.TradingEconomics.UnitTests.Engine
{
    public sealed class TradeLifecycleEngineTests
    {
        private readonly TradeLifecycleEngine _engine = new();

        [Fact]
        public void CreatePlan_ShouldThrow_WhenSignalIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _engine.CreatePlan(null!, CreateRisk(), CreateSize(), CreateExecution(), CreateExit()));
        }

        [Fact]
        public void CreatePlan_ShouldReturnRejected_WhenSignalIsNoTrade()
        {
            var result = _engine.CreatePlan(
                new SignalResult { Direction = "NO_TRADE", Reason = "No setup." },
                CreateRisk(),
                CreateSize(),
                CreateExecution(),
                CreateExit());

            Assert.Equal("REJECTED", result.Status);
            Assert.Equal("No setup.", result.Reason);
        }

        [Fact]
        public void CreatePlan_ShouldReturnRejected_WhenRiskIsNotAllowed()
        {
            var result = _engine.CreatePlan(
                CreateSignal(),
                new RiskResult { IsAllowed = false, Reason = "Risk blocked." },
                CreateSize(),
                CreateExecution(),
                CreateExit());

            Assert.Equal("REJECTED", result.Status);
            Assert.Equal("Risk blocked.", result.Reason);
        }

        [Fact]
        public void CreatePlan_ShouldReturnRejected_WhenSizeIsNotAllowed()
        {
            var result = _engine.CreatePlan(
                CreateSignal(),
                CreateRisk(),
                new PositionSizeResult { IsAllowed = false, Reason = "Size blocked." },
                CreateExecution(),
                CreateExit());

            Assert.Equal("REJECTED", result.Status);
            Assert.Equal("Size blocked.", result.Reason);
        }

        [Fact]
        public void CreatePlan_ShouldReturnRejected_WhenExecutionHasNoFill()
        {
            var execution = CreateExecution();
            execution.FilledSize = 0m;

            var result = _engine.CreatePlan(
                CreateSignal(),
                CreateRisk(),
                CreateSize(),
                execution,
                CreateExit());

            Assert.Equal("REJECTED", result.Status);
            Assert.Equal("Execution failed. No filled size.", result.Reason);
        }

        [Fact]
        public void CreatePlan_ShouldCreateOpenTrade()
        {
            var result = _engine.CreatePlan(
                CreateSignal(),
                CreateRisk(),
                CreateSize(),
                CreateExecution(),
                CreateExit());

            Assert.Equal("OPEN", result.Status);
            Assert.Equal("EURUSD", result.Pair);
            Assert.Equal("LONG", result.Direction);
            Assert.Equal(1.1000m, result.EntryPrice);
            Assert.Equal(10_000m, result.PositionSize);
            Assert.Equal(1.0900m, result.StopLoss);
            Assert.Equal(1.1200m, result.TakeProfit);
        }

        [Fact]
        public void Update_ShouldThrow_WhenTradeIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _engine.Update(null!, CreateCandle()));
        }

        [Fact]
        public void Update_ShouldReturnSameTrade_WhenTradeIsNotOpen()
        {
            var trade = CreateTrade(status: "CLOSED");

            var result = _engine.Update(trade, CreateCandle());

            Assert.Same(trade, result);
        }

        [Fact]
        public void Update_ShouldCloseLongTrade_WhenStopLossIsHit()
        {
            var trade = CreateTrade(direction: "LONG");

            var result = _engine.Update(
                trade,
                CreateCandle(low: 1.0890m, high: 1.1100m));

            Assert.Equal("CLOSED", result.Status);
            Assert.Equal("STOP_LOSS", result.CloseReason);
            Assert.Equal(1.0900m, result.ExitPrice);
        }

        [Fact]
        public void Update_ShouldCloseLongTrade_WhenTakeProfitIsHit()
        {
            var trade = CreateTrade(direction: "LONG");

            var result = _engine.Update(
                trade,
                CreateCandle(low: 1.1000m, high: 1.1210m));

            Assert.Equal("CLOSED", result.Status);
            Assert.Equal("TAKE_PROFIT", result.CloseReason);
            Assert.Equal(1.1200m, result.ExitPrice);
        }

        [Fact]
        public void Update_ShouldCloseShortTrade_WhenStopLossIsHit()
        {
            var trade = CreateTrade(direction: "SHORT", stopLoss: 1.1100m, takeProfit: 1.0800m);

            var result = _engine.Update(
                trade,
                CreateCandle(low: 1.0900m, high: 1.1110m));

            Assert.Equal("CLOSED", result.Status);
            Assert.Equal("STOP_LOSS", result.CloseReason);
            Assert.Equal(1.1100m, result.ExitPrice);
        }

        [Fact]
        public void Update_ShouldCloseShortTrade_WhenTakeProfitIsHit()
        {
            var trade = CreateTrade(direction: "SHORT", stopLoss: 1.1100m, takeProfit: 1.0800m);

            var result = _engine.Update(
                trade,
                CreateCandle(low: 1.0790m, high: 1.1000m));

            Assert.Equal("CLOSED", result.Status);
            Assert.Equal("TAKE_PROFIT", result.CloseReason);
            Assert.Equal(1.0800m, result.ExitPrice);
        }

        [Fact]
        public void Update_ShouldKeepTradeOpen_WhenNoExitLevelIsHit()
        {
            var trade = CreateTrade(direction: "LONG");

            var result = _engine.Update(
                trade,
                CreateCandle(low: 1.0950m, high: 1.1150m));

            Assert.Same(trade, result);
            Assert.Equal("OPEN", result.Status);
        }

        private static SignalResult CreateSignal()
        {
            return new SignalResult
            {
                Direction = "LONG",
                Reason = "Signal accepted"
            };
        }

        private static RiskResult CreateRisk()
        {
            return new RiskResult
            {
                IsAllowed = true,
                Reason = "Risk accepted"
            };
        }

        private static PositionSizeResult CreateSize()
        {
            return new PositionSizeResult
            {
                IsAllowed = true,
                Reason = "Size accepted"
            };
        }

        private static ExecutionOrder CreateExecution()
        {
            return new ExecutionOrder
            {
                Pair = "eurusd",
                Direction = "long",
                ExecutedPrice = 1.1000m,
                FilledSize = 10_000m,
                Timestamp = DateTime.UtcNow
            };
        }

        private static ExitPlan CreateExit()
        {
            return new ExitPlan
            {
                StopLoss = 1.0900m,
                TakeProfit = 1.1200m
            };
        }

        private static TradePlan CreateTrade(
            string status = "OPEN",
            string direction = "LONG",
            decimal stopLoss = 1.0900m,
            decimal takeProfit = 1.1200m)
        {
            return new TradePlan
            {
                Status = status,
                Direction = direction,
                StopLoss = stopLoss,
                TakeProfit = takeProfit
            };
        }

        private static OhlcvBar CreateCandle(
            decimal low = 1.0950m,
            decimal high = 1.1150m)
        {
            return new OhlcvBar
            {
                TimestampUtc = DateTime.UtcNow,
                Low = low,
                High = high
            };
        }
    }
}