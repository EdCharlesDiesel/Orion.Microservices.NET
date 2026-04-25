using Orion.API.TradingEconomics.Engine;
using Orion.API.TradingEconomics.Enum;

namespace Orion.API.TradingEconomics.Entities;

public sealed class RealTimeRiskResult
{
    public string Pair { get; set; } = "";
    public string Direction { get; set; } = "";
    public decimal AccountBalance { get; set; }
    public decimal AccountEquity { get; set; }
    public decimal DrawdownPercent { get; set; }
    public decimal DailyLossPercent { get; set; }
    public decimal PositionRiskAmount { get; set; }
    public decimal PositionRiskPercent { get; set; }
    public decimal SpreadPercent { get; set; }
    public RiskAction Action { get; set; }
    public bool IsAllowed { get; set; }
    public List<string> Violations { get; set; } = new();
    public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
}