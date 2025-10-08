using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface ICurrencysRepository
{
    Task<IEnumerable<Currency>> GetAllAsync();
    Task<Currency?> GetByIdAsync(string id);
    Task AddAsync(Currency entity);
    void Update(Currency entity);
    void Delete(Currency entity);
}