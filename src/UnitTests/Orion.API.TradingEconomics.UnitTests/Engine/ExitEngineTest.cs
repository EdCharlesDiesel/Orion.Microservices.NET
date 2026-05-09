using Orion.API.TradingEconomics.Engine;
using Orion.API.TradingEconomics.Entities;
using Xunit;

namespace Orion.API.TradingEconomics.UnitTests.Engine
{
    public class ExitEngineTests
    {
        private readonly ExitEngine _engine;

        public ExitEngineTests()
        {
            _engine = new ExitEngine();
        }

        [Fact]
        public void ShouldExit_NullPosition_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _engine.ShouldExit(null!, new Candle(), out _));
        }

        [Fact]
        public void ShouldExit_NullCandle_ThrowsArgumentNullException()
        {
            var position = CreateLongPosition(1.1000m, 1.0950m, 1.1100m);
            Assert.Throws<ArgumentNullException>(() =>
                _engine.ShouldExit(position, null!, out _));
        }

        [Theory]
        [InlineData("INVALID")]
        [InlineData("")]
        [InlineData(null)]
        public void ShouldExit_InvalidDirection_ThrowsArgumentException(string? direction)
        {
            var position = CreateLongPosition(1.1000m, 1.0950m, 1.1100m);
            position.Direction = direction;
            
            Assert.Throws<ArgumentException>(() =>
                _engine.ShouldExit(position, new Candle(), out _));
        }

        [Fact]
        public void ShouldExit_LongPosition_StopLossHit_ReturnsTrue()
        {
            var position = CreateLongPosition(1.1000m, 1.0950m, 1.1100m);
            var candle = new Candle { Low = 1.0940m, High = 1.1050m };
            
            var shouldExit = _engine.ShouldExit(position, candle, out var exitPrice);
            
            Assert.True(shouldExit);
            Assert.Equal(1.0950m, exitPrice);
        }

        [Fact]
        public void ShouldExit_LongPosition_TakeProfitHit_ReturnsTrue()
        {
            var position = CreateLongPosition(1.1000m, 1.0950m, 1.1100m);
            var candle = new Candle { Low = 1.1020m, High = 1.1110m };
            
            var shouldExit = _engine.ShouldExit(position, candle, out var exitPrice);
            
            Assert.True(shouldExit);
            Assert.Equal(1.1100m, exitPrice);
        }

        [Fact]
        public void ShouldExit_LongPosition_NoExitConditions_ReturnsFalse()
        {
            var position = CreateLongPosition(1.1000m, 1.0950m, 1.1100m);
            var candle = new Candle { Low = 1.1010m, High = 1.1080m };
            
            var shouldExit = _engine.ShouldExit(position, candle, out var exitPrice);
            
            Assert.False(shouldExit);
            Assert.Equal(0m, exitPrice);
        }

        [Fact]
        public void ShouldExit_ShortPosition_StopLossHit_ReturnsTrue()
        {
            var position = CreateShortPosition(1.1000m, 1.1050m, 1.0900m);
            var candle = new Candle { High = 1.1060m, Low = 1.0980m };
            
            var shouldExit = _engine.ShouldExit(position, candle, out var exitPrice);
            
            Assert.True(shouldExit);
            Assert.Equal(1.1050m, exitPrice);
        }

        [Fact]
        public void ShouldExit_ShortPosition_TakeProfitHit_ReturnsTrue()
        {
            var position = CreateShortPosition(1.1000m, 1.1050m, 1.0900m);
            var candle = new Candle { High = 1.1020m, Low = 1.0890m };
            
            var shouldExit = _engine.ShouldExit(position, candle, out var exitPrice);
            
            Assert.True(shouldExit);
            Assert.Equal(1.0900m, exitPrice);
        }

        [Fact]
        public void ShouldExit_ShortPosition_NoExitConditions_ReturnsFalse()
        {
            var position = CreateShortPosition(1.1000m, 1.1050m, 1.0900m);
            var candle = new Candle { High = 1.1030m, Low = 1.0930m };
            
            var shouldExit = _engine.ShouldExit(position, candle, out _);
            
            Assert.False(shouldExit);
        }

        [Fact]
        public void Calculate_NullSignal_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _engine.Calculate(null!, new ExecutionOrder(), new RiskResult(), null));
        }

        [Fact]
        public void Calculate_NullExecution_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _engine.Calculate(new SignalResult(), null!, new RiskResult(), null));
        }

        [Fact]
        public void Calculate_NullRisk_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _engine.Calculate(new SignalResult(), new ExecutionOrder(), null!, null));
        }

        [Fact]
        public void Calculate_InvalidExecutionDirection_ThrowsArgumentException()
        {
            var execution = new ExecutionOrder { Direction = "INVALID", ExecutedPrice = 1.1000m };
            
            Assert.Throws<ArgumentException>(() =>
                _engine.Calculate(new SignalResult(), execution, new RiskResult(), null));
        }

        [Fact]
        public void Calculate_ZeroExecutedPrice_ThrowsArgumentException()
        {
            var execution = new ExecutionOrder { Direction = "LONG", ExecutedPrice = 0 };
            
            Assert.Throws<ArgumentException>(() =>
                _engine.Calculate(new SignalResult(), execution, new RiskResult(), null));
        }

        [Fact]
        public void Calculate_LongPosition_UsesRiskStopLossDistance()
        {
            var execution = new ExecutionOrder { Direction = "LONG", ExecutedPrice = 1.1000m };
            var risk = new RiskResult { StopLossDistance = 0.0100m, TakeProfitDistance = 0.0200m };
            
            var result = _engine.Calculate(new SignalResult(), execution, risk, null);
            
            Assert.Equal(1.0900m, result.StopLoss);
            Assert.Equal(1.1200m, result.TakeProfit);
            Assert.Equal(2m, result.RiskRewardRatio);
        }

        [Fact]
        public void Calculate_ShortPosition_UsesRiskStopLossDistance()
        {
            var execution = new ExecutionOrder { Direction = "SHORT", ExecutedPrice = 1.1000m };
            var risk = new RiskResult { StopLossDistance = 0.0100m, TakeProfitDistance = 0.0200m };
            
            var result = _engine.Calculate(new SignalResult(), execution, risk, null);
            
            Assert.Equal(1.1100m, result.StopLoss);
            Assert.Equal(1.0800m, result.TakeProfit);
        }

        [Fact]
        public void Calculate_NoStopDistance_UsesDefaultPercentage()
        {
            var execution = new ExecutionOrder { Direction = "LONG", ExecutedPrice = 1.1000m };
            var risk = new RiskResult();
            
            var result = _engine.Calculate(new SignalResult(), execution, risk, null);
            
            var expectedStopDistance = 1.1000m * 0.005m;
            Assert.Equal(1.1000m - expectedStopDistance, result.StopLoss);
        }

        [Fact]
        public void Calculate_NoTakeProfitDistance_UsesDoubleStopDistance()
        {
            var execution = new ExecutionOrder { Direction = "LONG", ExecutedPrice = 1.1000m };
            var risk = new RiskResult();
            
            var result = _engine.Calculate(new SignalResult(), execution, risk, null);
            
            var stopDistance = 1.1000m * 0.005m;
            var expectedTakeProfit = 1.1000m + (stopDistance * 2);
            Assert.Equal(expectedTakeProfit, result.TakeProfit);
        }

        [Fact]
        public void Calculate_HighConfidence_ReducesTrailingStopDistance()
        {
            var execution = new ExecutionOrder { Direction = "LONG", ExecutedPrice = 1.1000m };
            var risk = new RiskResult { StopLossDistance = 0.0100m };
            var indicators = new List<NormalizedIndicator>
            {
                new NormalizedIndicator { Name = "CONFIDENCE", Value = 0.8m }
            };
            
            var result = _engine.Calculate(new SignalResult(), execution, risk, indicators);
            
            Assert.Equal(0.0075m, result.TrailingStopDistance);
        }

        [Fact]
        public void Calculate_LowConfidence_UsesFullStopDistance()
        {
            var execution = new ExecutionOrder { Direction = "LONG", ExecutedPrice = 1.1000m };
            var risk = new RiskResult { StopLossDistance = 0.0100m };
            var indicators = new List<NormalizedIndicator>
            {
                new NormalizedIndicator { Name = "CONFIDENCE", Value = 0.5m }
            };
            
            var result = _engine.Calculate(new SignalResult(), execution, risk, indicators);
            
            Assert.Equal(0.0100m, result.TrailingStopDistance);
        }

        [Fact]
        public void Calculate_NoConfidenceIndicator_UsesFullStopDistance()
        {
            var execution = new ExecutionOrder { Direction = "LONG", ExecutedPrice = 1.1000m };
            var risk = new RiskResult { StopLossDistance = 0.0100m };
            
            var result = _engine.Calculate(new SignalResult(), execution, risk, null);
            
            Assert.Equal(0.0100m, result.TrailingStopDistance);
        }

        [Fact]
        public void Calculate_SetsTimestampToUtc()
        {
            var execution = new ExecutionOrder { Direction = "LONG", ExecutedPrice = 1.1000m };
            var beforeTime = DateTime.UtcNow;
            
            var result = _engine.Calculate(new SignalResult(), execution, new RiskResult(), null);
            
            Assert.True(result.CreatedAtUtc >= beforeTime);
            Assert.Equal(DateTimeKind.Utc, result.CreatedAtUtc.Kind);
        }

        private static OpenPosition CreateLongPosition(decimal entry, decimal stopLoss, decimal takeProfit)
        {
            return new OpenPosition
            {
                Direction = "LONG",
                EntryPrice = entry,
                StopLoss = stopLoss,
                TakeProfit = takeProfit
            };
        }

        private static OpenPosition CreateShortPosition(decimal entry, decimal stopLoss, decimal takeProfit)
        {
            return new OpenPosition
            {
                Direction = "SHORT",
                EntryPrice = entry,
                StopLoss = stopLoss,
                TakeProfit = takeProfit
            };
        }
    }
}