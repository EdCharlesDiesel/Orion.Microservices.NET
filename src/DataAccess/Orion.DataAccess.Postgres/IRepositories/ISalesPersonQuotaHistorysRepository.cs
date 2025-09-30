using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface ISalesPersonQuotaHistorysRepository
{
    Task<IEnumerable<SalesPersonQuotaHistory>> GetAllAsync();
    Task<SalesPersonQuotaHistory?> GetByIdAsync(int id);
    Task AddAsync(SalesPersonQuotaHistory entity);
    void Update(SalesPersonQuotaHistory entity);
    void Delete(SalesPersonQuotaHistory entity);
}