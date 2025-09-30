using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Orion.API.Dashboard;

public static class HealthCheckExtensions
{
    public static IServiceCollection AddCustomHealthChecks(
        this IServiceCollection services, string connectionString)
    {
        services.AddHealthChecks()
            // .AddNpgSql(connectionString, name: "database") // ✅ correct extension
            .AddCheck("self", () => HealthCheckResult.Healthy());

        return services;
    }
}
