using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Repositories;

namespace Orion.DataAccess.Postgres.Tools;

public static class DatabaseServiceCollectionExtensions
{
    public static IServiceCollection AddPostgresDataAccess(this IServiceCollection services, IConfiguration config)
    {
        // add the DbContext
        services.AddDbContext<OrionDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}