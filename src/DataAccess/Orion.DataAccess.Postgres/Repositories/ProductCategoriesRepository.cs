using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;

public class ProductCategoriesRepository(OrionDbContext context) : IProductCategoriesRepository
{
    
    public async Task<IEnumerable<ProductCategory>> GetAllAsync() =>
        await context.ProductCategories.ToListAsync();

    public async Task<ProductCategory?> GetByIdAsync(int id) =>
        await context.ProductCategories.FindAsync(id);

    public async Task AddAsync(ProductCategory entity) =>
        await context.ProductCategories.AddAsync(entity);

    public void Update(ProductCategory entity) =>
        context.ProductCategories.Update(entity);

    public void Delete(ProductCategory entity) =>
        context.ProductCategories.Remove(entity);
}