using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface ICountryRegionsRepository
{
    Task<IEnumerable<CountryRegion>> GetAllAsync();
    Task<CountryRegion?> GetByIdAsync(int id);
    Task AddAsync(CountryRegion entity);
    void Update(CountryRegion entity);
    void Delete(CountryRegion entity);
}