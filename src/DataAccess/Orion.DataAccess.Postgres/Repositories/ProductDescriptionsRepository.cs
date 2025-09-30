using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;


public class ProductDescriptionsRepository(OrionDbContext context) : IProductDescriptionsRepository
{
    
    public async Task<IEnumerable<ProductDescription>> GetAllAsync() =>
        await context.ProductDescriptions.ToListAsync();

    public async Task<ProductDescription?> GetByIdAsync(int id) =>
        await context.ProductDescriptions.FindAsync(id);

    public async Task AddAsync(ProductDescription entity) =>
        await context.ProductDescriptions.AddAsync(entity);

    public void Update(ProductDescription entity) =>
        context.ProductDescriptions.Update(entity);

    public void Delete(ProductDescription entity) =>
        context.ProductDescriptions.Remove(entity);
}