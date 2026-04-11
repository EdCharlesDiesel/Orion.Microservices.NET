using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface IAwBuildVersionRepository
{
    Task<IEnumerable<AWBuildVersion>> GetAllAsync();
    Task<AWBuildVersion?> GetByIdAsync(int id);
    Task AddAsync(AWBuildVersion entity);
    void Update(AWBuildVersion entity);
    void Delete(AWBuildVersion entity);
}