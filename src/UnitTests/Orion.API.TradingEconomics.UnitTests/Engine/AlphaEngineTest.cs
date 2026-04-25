using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Orion.API.TradingEconomics.Engine;
using Orion.API.TradingEconomics.Entities;
using Xunit;
namespace Orion.API.TradingEconomics.UnitTests.Engine;



public sealed class AlphaEngineTests
{
    private readonly AlphaEngine _engine = new();

    [Fact]
    public void Generate_WhenPairIsMissing_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            _engine.Generate("", []));
    }

    [Fact]
    public void Generate_WhenIndicatorsAreNull_ReturnsFlatResult()
    {
        var result = _engine.Generate("eur/usd", null);

        Assert.Equal("EUR/USD", result.Pair);
        Assert.Equal("FLAT", result.Direction);
        Assert.Equal(0m, result.AlphaScore);
        Assert.Equal(0m, result.Confidence);
    }

    [Fact]
    public void Generate_WhenScoreIsBullish_ReturnsLong()
    {
        var result = _engine.Generate("EUR/USD", new List<NormalizedIndicator>
        {
            new() { Name = "TREND", Value = 1m },
            new() { Name = "MOMENTUM", Value = 1m },
            new() { Name = "SENTIMENT", Value = 1m },
            new() { Name = "MACRO", Value = 1m }
        });

        Assert.Equal("LONG", result.Direction);
        Assert.True(result.AlphaScore >= 0.25m);
    }

    [Fact]
    public void Generate_WhenScoreIsBearish_ReturnsShort()
    {
        var result = _engine.Generate("EUR/USD", new List<NormalizedIndicator>
        {
            new() { Name = "TREND", Value = -1m },
            new() { Name = "MOMENTUM", Value = -1m },
            new() { Name = "SENTIMENT", Value = -1m },
            new() { Name = "MACRO", Value = -1m }
        });

        Assert.Equal("SHORT", result.Direction);
        Assert.True(result.AlphaScore <= -0.25m);
    }

    [Fact]
    public void Generate_WhenVolatilityIsHigh_AppliesPenalty()
    {
        var result = _engine.Generate("EUR/USD", new List<NormalizedIndicator>
        {
            new() { Name = "TREND", Value = 1m },
            new() { Name = "VOLATILITY", Value = 1m }
        });

        Assert.Equal(0.10m, result.VolatilityPenalty);
        Assert.Equal(0.20m, result.AlphaScore);
        Assert.Equal("FLAT", result.Direction);
    }

    [Fact]
    public void Generate_WhenHighImpactEventAndLowConfidence_ForcesFlat()
    {
        var result = _engine.Generate(
            "EUR/USD",
            new List<NormalizedIndicator>
            {
                new() { Name = "TREND", Value = 1m }
            },
            new List<MacroEvent>
            {
                new() { Impact = "HIGH" }
            });

        Assert.Equal("FLAT", result.Direction);
        Assert.Equal(1, result.HighImpactMacroEvents);
    }

    [Fact]
    public void Generate_WhenIndicatorNamesHaveDifferentCasing_StillCalculates()
    {
        var result = _engine.Generate("EUR/USD", new List<NormalizedIndicator>
        {
            new() { Name = "trend", Value = 1m },
            new() { Name = "momentum", Value = 1m }
        });

        Assert.Equal(0.55m, result.AlphaScore);
        Assert.Equal("LONG", result.Direction);
    }
}