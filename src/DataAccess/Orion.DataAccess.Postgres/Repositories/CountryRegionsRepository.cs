using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;

public class CountryRegionsRepository(OrionDbContext context) : ICountryRegionsRepository
{
    public async Task<IEnumerable<CountryRegion>> GetAllAsync() =>
        await context.CountryRegions.ToListAsync();

    public async Task<CountryRegion?> GetByIdAsync(int id) =>
        await context.CountryRegions.FindAsync(id);

    public async Task AddAsync(CountryRegion entity) =>
        await context.CountryRegions.AddAsync(entity);

    public void Update(CountryRegion entity) =>
        context.CountryRegions.Update(entity);

    public void Delete(CountryRegion entity) =>
        context.CountryRegions.Remove(entity);
}