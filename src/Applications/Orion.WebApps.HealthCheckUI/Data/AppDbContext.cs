using Microsoft.EntityFrameworkCore;
using Orion.WebApps.HealthCheckUI.Models;

namespace Orion.WebApps.HealthCheckUI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<HealthRecord> HealthRecords => Set<HealthRecord>();
}