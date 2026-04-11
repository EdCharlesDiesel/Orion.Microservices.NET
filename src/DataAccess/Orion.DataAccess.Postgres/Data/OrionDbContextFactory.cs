using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Orion.DataAccess.Postgres.Data
{
    public class OrionDbContextFactory : IDesignTimeDbContextFactory<OrionDbContext>
    {
        public OrionDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<OrionDbContext>();

            // Use the same connection string you’d normally configure
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=OrionProductionDatabase;Username=postgres;Password=$ta99Ath0");

            return new OrionDbContext(optionsBuilder.Options);
        }
    }
}