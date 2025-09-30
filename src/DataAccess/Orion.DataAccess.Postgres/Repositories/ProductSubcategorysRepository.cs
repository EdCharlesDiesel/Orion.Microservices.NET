using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;

public class ProductSubcategorysRepository(OrionDbContext context) : IProductSubcategorysRepository
{
    
    public async Task<IEnumerable<ProductSubcategory>> GetAllAsync() =>
        await context.ProductSubcategories.ToListAsync();

    public async Task<ProductSubcategory?> GetByIdAsync(int id) =>
        await context.ProductSubcategories.FindAsync(id);

    public async Task AddAsync(ProductSubcategory entity) =>
        await context.ProductSubcategories.AddAsync(entity);

    public void Update(ProductSubcategory entity) =>
        context.ProductSubcategories.Update(entity);

    public void Delete(ProductSubcategory entity) =>
        context.ProductSubcategories.Remove(entity);
}