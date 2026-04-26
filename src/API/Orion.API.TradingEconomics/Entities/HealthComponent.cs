using Microsoft.Extensions.Diagnostics.HealthChecks;
using Orion.API.TradingEconomics.Enum;

namespace Orion.API.TradingEconomics.Entities
{
    public sealed class HealthComponent
    {
        public string Name { get; set; } = string.Empty;

        public HealthComponentType Type { get; set; }

        public bool Critical { get; set; }

        public bool Enabled { get; set; } = true;

        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(5);

        public TimeSpan DegradedThreshold { get; set; } = TimeSpan.Zero;

        public TimeSpan? CheckInterval { get; set; }

        public DateTime LastCheckTime { get; set; }

        public HealthCheckResult? LastResult { get; set; }

        public int TotalChecks { get; set; }

        public int FailedChecks { get; set; }
    }
}