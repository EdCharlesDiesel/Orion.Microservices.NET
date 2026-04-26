using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Interfaces
{
    /// <summary>
    /// Runs and manages system health checks.
    /// </summary>
    public interface IHealthCheckEngine
    {
        void RegisterComponent(HealthComponent component);

        Task<HealthReport> RunHealthChecksAsync(CancellationToken cancellationToken = default);

        Task<HealthReport> GetCurrentHealthAsync();

        Task<HealthTrend> GetHealthTrendAsync(int hours = 24);

        Task<ComponentDetails?> GetComponentDetailsAsync(string componentName);

        void EnableComponent(string name, bool enabled);

        Task GracefulShutdownAsync();
    }
}