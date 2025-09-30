using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface IBuildVersionRepository
{
    Task<IEnumerable<BuildVersion>> GetAllAsync();
    Task<BuildVersion?> GetByIdAsync(int id);
    Task AddAsync(BuildVersion entity);
    void Update(BuildVersion entity);
    void Delete(BuildVersion entity);
}