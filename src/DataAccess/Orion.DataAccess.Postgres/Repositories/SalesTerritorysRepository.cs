using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;

public class SalesTerritorysRepository(OrionDbContext context) : ISalesTerritorysRepository
{
    public async Task<IEnumerable<SalesTerritory>> GetAllAsync() =>
        await context.SalesTerritories.ToListAsync();

    public async Task<SalesTerritory?> GetByIdAsync(int id) =>
        await context.SalesTerritories.FindAsync(id);

    public async Task AddAsync(SalesTerritory entity) =>
        await context.SalesTerritories.AddAsync(entity);

    public void Update(SalesTerritory entity) =>
        context.SalesTerritories.Update(entity);

    public void Delete(SalesTerritory entity) =>
        context.SalesTerritories.Remove(entity);
}