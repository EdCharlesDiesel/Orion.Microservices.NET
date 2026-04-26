using Microsoft.Extensions.Configuration;
using Orion.API.TradingEconomics.Engine;
using Orion.API.TradingEconomics.Entities;
using Xunit;

namespace Orion.API.TradingEconomics.UnitTests.Engine
{
    public sealed class OrderManagementEngineTests
    {
        [Fact]
        public void Constructor_ShouldThrow_WhenConfigIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new OrderManagementEngine(null!));
        }

        [Fact]
        public void CreateOrder_ShouldThrow_WhenTradeIsNull()
        {
            var engine = CreateEngine();

            Assert.Throws<ArgumentNullException>(() =>
                engine.CreateOrder(null!, new PositionSizeResult(), new AccountContext()));
        }

        [Fact]
        public void CreateOrder_ShouldReject_WhenTradeIsNotOpen()
        {
            var engine = CreateEngine();

            var result = engine.CreateOrder(
                CreateTrade(status: "CLOSED"),
                CreateSize(),
                new AccountContext());

            Assert.Equal("REJECTED", result.Status);
            Assert.Equal("Trade is not open.", result.Reason);
        }

        [Fact]
        public void CreateOrder_ShouldReject_WhenPositionSizeIsInvalid()
        {
            var engine = CreateEngine();

            var result = engine.CreateOrder(
                CreateTrade(),
                new PositionSizeResult { PositionSize = 0m },
                new AccountContext());

            Assert.Equal("REJECTED", result.Status);
            Assert.Equal("Invalid position size.", result.Reason);
        }

        [Fact]
        public void CreateOrder_ShouldReject_WhenLiveTradingIsDisabled()
        {
            var engine = CreateEngine(liveEnabled: false);

            var result = engine.CreateOrder(
                CreateTrade(),
                CreateSize(),
                new AccountContext());

            Assert.Equal("REJECTED", result.Status);
            Assert.Equal("Live trading is disabled.", result.Reason);
        }

        [Fact]
        public void CreateOrder_ShouldCreatePaperOrder_WhenPaperTradingOnlyIsEnabled()
        {
            var engine = CreateEngine(liveEnabled: true, paperOnly: true);

            var result = engine.CreateOrder(
                CreateTrade(),
                CreateSize(),
                new AccountContext());

            Assert.Equal("PAPER_ORDER", result.Status);
            Assert.Equal("EURUSD", result.Pair);
            Assert.Equal("LONG", result.Direction);
            Assert.Equal("MARKET", result.OrderType);
            Assert.Equal(10000m, result.Quantity);
        }

        [Fact]
        public void CreateOrder_ShouldCreateLiveOrder_WhenPaperTradingOnlyIsDisabled()
        {
            var engine = CreateEngine(liveEnabled: true, paperOnly: false);

            var result = engine.CreateOrder(
                CreateTrade(),
                CreateSize(),
                new AccountContext());

            Assert.Equal("LIVE_ORDER", result.Status);
        }

        [Fact]
        public void ValidateFill_ShouldThrow_WhenOrderIsNull()
        {
            var engine = CreateEngine();

            Assert.Throws<ArgumentNullException>(() =>
                engine.ValidateFill(null!, new ExecutionOrder()));
        }

        [Fact]
        public void ValidateFill_ShouldReject_WhenOrderWasRejected()
        {
            var engine = CreateEngine();

            var result = engine.ValidateFill(
                OrderRequest.Rejected("Bad order."),
                new ExecutionOrder());

            Assert.Equal("REJECTED", result.Status);
            Assert.Equal("Bad order.", result.Reason);
        }

        [Fact]
        public void ValidateFill_ShouldReject_WhenFilledSizeIsZero()
        {
            var engine = CreateEngine();

            var result = engine.ValidateFill(
                CreateOrder(),
                new ExecutionOrder { FilledSize = 0m });

            Assert.Equal("REJECTED", result.Status);
            Assert.Equal("Order was not filled.", result.Reason);
        }

        [Fact]
        public void ValidateFill_ShouldReject_WhenFillRatioIsTooLow()
        {
            var engine = CreateEngine(minFillRatio: 0.90m);

            var result = engine.ValidateFill(
                CreateOrder(quantity: 100m),
                new ExecutionOrder
                {
                    Pair = "EURUSD",
                    Direction = "LONG",
                    FilledSize = 50m,
                    ExecutedPrice = 1.1m,
                    Timestamp = DateTime.UtcNow
                });

            Assert.Equal("REJECTED", result.Status);
            Assert.Contains("Fill ratio too low", result.Reason);
        }

        [Fact]
        public void ValidateFill_ShouldReturnFilled_WhenExecutionIsValid()
        {
            var engine = CreateEngine(minFillRatio: 0.80m);

            var result = engine.ValidateFill(
                CreateOrder(quantity: 100m),
                new ExecutionOrder
                {
                    Pair = "EURUSD",
                    Direction = "LONG",
                    FilledSize = 100m,
                    ExecutedPrice = 1.1m,
                    Timestamp = DateTime.UtcNow
                });

            Assert.Equal("FILLED", result.Status);
            Assert.Equal(100m, result.RequestedQuantity);
            Assert.Equal(100m, result.FilledQuantity);
            Assert.Equal(1.1m, result.AverageFillPrice);
        }

        [Fact]
        public void Cancel_ShouldReturnCancelledState()
        {
            var engine = CreateEngine();

            var result = engine.Cancel(CreateOrder(), "User cancelled.");

            Assert.Equal("CANCELLED", result.Status);
            Assert.Equal("User cancelled.", result.Reason);
            Assert.Equal(0m, result.FilledQuantity);
            Assert.Null(result.FilledAt);
        }

        private static OrderManagementEngine CreateEngine(
            bool liveEnabled = true,
            bool paperOnly = true,
            decimal minFillRatio = 0.80m)
        {
            var values = new Dictionary<string, string?>
            {
                ["TradingSystem:LiveTrading:Enabled"] = liveEnabled.ToString(),
                ["TradingSystem:LiveTrading:PaperTradingOnly"] = paperOnly.ToString(),
                ["TradingSystem:DefaultPairConfig:Enabled"] = "true",
                ["TradingSystem:Execution:MinFillRatio"] = minFillRatio.ToString()
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(values)
                .Build();

            return new OrderManagementEngine(new ConfigurationEngine(configuration));
        }

        private static TradePlan CreateTrade(string status = "OPEN")
        {
            return new TradePlan
            {
                Status = status,
                Pair = "eurusd",
                Direction = "long",
                EntryPrice = 1.1000m,
                StopLoss = 1.0900m,
                TakeProfit = 1.1200m,
                Reason = "Test trade"
            };
        }

        private static PositionSizeResult CreateSize()
        {
            return new PositionSizeResult
            {
                PositionSize = 10000m
            };
        }

        private static OrderRequest CreateOrder(decimal quantity = 100m)
        {
            return new OrderRequest
            {
                Status = "LIVE_ORDER",
                Pair = "EURUSD",
                Direction = "LONG",
                Quantity = quantity,
                Reason = "Test order"
            };
        }
    }
}