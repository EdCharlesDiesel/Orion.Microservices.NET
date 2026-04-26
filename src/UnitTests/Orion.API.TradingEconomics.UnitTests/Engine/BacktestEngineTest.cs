using MediatR;
using Moq;
using Orion.API.TradingEconomics.Commands;
using Orion.API.TradingEconomics.Engine;
using Orion.API.TradingEconomics.Entities;
using Xunit;
namespace Orion.API.TradingEconomics.UnitTests.Engine;
public sealed class BacktestEngineTests
{
    [Fact]
    public async Task RunAsync_WhenValidInput_ReturnsTrades()
    {
        var mediator = new Mock<IMediator>();
        var execution = new Mock<AdvancedExecutionEngine>(null!, null!, null!, null!);

        mediator.Setup(x => x.Send(It.IsAny<BuildPortfolioCommand>(), default))
            .ReturnsAsync(new List<PortfolioPosition>
            {
                new()
                {
                    Pair = "EUR/USD",
                    Direction = "LONG",
                    PositionSize = 1
                }
            });

        execution.Setup(x => x.ExecuteAsync("EUR/USD", "LONG", 1, default))
            .ReturnsAsync(new ExecutionResult
            {
                Order = new ExecutionOrder
                {
                    ExecutedPrice = 1.1m,
                    FilledSize = 1
                }
            });

        var engine = new BacktestEngine(mediator.Object, execution.Object, new Random(42));

        var result = await engine.RunAsync(DateTime.UtcNow.Date, DateTime.UtcNow.Date, 1000);

        Assert.Single(result);
        Assert.Equal("EUR/USD", result[0].Pair);
    }

    [Fact]
    public async Task RunAsync_WhenNoPortfolio_ReturnsEmptyTrades()
    {
        var mediator = new Mock<IMediator>();
        var execution = new Mock<AdvancedExecutionEngine>(null!, null!, null!, null!);

        mediator.Setup(x => x.Send(It.IsAny<BuildPortfolioCommand>(), default))
            .ReturnsAsync(new List<PortfolioPosition>());

        var engine = new BacktestEngine(mediator.Object, execution.Object);

        var result = await engine.RunAsync(DateTime.UtcNow.Date, DateTime.UtcNow.Date, 1000);

        Assert.Empty(result);
    }

    [Fact]
    public async Task RunAsync_WhenExecutionReturnsNoFill_SkipsTrade()
    {
        var mediator = new Mock<IMediator>();
        var execution = new Mock<AdvancedExecutionEngine>(null!, null!, null!, null!);

        mediator.Setup(x => x.Send(It.IsAny<BuildPortfolioCommand>(), default))
            .ReturnsAsync(new List<PortfolioPosition>
            {
                new() { Pair = "EUR/USD", Direction = "LONG", PositionSize = 1 }
            });

        execution.Setup(x => x.ExecuteAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>(), default))
            .ReturnsAsync(new ExecutionResult
            {
                Order = new ExecutionOrder { FilledSize = 0 }
            });

        var engine = new BacktestEngine(mediator.Object, execution.Object);

        var result = await engine.RunAsync(DateTime.UtcNow.Date, DateTime.UtcNow.Date, 1000);

        Assert.Empty(result);
    }

    [Fact]
    public async Task RunAsync_WhenEndBeforeStart_Throws()
    {
        var engine = new BacktestEngine(
            Mock.Of<IMediator>(),
            Mock.Of<AdvancedExecutionEngine>());

        await Assert.ThrowsAsync<ArgumentException>(() =>
            engine.RunAsync(DateTime.UtcNow, DateTime.UtcNow.AddDays(-1), 1000));
    }

    [Fact]
    public async Task RunAsync_WhenCapitalInvalid_Throws()
    {
        var engine = new BacktestEngine(
            Mock.Of<IMediator>(),
            Mock.Of<AdvancedExecutionEngine>());

        await Assert.ThrowsAsync<ArgumentException>(() =>
            engine.RunAsync(DateTime.UtcNow, DateTime.UtcNow, 0));
    }
}