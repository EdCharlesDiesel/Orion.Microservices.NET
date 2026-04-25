using Orion.API.TradingEconomics.Enum;

namespace Orion.API.TradingEconomics.Entities;

public sealed class ComplianceResult
{
    public string Pair { get; set; } = "";
    public string Direction { get; set; } = "";
    public decimal RequestedSize { get; set; }
    public bool IsApproved { get; set; }
    public ComplianceDecision Decision { get; set; }
    public List<string> Violations { get; set; } = new();
    public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
}