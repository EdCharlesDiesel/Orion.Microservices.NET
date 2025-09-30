using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface ISalesTerritorysRepository
{
    Task<IEnumerable<SalesTerritory>> GetAllAsync();
    Task<SalesTerritory?> GetByIdAsync(int id);
    Task AddAsync(SalesTerritory entity);
    void Update(SalesTerritory entity);
    void Delete(SalesTerritory entity);
}