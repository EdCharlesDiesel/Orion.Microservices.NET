using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;

public class ProductListPriceHistorysRepository(OrionDbContext context) : IProductListPriceHistorysRepository
{
    
    public async Task<IEnumerable<ProductListPriceHistory>> GetAllAsync() =>
        await context.ProductListPriceHistories.ToListAsync();

    public async Task<ProductListPriceHistory?> GetByIdAsync(int id) =>
        await context.ProductListPriceHistories.FindAsync(id);

    public async Task AddAsync(ProductListPriceHistory entity) =>
        await context.ProductListPriceHistories.AddAsync(entity);

    public void Update(ProductListPriceHistory entity) =>
        context.ProductListPriceHistories.Update(entity);

    public void Delete(ProductListPriceHistory entity) =>
        context.ProductListPriceHistories.Remove(entity);
}