using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface ICountryRegionCurrencysRepository
{
    Task<IEnumerable<CountryRegionCurrency>> GetAllAsync();
    Task<CountryRegionCurrency?> GetByIdAsync(int id);
    Task AddAsync(CountryRegionCurrency entity);
    void Update(CountryRegionCurrency entity);
    void Delete(CountryRegionCurrency entity);
}