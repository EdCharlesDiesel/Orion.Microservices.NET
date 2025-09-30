using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;
using Orion.DataAccess.Postgres.Tools;

namespace Orion.DataAccess.Postgres.Repositories;

public class CountryRegionCurrencysRepository(OrionDbContext context) : ICountryRegionCurrencysRepository
{
    public async Task<IEnumerable<CountryRegionCurrency>> GetAllAsync() =>
        await context.CountryRegionCurrencies.ToListAsync();

    public async Task<CountryRegionCurrency?> GetByIdAsync(int id) =>
        await context.CountryRegionCurrencies.FindAsync(id);

    public async Task AddAsync(CountryRegionCurrency entity) =>
        await context.CountryRegionCurrencies.AddAsync(entity);

    public void Update(CountryRegionCurrency entity) =>
        context.CountryRegionCurrencies.Update(entity);

    public void Delete(CountryRegionCurrency entity) =>
        context.CountryRegionCurrencies.Remove(entity);
}