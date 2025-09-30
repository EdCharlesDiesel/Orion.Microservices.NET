using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;

public class ProductInventoriesRepository(OrionDbContext context) : IProductInventoriesRepository
{
    
    public async Task<IEnumerable<ProductInventory>> GetAllAsync() =>
        await context.ProductInventories.ToListAsync();

    public async Task<ProductInventory?> GetByIdAsync(int id) =>
        await context.ProductInventories.FindAsync(id);

    public async Task AddAsync(ProductInventory entity) =>
        await context.ProductInventories.AddAsync(entity);

    public void Update(ProductInventory entity) =>
        context.ProductInventories.Update(entity);

    public void Delete(ProductInventory entity) =>
        context.ProductInventories.Remove(entity);
}