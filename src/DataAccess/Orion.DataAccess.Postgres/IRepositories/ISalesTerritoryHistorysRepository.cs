using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface ISalesTerritoryHistorysRepository
{
    Task<IEnumerable<SalesTerritoryHistory>> GetAllAsync();
    Task<SalesTerritoryHistory?> GetByIdAsync(int id);
    Task AddAsync(SalesTerritoryHistory entity);
    void Update(SalesTerritoryHistory entity);
    void Delete(SalesTerritoryHistory entity);
}