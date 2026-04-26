using Moq;
using Orion.API.TradingEconomics.Engine;
using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Interfaces;
using Xunit;
using YahooQuotesApi;

namespace Orion.API.TradingEconomics.UnitTests.Engine
{
    public class ExecutionEngineTests
    {
        private readonly Mock<IMarketDataFeed> _mockMarket;
        private readonly Mock<IExecutionCostModel> _mockCost;
        private readonly ExecutionEngine _engine;

        public ExecutionEngineTests()
        {
            _mockMarket = new Mock<IMarketDataFeed>();
            _mockCost = new Mock<IExecutionCostModel>();
            _engine = new ExecutionEngine(_mockMarket.Object, _mockCost.Object);
        }

        [Fact]
        public async Task ExecuteAsync_NullPair_ThrowsArgumentException()
        {
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _engine.ExecuteAsync(null!, "LONG", 1000));
        }

        [Fact]
        public async Task ExecuteAsync_EmptyDirection_ThrowsArgumentException()
        {
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _engine.ExecuteAsync("EURUSD", "", 1000));
        }

        [Fact]
        public async Task ExecuteAsync_InvalidDirection_ThrowsArgumentException()
        {
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _engine.ExecuteAsync("EURUSD", "INVALID", 1000));
        }

        [Fact]
        public async Task ExecuteAsync_ZeroSize_ThrowsArgumentOutOfRangeException()
        {
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                _engine.ExecuteAsync("EURUSD", "LONG", 0));
        }

        [Fact]
        public async Task ExecuteAsync_NegativeSize_ThrowsArgumentOutOfRangeException()
        {
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                _engine.ExecuteAsync("EURUSD", "LONG", -100));
        }

        [Fact]
        public async Task ExecuteAsync_NoMarketTick_ThrowsInvalidOperationException()
        {
            _mockMarket.Setup(m => m.GetLatestTickAsync("EURUSD", It.IsAny<CancellationToken>()))
                .ReturnsAsync((Tick?)null);
            
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _engine.ExecuteAsync("EURUSD", "LONG", 1000));
        }

        [Fact]
        public async Task ExecuteAsync_InvalidBid_ThrowsInvalidOperationException()
        {
            var tick = new Tick { Pair = "EURUSD", Bid = 0, Ask = 1.2m };
            _mockMarket.Setup(m => m.GetLatestTickAsync("EURUSD", It.IsAny<CancellationToken>()))
                .ReturnsAsync(tick);
            
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _engine.ExecuteAsync("EURUSD", "LONG", 1000));
        }

        [Fact]
        public async Task ExecuteAsync_InvalidAsk_ThrowsInvalidOperationException()
        {
            var tick = new Tick { Pair = "EURUSD", Bid = 1.1m, Ask = 0 };
            _mockMarket.Setup(m => m.GetLatestTickAsync("EURUSD", It.IsAny<CancellationToken>()))
                .ReturnsAsync(tick);
            
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _engine.ExecuteAsync("EURUSD", "LONG", 1000));
        }

        [Fact]
        public async Task ExecuteAsync_AskBelowBid_ThrowsInvalidOperationException()
        {
            var tick = new Tick { Pair = "EURUSD", Bid = 1.2m, Ask = 1.1m };
            _mockMarket.Setup(m => m.GetLatestTickAsync("EURUSD", It.IsAny<CancellationToken>()))
                .ReturnsAsync(tick);
            
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _engine.ExecuteAsync("EURUSD", "LONG", 1000));
        }

        [Fact]
        public async Task ExecuteAsync_LongDirection_UsesAskPrice()
        {
            var tick = new Tick { Pair = "EURUSD", Bid = 1.1000m, Ask = 1.1010m };
            _mockMarket.Setup(m => m.GetLatestTickAsync("EURUSD", It.IsAny<CancellationToken>()))
                .ReturnsAsync(tick);
            _mockCost.Setup(c => c.EstimateSlippage("EURUSD", 1000)).Returns(0.0002m);
            
            var result = await _engine.ExecuteAsync("EURUSD", "LONG", 1000);
            
            Assert.Equal("LONG", result.Direction);
            Assert.Equal(tick.Ask + 0.0002m, result.ExecutedPrice);
        }

        [Fact]
        public async Task ExecuteAsync_ShortDirection_UsesBidPrice()
        {
            var tick = new Tick { Pair = "EURUSD", Bid = 1.1000m, Ask = 1.1010m };
            _mockMarket.Setup(m => m.GetLatestTickAsync("EURUSD", It.IsAny<CancellationToken>()))
                .ReturnsAsync(tick);
            _mockCost.Setup(c => c.EstimateSlippage("EURUSD", 1000)).Returns(0.0002m);
            
            var result = await _engine.ExecuteAsync("EURUSD", "SHORT", 1000);
            
            Assert.Equal("SHORT", result.Direction);
            Assert.Equal(tick.Bid - 0.0002m, result.ExecutedPrice);
        }

        [Fact]
        public async Task ExecuteAsync_CalculatesMidPriceCorrectly()
        {
            var tick = new Tick { Pair = "EURUSD", Bid = 1.1000m, Ask = 1.1020m };
            _mockMarket.Setup(m => m.GetLatestTickAsync("EURUSD", It.IsAny<CancellationToken>()))
                .ReturnsAsync(tick);
            
            var result = await _engine.ExecuteAsync("EURUSD", "LONG", 1000);
            
            Assert.Equal(1.1010m, result.RequestedPrice);
        }

        [Fact]
        public void Execute_NullOrderBook_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _engine.Execute(null!, "LONG", 1000));
        }

        [Fact]
        public void Execute_InvalidDirection_ThrowsArgumentException()
        {
            var orderBook = CreateValidOrderBook();
            Assert.Throws<ArgumentException>(() =>
                _engine.Execute(orderBook, "INVALID", 1000));
        }

        [Fact]
        public void Execute_NoLiquidityForLong_ThrowsInvalidOperationException()
        {
            var orderBook = new OrderBook
            {
                Pair = "EURUSD",
                Asks = new List<OrderBookLevel>(),
                Bids = CreateBidLevels()
            };
            
            Assert.Throws<InvalidOperationException>(() =>
                _engine.Execute(orderBook, "LONG", 1000));
        }

        [Fact]
        public void Execute_LongDirection_FillsFromAsks()
        {
            var orderBook = CreateOrderBookWithLiquidity();
            var result = _engine.Execute(orderBook, "LONG", 1500);
            
            Assert.Equal(1500, result.FilledSize);
            Assert.Equal(1.1015m, result.ExecutedPrice);
        }

        [Fact]
        public void Execute_ShortDirection_FillsFromBids()
        {
            var orderBook = CreateOrderBookWithLiquidity();
            var result = _engine.Execute(orderBook, "SHORT", 1200);
            
            Assert.Equal(1200, result.FilledSize);
            Assert.Equal(1.0990m, result.ExecutedPrice);
        }

        [Fact]
        public void Execute_PartialFill_ReturnsFilledAmount()
        {
            var orderBook = CreateOrderBookWithLiquidity();
            var result = _engine.Execute(orderBook, "LONG", 5000);
            
            Assert.Equal(3000, result.FilledSize); // Only 3000 available
        }

        [Fact]
        public void Execute_CalculatesSpreadCost()
        {
            var orderBook = CreateOrderBookWithLiquidity();
            var result = _engine.Execute(orderBook, "LONG", 1000);
            
            Assert.Equal(0.0005m, result.SpreadCost);
        }

        [Fact]
        public void Execute_CalculatesSlippageCost()
        {
            var orderBook = CreateOrderBookWithLiquidity();
            var result = _engine.Execute(orderBook, "LONG", 2500);
            
            Assert.True(result.SlippageCost > 0);
        }

        [Fact]
        public void Execute_SkipsInvalidLevels()
        {
            var orderBook = new OrderBook
            {
                Pair = "EURUSD",
                Asks = new List<OrderBookLevel>
                {
                    new OrderBookLevel { Price = 0, Volume = 1000 },
                    new OrderBookLevel { Price = 1.1010m, Volume = -500 },
                    new OrderBookLevel { Price = 1.1015m, Volume = 1000 }
                },
                Bids = CreateBidLevels()
            };
            
            var result = _engine.Execute(orderBook, "LONG", 1000);
            Assert.Equal(1000, result.FilledSize);
        }

        [Fact]
        public void Constructor_NullMarketData_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new ExecutionEngine(null!, _mockCost.Object));
        }

        [Fact]
        public void Constructor_NullCostModel_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new ExecutionEngine(_mockMarket.Object, null!));
        }

        private static OrderBook CreateValidOrderBook()
        {
            return new OrderBook
            {
                Pair = "EURUSD",
                Asks = new List<OrderBookLevel>(),
                Bids = new List<OrderBookLevel>()
            };
        }

        private static OrderBook CreateOrderBookWithLiquidity()
        {
            return new OrderBook
            {
                Pair = "EURUSD",
                Bids = new List<OrderBookLevel>
                {
                    new OrderBookLevel { Price = 1.0995m, Volume = 1000 },
                    new OrderBookLevel { Price = 1.0990m, Volume = 800 },
                    new OrderBookLevel { Price = 1.0985m, Volume = 600 }
                },
                Asks = new List<OrderBookLevel>
                {
                    new OrderBookLevel { Price = 1.1010m, Volume = 1000 },
                    new OrderBookLevel { Price = 1.1015m, Volume = 1000 },
                    new OrderBookLevel { Price = 1.1020m, Volume = 1000 }
                }
            };
        }

        private static List<OrderBookLevel> CreateBidLevels()
        {
            return new List<OrderBookLevel>
            {
                new OrderBookLevel { Price = 1.0995m, Volume = 1000 },
                new OrderBookLevel { Price = 1.0990m, Volume = 800 }
            };
        }
    }
}