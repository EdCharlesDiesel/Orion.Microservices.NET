using Microsoft.Extensions.Diagnostics.HealthChecks;
using Orion.API.TradingEconomics.Enum;

namespace Orion.API.TradingEconomics.Entities;

public class HealthComponent
{
    public string Name { get; set; }
    public HealthComponentType Type { get; set; }
    public bool Critical { get; set; }
    public bool Enabled { get; set; } = true;
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(10);
    public TimeSpan DegradedThreshold { get; set; } = TimeSpan.FromSeconds(5);
    public TimeSpan? CheckInterval { get; set; }
    public DateTime LastCheckTime { get; set; }
    public HealthCheckResult? LastResult { get; set; }
    public long TotalChecks { get; set; }
    public long FailedChecks { get; set; }
}