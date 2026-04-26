using Moq;
using Orion.API.TradingEconomics.Engine;
using Orion.API.TradingEconomics.Engine.Interfaces.Orion.API.TradingEconomics.Engine.Interfaces;
using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Interfaces;
using Xunit;

namespace Orion.API.TradingEconomics.UnitTests.Engine
{
    public sealed class ExecutionEngineTests
    {
        private readonly Mock<IMarketDataEngine> _market = new();
        private readonly Mock<IExecutionCostModel> _cost = new();

        private ExecutionEngine Create()
            => new(_market.Object, _cost.Object);

        [Fact]
        public async Task ExecuteAsync_ShouldThrow_WhenPairInvalid()
        {
            var engine = Create();

            await Assert.ThrowsAsync<ArgumentException>(() =>
                engine.ExecuteAsync("", "LONG", 1));
        }

        [Fact(Skip = "")]
        public async Task ExecuteAsync_ShouldReturnOrder()
        {
            _market.Setup(x => x.GetLatestTickAsync("EURUSD", It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MarketTick { Bid = 1.1m, Ask = 1.2m });
            
            _cost.Setup(x => x.EstimateSpread(It.IsAny<string>(), 1.1m, 1.2m)).Returns(0.1m);
            _cost.Setup(x => x.EstimateSlippage(It.IsAny<string>(), 1m)).Returns(0.01m);
            
            var engine = Create();
            
            var result = await engine.ExecuteAsync("EURUSD", "LONG", 1);
            
            Assert.Equal("LONG", result.Direction);
            Assert.True(result.ExecutedPrice > 0);
        }

        [Fact]
        public void Execute_ShouldThrow_WhenOrderBookNull()
        {
            var engine = Create();

            Assert.Throws<ArgumentNullException>(() =>
                engine.Execute(null!, "LONG", 1));
        }

        [Fact]
        public void Execute_ShouldFillOrder()
        {
            var engine = Create();

            var book = new OrderBook
            {
                Pair = "EURUSD",
                Asks = new List<OrderBookLevel>
                {
                    new() { Price = 1.2m, Volume = 5 }
                }
            };

            var result = engine.Execute(book, "LONG", 1);

            Assert.Equal("LONG", result.Direction);
            Assert.Equal(1, result.FilledSize);
        }

        [Fact]
        public void Execute_ShouldThrow_WhenNoLiquidity()
        {
            var engine = Create();

            var book = new OrderBook
            {
                Pair = "EURUSD",
                Asks = []
            };

            Assert.Throws<InvalidOperationException>(() =>
                engine.Execute(book, "LONG", 1));
        }
    }
}