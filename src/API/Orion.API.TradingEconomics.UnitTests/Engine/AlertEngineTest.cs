using JetBrains.Annotations;
using Orion.API.TradingEconomics.Engine;
using Xunit;
using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.UnitTests.Engine;
[TestSubject(typeof(AlertEngine))]
public sealed class AlertEngineTests
{
    private readonly AlertEngine _engine = new();

    [Fact]
    public void Evaluate_WhenDecisionIsNull_ReturnsCriticalAlert()
    {
        var alerts = _engine.Evaluate(null);

        Assert.Single(alerts);
        Assert.Contains(alerts, x => x.Message == "Decision is null.");
    }

    [Fact]
    public void Evaluate_WhenNoTrade_ReturnsInfoAlert()
    {
        var decision = new TradingDecision
        {
            Direction = "NO_TRADE",
            Reason = "Market conditions are not favorable."
        };

        var alerts = _engine.Evaluate(decision);

        Assert.Single(alerts);
        Assert.Contains(alerts, x => x.Message == "Market conditions are not favorable.");
    }

    [Fact]
    public void Evaluate_WhenRiskScoreIsCritical_ReturnsCriticalRiskAlert()
    {
        var decision = CreateValidDecision();
        decision.RiskScore = 85;

        var alerts = _engine.Evaluate(decision);

        Assert.Contains(alerts, x => x.Message == "Risk score is extremely high.");
    }

    [Fact]
    public void Evaluate_WhenRiskScoreIsWarning_ReturnsWarningRiskAlert()
    {
        var decision = CreateValidDecision();
        decision.RiskScore = 65;

        var alerts = _engine.Evaluate(decision);

        Assert.Contains(alerts, x => x.Message == "Risk score is elevated.");
    }

    [Fact]
    public void Evaluate_WhenConfidenceIsLow_ReturnsWarningAlert()
    {
        var decision = CreateValidDecision();
        decision.Confidence = 25;

        var alerts = _engine.Evaluate(decision);

        Assert.Contains(alerts, x => x.Message == "Signal confidence is low.");
    }

    [Fact]
    public void Evaluate_WhenPositionSizeIsInvalid_ReturnsCriticalAlert()
    {
        var decision = CreateValidDecision();
        decision.PositionSize = 0;

        var alerts = _engine.Evaluate(decision);

        Assert.Contains(alerts, x => x.Message == "Position size is invalid.");
    }

    [Fact]
    public void Evaluate_WhenDecisionIsValid_ReturnsNoAlertsInfo()
    {
        var decision = CreateValidDecision();

        var alerts = _engine.Evaluate(decision);

        Assert.Single(alerts);
        Assert.Contains(alerts, x => x.Message == "No alerts. Trade decision is within acceptable limits.");
    }

    private static TradingDecision CreateValidDecision()
    {
        return new TradingDecision
        {
            Pair = "EUR/USD",
            Direction = "BUY",
            Reason = "Valid setup.",
            RiskScore = 30,
            Confidence = 75,
            PositionSize = 1,
            EntryPrice = 1.1000m,
            StopLoss = 1.0950m,
            TakeProfit = 1.1100m
        };
    }
}