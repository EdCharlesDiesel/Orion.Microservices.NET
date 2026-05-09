
using Orion.API.TradingEconomics.Engine;
using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Enum;
using Xunit;

namespace Orion.API.TradingEconomics.UnitTests.Engine;

public sealed class ComplianceEngineTests
{
    private readonly ComplianceEngine _engine = new();

    [Fact]
    public void Validate_WhenPairMissing_Throws()
    {
        Assert.Throws<ArgumentException>(() =>
            _engine.Validate("", "LONG", 100, ValidAccount(), AllowedRisk()));
    }

    [Fact]
    public void Validate_WhenDirectionMissing_Throws()
    {
        Assert.Throws<ArgumentException>(() =>
            _engine.Validate("EUR/USD", "", 100, ValidAccount(), AllowedRisk()));
    }

    [Fact]
    public void Validate_WhenRequestedSizeInvalid_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            _engine.Validate("EUR/USD", "LONG", 0, ValidAccount(), AllowedRisk()));
    }

    [Fact]
    public void Validate_WhenInvalidDirection_ReturnsRejected()
    {
        var result = _engine.Validate("EUR/USD", "BUY", 100, ValidAccount(), AllowedRisk());

        Assert.False(result.IsApproved);
        Assert.Equal(ComplianceDecision.Rejected, result.Decision);
        Assert.Contains("Direction must be LONG or SHORT.", result.Violations);
    }

    [Fact]
    public void Validate_WhenRiskDisallows_ReturnsRejected()
    {
        var risk = new RealTimeRiskResult
        {
            IsAllowed = false,
            Violations = new List<string> { "Max drawdown exceeded." }
        };

        var result = _engine.Validate("EUR/USD", "LONG", 100, ValidAccount(), risk);

        Assert.False(result.IsApproved);
        Assert.Contains("Max drawdown exceeded.", result.Violations);
    }

    [Fact]
    public void Validate_WhenAccountInvalid_ReturnsRejected()
    {
        var account = new AccountSnapshot
        {
            Balance = 0,
            Equity = 0,
            FreeMargin = -1
        };

        var result = _engine.Validate("EUR/USD", "LONG", 100, account, AllowedRisk());

        Assert.False(result.IsApproved);
        Assert.Contains("Account balance must be greater than zero.", result.Violations);
        Assert.Contains("Account equity must be greater than zero.", result.Violations);
        Assert.Contains("Free margin cannot be negative.", result.Violations);
    }

    [Fact]
    public void Validate_WhenRequestedSizeExceedsEquity_ReturnsRejected()
    {
        var result = _engine.Validate("EUR/USD", "LONG", 2000, ValidAccount(), AllowedRisk());

        Assert.False(result.IsApproved);
        Assert.Contains("Requested size exceeds account equity.", result.Violations);
    }

    [Fact]
    public void Validate_WhenValid_ReturnsApproved()
    {
        var result = _engine.Validate(" eur/usd ", " long ", 100, ValidAccount(), AllowedRisk());

        Assert.True(result.IsApproved);
        Assert.Equal(ComplianceDecision.Approved, result.Decision);
        Assert.Equal("EUR/USD", result.Pair);
        Assert.Equal("LONG", result.Direction);
        Assert.Empty(result.Violations);
    }

    private static AccountSnapshot ValidAccount()
    {
        return new AccountSnapshot
        {
            Balance = 1000,
            Equity = 1000,
            FreeMargin = 500
        };
    }

    private static RealTimeRiskResult AllowedRisk()
    {
        return new RealTimeRiskResult
        {
            IsAllowed = true,
            Violations = new List<string>()
        };
    }
}