using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories
{
    public class BuildVersionRepository(OrionDbContext context) : IBuildVersionRepository
    {
        public async Task<IEnumerable<BuildVersion>> GetAllAsync() =>
            await context.BuildVersions.ToListAsync();

        public async Task<BuildVersion?> GetByIdAsync(int id) =>
            await context.BuildVersions.FindAsync(id);

        public async Task AddAsync(BuildVersion entity) =>
            await context.BuildVersions.AddAsync(entity);

        public void Update(BuildVersion entity) =>
            context.BuildVersions.Update(entity);

        public void Delete(BuildVersion entity) =>
            context.BuildVersions.Remove(entity);
    }
}