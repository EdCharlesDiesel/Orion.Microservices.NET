using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories
{
    public class DatabaseLogRepository(OrionDbContext context) : IDatabaseLogRepository
    {
        public async Task<IEnumerable<DatabaseLog>> GetAllAsync() =>
            await context.DatabaseLogs.ToListAsync();

        public async Task<DatabaseLog?> GetByIdAsync(int id) =>
            await context.DatabaseLogs.FindAsync(id);

        public async Task AddAsync(DatabaseLog entity) =>
            await context.DatabaseLogs.AddAsync(entity);

        public void Update(DatabaseLog entity) =>
            context.DatabaseLogs.Update(entity);

        public void Delete(DatabaseLog entity) =>
            context.DatabaseLogs.Remove(entity);
    }
}