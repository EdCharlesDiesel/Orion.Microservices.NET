using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.Tools;

public interface ICurrencyRatesRepository
{
    Task<IEnumerable<CurrencyRate>> GetAllAsync();
    Task<CurrencyRate?> GetByIdAsync(int id);
    Task AddAsync(CurrencyRate entity);
    void Update(CurrencyRate entity);
    void Delete(CurrencyRate entity);
}