using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.IRepositories;
using Orion.DataAccess.Postgres.Repositories;
using Orion.DataAccess.Postgres.Tools;

namespace Orion.API.Production
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterBusinessServices(
            this IServiceCollection services)
        {
            // services.AddScoped<IEmployeeService, EmployeeService>();
            // services.AddScoped<IPromotionService, PromotionService>();
            // services.AddScoped<EmployeeFactory>(); 
            return services;
        }
    
        public static IServiceCollection RegisterHumanResourcesServices(
            this IServiceCollection services, IConfiguration configuration)
        {
            // Add the DbContext with PostgresSQL
            services.AddDbContext<OrionDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            // Register repositories
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            // Add other repositories as needed
            return services;
        }
    }
}