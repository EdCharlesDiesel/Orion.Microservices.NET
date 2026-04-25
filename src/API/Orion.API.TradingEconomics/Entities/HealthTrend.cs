using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Orion.API.TradingEconomics.Entities;

public class HealthTrend
{
    public List<HealthSnapshot> Snapshots { get; set; }
    public decimal UptimePercentage { get; set; }
    public TimeSpan MeanTimeToRecovery { get; set; }
    public Dictionary<HealthStatus, int> StatusDistribution { get; set; }
}