using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface IScrapReasonsRepository
{
    Task<IEnumerable<ScrapReason>> GetAllAsync();
    Task<ScrapReason?> GetByIdAsync(int id);
    Task AddAsync(ScrapReason entity);
    void Update(ScrapReason entity);
    void Delete(ScrapReason entity);
}