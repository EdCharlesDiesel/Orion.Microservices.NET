using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface ISalesReasonsRepository
{
    Task<IEnumerable<SalesReason>> GetAllAsync();
    Task<SalesReason?> GetByIdAsync(int id);
    Task AddAsync(SalesReason entity);
    void Update(SalesReason entity);
    void Delete(SalesReason entity);
}