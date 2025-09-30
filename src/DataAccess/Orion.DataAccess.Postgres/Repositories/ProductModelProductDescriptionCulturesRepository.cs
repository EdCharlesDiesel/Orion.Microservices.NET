using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;


public class ProductModelProductDescriptionCulturesRepository(OrionDbContext context) : IProductModelProductDescriptionCulturesRepository
{
    
    public async Task<IEnumerable<ProductModelProductDescriptionCulture>> GetAllAsync() =>
        await context.ProductModelProductDescriptionCultures.ToListAsync();

    public async Task<ProductModelProductDescriptionCulture?> GetByIdAsync(int id) =>
        await context.ProductModelProductDescriptionCultures.FindAsync(id);

    public async Task AddAsync(ProductModelProductDescriptionCulture entity) =>
        await context.ProductModelProductDescriptionCultures.AddAsync(entity);

    public void Update(ProductModelProductDescriptionCulture entity) =>
        context.ProductModelProductDescriptionCultures.Update(entity);

    public void Delete(ProductModelProductDescriptionCulture entity) =>
        context.ProductModelProductDescriptionCultures.Remove(entity);
}