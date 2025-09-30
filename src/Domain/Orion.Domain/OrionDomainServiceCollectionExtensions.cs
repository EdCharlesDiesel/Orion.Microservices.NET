using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Orion.Domain.Security;

namespace Orion.Domain;

public static class OrionDomainServiceCollectionExtensions
{
    public static IServiceCollection RegisterBusinessServices(
        this IServiceCollection services,
        IConfiguration config) // ✅ add config here
    {
        services.Configure<PayFastOptions>(config.GetSection("PayFast"));
        services.AddScoped<PayFastService>();
        services.AddScoped<OzowService>();
        services.AddHttpClient<PeachPaymentsService>();

        return services;
    }
}