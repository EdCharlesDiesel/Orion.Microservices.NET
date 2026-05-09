using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Orion.API.TradingEconomics.Entities;

public class HealthSnapshot
{
    public DateTime Timestamp { get; set; }
    public HealthStatus Status { get; set; }
    public int ComponentCount { get; set; }
    public int HealthyCount { get; set; }
    public int DegradedCount { get; set; }
    public int UnhealthyCount { get; set; }
}