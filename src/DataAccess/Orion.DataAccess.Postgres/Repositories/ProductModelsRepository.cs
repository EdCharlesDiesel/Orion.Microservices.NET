using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;


public class ProductModelsRepository(OrionDbContext context) : IProductModelsRepository
{
    
    public async Task<IEnumerable<ProductModel>> GetAllAsync() =>
        await context.ProductModels.ToListAsync();

    public async Task<ProductModel?> GetByIdAsync(int id) =>
        await context.ProductModels.FindAsync(id);

    public async Task AddAsync(ProductModel entity) =>
        await context.ProductModels.AddAsync(entity);

    public void Update(ProductModel entity) =>
        context.ProductModels.Update(entity);

    public void Delete(ProductModel entity) =>
        context.ProductModels.Remove(entity);
}