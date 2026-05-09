using Orion.API.TradingEconomics.Engine;
using Orion.API.TradingEconomics.Entities;
using Xunit;

namespace Orion.API.TradingEconomics.UnitTests.Engine;

public sealed class CircuitBreakerEngineTests
{
    private static CircuitBreakerEngine CreateEngine(decimal maxLoss = 5, int maxTrades = 3)
    {
        throw new NotImplementedException();
        // var config = new Mock<ConfigurationEngine>();
        // config.Setup(x => x.GetConfig())
        //     .Returns(
        //         new AppConfiguration()
        //     { 
        //         // LiveTrading = new LiveTradingConfig
        //         // {
        //         //     MaxDailyLossPercent = maxLoss,
        //         //     MaxOpenTrades = maxTrades
        //         // }
        //     }
        //         );

        // return new CircuitBreakerEngine(config.Object);
    }

    [Fact]
    public void Evaluate_InvalidEquity_Trips()
    {
        var engine = CreateEngine();

        var result = engine.Evaluate(
            new AccountContext { Equity = 0 },
            null,
            null,
            null);

        Assert.True(result.IsTripped);
    }

    [Fact]
    public void Evaluate_DataQualityFail_Trips()
    {
        var engine = CreateEngine();

        var result = engine.Evaluate(
            new AccountContext { Equity = 1000 },
            null,
            null,
            new DataQualityResult { IsValid = false, Reason = "Bad data" });

        Assert.True(result.IsTripped);
    }

    [Fact]
    public void Evaluate_DailyLossExceeded_Trips()
    {
        var engine = CreateEngine(5);

        var result = engine.Evaluate(
            new AccountContext { Equity = 1000 },
            new List<TradePlan>
            {
                new() { Status = "CLOSED", ProfitLoss = -100 }
            },
            null,
            new DataQualityResult { IsValid = true });

        Assert.True(result.IsTripped);
    }

    [Fact]
    public void Evaluate_MaxOpenTradesExceeded_Trips()
    {
        var engine = CreateEngine(maxTrades: 1);

        var result = engine.Evaluate(
            new AccountContext { Equity = 1000 },
            null,
            new List<TradePlan>
            {
                new() { Status = "OPEN" },
                new() { Status = "OPEN" }
            },
            new DataQualityResult { IsValid = true });

        Assert.True(result.IsTripped);
    }

    [Fact]
    public void Evaluate_OpenRiskTooHigh_Trips()
    {
        var engine = CreateEngine(5);

        var result = engine.Evaluate(
            new AccountContext { Equity = 1000 },
            null,
            new List<TradePlan>
            {
                new()
                {
                    Status = "OPEN",
                    Direction = "LONG",
                    EntryPrice = 1,
                    StopLoss = 0.5m,
                    PositionSize = 1000
                }
            },
            new DataQualityResult { IsValid = true });

        Assert.True(result.IsTripped);
    }

    [Fact]
    public void Evaluate_AllGood_Clear()
    {
        var engine = CreateEngine();

        var result = engine.Evaluate(
            new AccountContext { Equity = 1000 },
            new List<TradePlan>(),
            new List<TradePlan>(),
            new DataQualityResult { IsValid = true });

        Assert.False(result.IsTripped);
    }
}