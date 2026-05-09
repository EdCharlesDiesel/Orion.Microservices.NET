
using Orion.API.TradingEconomics.Engine;
using Xunit;
using Moq;
using Orion.API.TradingEconomics.Interfaces;
using Orion.API.TradingEconomics.Entities;
namespace Orion.API.TradingEconomics.UnitTests.Engine;


public class AdvancedExecutionEngineTests
{
    private class TestExecutionEngine : AdvancedExecutionEngine
    {
        public TestExecutionEngine(
            IOrderBookProvider orderBook,
            ILatencyModel latency,
            INewsEventService news,
            IOrderBookExecutionService executor)
            : base(orderBook, latency, news, executor)
        {
        }
    }

    [Fact]
    public async Task ExecuteAsync_Should_Return_FullFill_When_Liquidity_Sufficient()
    {
        // Arrange
        var orderBookMock = new Mock<IOrderBookProvider>();
        var latencyMock = new Mock<ILatencyModel>();
        var newsMock = new Mock<INewsEventService>();
        var executorMock = new Mock<IOrderBookExecutionService>();

        latencyMock.Setup(x => x.SimulateLatencyMsAsync())
                   .ReturnsAsync(1);

        newsMock.Setup(x => x.IsHighImpactEventAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(false);

        var book = new OrderBook { Pair = "EURUSD" };

        orderBookMock.Setup(x => x.GetOrderBookAsync("EURUSD"))
                     .ReturnsAsync(book);

        executorMock.Setup(x => x.Execute(book, "BUY", 100))
            .Returns(new ExecutionOrder
            {
                Pair = "EURUSD",
                RequestedSize = 100,
                FilledSize = 100
            });

        var engine = new TestExecutionEngine(
            orderBookMock.Object,
            latencyMock.Object,
            newsMock.Object,
            executorMock.Object);

        // Act
        var result = await engine.ExecuteAsync("EURUSD", "BUY", 100);

        // Assert
        Assert.False(result.PartialFill);
        Assert.Equal(1m, result.FillRatio);
        Assert.False(result.HighImpactEvent);
    }

    [Fact]
    public async Task ExecuteAsync_Should_Reduce_Size_On_HighImpact_News()
    {
        // Arrange
        var orderBookMock = new Mock<IOrderBookProvider>();
        var latencyMock = new Mock<ILatencyModel>();
        var newsMock = new Mock<INewsEventService>();
        var executorMock = new Mock<IOrderBookExecutionService>();

        latencyMock.Setup(x => x.SimulateLatencyMsAsync())
                   .ReturnsAsync(1);

        newsMock.Setup(x => x.IsHighImpactEventAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(true);

        var book = new OrderBook { Pair = "EURUSD" };

        orderBookMock.Setup(x => x.GetOrderBookAsync("EURUSD"))
                     .ReturnsAsync(book);

        executorMock.Setup(x => x.Execute(book, "BUY", 50)) // size halved
            .Returns(new ExecutionOrder
            {
                Pair = "EURUSD",
                RequestedSize = 50,
                FilledSize = 50
            });

        var engine = new TestExecutionEngine(
            orderBookMock.Object,
            latencyMock.Object,
            newsMock.Object,
            executorMock.Object);

        // Act
        var result = await engine.ExecuteAsync("EURUSD", "BUY", 100);

        // Assert
        Assert.True(result.HighImpactEvent);
        Assert.Equal(1m, result.FillRatio);
    }

    [Fact]
    public async Task ExecuteAsync_Should_Throw_When_OrderBook_Null()
    {
        var orderBookMock = new Mock<IOrderBookProvider>();
        var latencyMock = new Mock<ILatencyModel>();
        var newsMock = new Mock<INewsEventService>();
        var executorMock = new Mock<IOrderBookExecutionService>();

        latencyMock.Setup(x => x.SimulateLatencyMsAsync()).ReturnsAsync(1);
        newsMock.Setup(x => x.IsHighImpactEventAsync(It.IsAny<DateTime>())).ReturnsAsync(false);

        orderBookMock.Setup(x => x.GetOrderBookAsync(It.IsAny<string>()))
                     .ReturnsAsync((OrderBook?)null);

        var engine = new TestExecutionEngine(
            orderBookMock.Object,
            latencyMock.Object,
            newsMock.Object,
            executorMock.Object);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            engine.ExecuteAsync("EURUSD", "BUY", 100));
    }
}