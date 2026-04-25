using Microsoft.Extensions.Diagnostics.HealthChecks;
using Orion.API.TradingEconomics.Enum;

namespace Orion.API.TradingEconomics.Entities;

public class ComponentDetails
{
    public string Name { get; set; }
    public HealthComponentType Type { get; set; }
    public bool Critical { get; set; }
    public DateTime LastCheck { get; set; }
    public HealthCheckResult? LastResult { get; set; }
    public long TotalChecks { get; set; }
    public long FailedChecks { get; set; }
    public double SuccessRate { get; set; }
}