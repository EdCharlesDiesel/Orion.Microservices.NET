using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;

public class SalesTerritoryHistorysRepository(OrionDbContext context) : ISalesTerritoryHistorysRepository
{
    public async Task<IEnumerable<SalesTerritoryHistory>> GetAllAsync() =>
        await context.SalesTerritoryHistories.ToListAsync();

    public async Task<SalesTerritoryHistory?> GetByIdAsync(int id) =>
        await context.SalesTerritoryHistories.FindAsync(id);

    public async Task AddAsync(SalesTerritoryHistory entity) =>
        await context.SalesTerritoryHistories.AddAsync(entity);

    public void Update(SalesTerritoryHistory entity) =>
        context.SalesTerritoryHistories.Update(entity);

    public void Delete(SalesTerritoryHistory entity) =>
        context.SalesTerritoryHistories.Remove(entity);
}