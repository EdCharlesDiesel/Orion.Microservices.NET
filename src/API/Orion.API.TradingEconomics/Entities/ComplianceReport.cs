using Orion.API.TradingEconomics.Engine;

namespace Orion.API.TradingEconomics.Entities;

public class ComplianceReport
{
    public DateTime GeneratedAt { get; set; }
    public DateRange Period { get; set; }
    public string Pair { get; set; }
    public int TotalDecisions { get; set; }
    public Dictionary<string, int> TradesByDirection { get; set; }
    public decimal AverageConfidence { get; set; }
    public Dictionary<DateTime, int> DecisionDistribution { get; set; }
}