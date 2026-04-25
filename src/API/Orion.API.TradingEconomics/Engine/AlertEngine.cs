
using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine
{
    public sealed class AlertEngine
    {
        public List<TradingAlert> Evaluate(TradingDecision decision)
        {
            var alerts = new List<TradingAlert>();

            if (decision == null)
            {
                alerts.Add(TradingAlert.Critical("Decision is null."));
                return alerts;
            }

            if (decision.Direction == "NO_TRADE")
            {
                alerts.Add(TradingAlert.Info(decision.Reason));
                return alerts;
            }

            if (decision.RiskScore >= 80)
                alerts.Add(TradingAlert.Critical("Risk score is extremely high."));

            if (decision.RiskScore >= 60 && decision.RiskScore < 80)
                alerts.Add(TradingAlert.Warning("Risk score is elevated."));

            if (decision.Confidence < 40)
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
}