using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;

internal class ProductCostHistoriesRepository(OrionDbContext context) : IProductCostHistorysRepository
{
    
    public async Task<IEnumerable<ProductCostHistory>> GetAllAsync() =>
        await context.ProductCostHistories.ToListAsync();

    public async Task<ProductCostHistory?> GetByIdAsync(int id) =>
        await context.ProductCostHistories.FindAsync(id);

    public async Task AddAsync(ProductCostHistory entity) =>
        await context.ProductCostHistories.AddAsync(entity);

    public void Update(ProductCostHistory entity) =>
        context.ProductCostHistories.Update(entity);

    public void Delete(ProductCostHistory entity) =>
        context.ProductCostHistories.Remove(entity);
}