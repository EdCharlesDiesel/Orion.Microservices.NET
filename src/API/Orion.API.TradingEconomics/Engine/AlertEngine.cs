using Orion.API.TradingEconomics.Engine.Interfaces;
using Orion.API.TradingEconomics.Entities;
namespace Orion.API.TradingEconomics.Engine;

/// <summary>
/// Evaluates a trading decision and produces risk, validation, and informational alerts.
/// </summary>
public sealed class AlertEngine: IAlertEngine
{
    private const decimal CriticalRiskThreshold = 80m;
    private const decimal WarningRiskThreshold = 60m;
    private const decimal LowConfidenceThreshold = 40m;

    /// <summary>
    /// Evaluates the supplied trading decision and returns applicable alerts.
    /// </summary>
    /// <param name="decision">The trading decision to evaluate.</param>
    /// <returns>A list of alerts describing validation, risk, or status issues.</returns>
    public List<TradingAlert> Evaluate(TradingDecision? decision)
    {
        var alerts = new List<TradingAlert>();

        if (decision is null)
        {
            alerts.Add(TradingAlert.Critical("Decision is null."));
            return alerts;
        }

        if (string.Equals(decision.Direction, "NO_TRADE", StringComparison.OrdinalIgnoreCase))
        {
            alerts.Add(TradingAlert.Info(
                string.IsNullOrWhiteSpace(decision.Reason)
                    ? "No trade decision."
                    : decision.Reason));

            return alerts;
        }

        if (decision.RiskScore >= CriticalRiskThreshold)
            alerts.Add(TradingAlert.Critical("Risk score is extremely high."));
        else if (decision.RiskScore >= WarningRiskThreshold)
            alerts.Add(TradingAlert.Warning("Risk score is elevated."));

        if (decision.Confidence < LowConfidenceThreshold)
            alerts.Add(TradingAlert.Warning("Signal confidence is low."));

        if (decision.PositionSize <= 0)
            alerts.Add(TradingAlert.Critical("Position size is invalid."));

        if (decision.StopLoss <= 0)
            alerts.Add(TradingAlert.Critical("Stop loss is missing or invalid."));

        if (decision.TakeProfit <= 0)
            alerts.Add(TradingAlert.Warning("Take profit is missing or invalid."));

        if (decision.EntryPrice <= 0)
            alerts.Add(TradingAlert.Critical("Entry price is invalid."));

        if (alerts.Count == 0)
            alerts.Add(TradingAlert.Info("No alerts. Trade decision is within acceptable limits."));

        return alerts;
    }
}