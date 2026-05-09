using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;

public class BuildVersionRepository : IBuildVersionRepository
{
    public BuildVersionRepository(OrionDbContext context)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<BuildVersion>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<BuildVersion?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(BuildVersion entity)
    {
        throw new NotImplementedException();
    }

    public void Update(BuildVersion entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(BuildVersion entity)
    {
        throw new NotImplementedException();
    }
}