using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories
{
    public class AwBuildVersionRepository(OrionDbContext context) : IAwBuildVersionRepository
    {
        public async Task<IEnumerable<AWBuildVersion>> GetAllAsync() =>
            await context.AwbuildVersions.ToListAsync();

        public async Task<AWBuildVersion?> GetByIdAsync(int id) =>
            await context.AwbuildVersions.FindAsync(id);

        public async Task AddAsync(AWBuildVersion entity) =>
            await context.AwbuildVersions.AddAsync(entity);

        public void Update(AWBuildVersion entity) =>
            context.AwbuildVersions.Update(entity);

        public void Delete(AWBuildVersion entity) =>
            context.AwbuildVersions.Remove(entity);
    }
}