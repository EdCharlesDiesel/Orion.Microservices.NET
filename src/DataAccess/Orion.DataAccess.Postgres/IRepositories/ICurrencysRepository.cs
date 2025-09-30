using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.Tools;

public interface ICurrencysRepository
{
    Task<IEnumerable<Currency>> GetAllAsync();
    Task<Currency?> GetByIdAsync(int id);
    Task AddAsync(Currency entity);
    void Update(Currency entity);
    void Delete(Currency entity);
}