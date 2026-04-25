using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Orion.API.TradingEconomics.Entities;

public class HealthReport
{
    public DateTime Timestamp { get; set; }
    public HealthStatus OverallStatus { get; set; }
    public Dictionary<string, HealthCheckResult> Checks { get; set; }
    public SystemMetrics Metrics { get; set; }
    public string Version { get; set; } = "1.0.0";
}