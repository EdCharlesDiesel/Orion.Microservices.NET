using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;

public class ProductModelIllustrationsRepository(OrionDbContext context) : IProductModelIllustrationsRepository
{
    
    public async Task<IEnumerable<ProductModelIllustration>> GetAllAsync() =>
        await context.ProductModelIllustrations.ToListAsync();

    public async Task<ProductModelIllustration?> GetByIdAsync(int id) =>
        await context.ProductModelIllustrations.FindAsync(id);

    public async Task AddAsync(ProductModelIllustration entity) =>
        await context.ProductModelIllustrations.AddAsync(entity);

    public void Update(ProductModelIllustration entity) =>
        context.ProductModelIllustrations.Update(entity);

    public void Delete(ProductModelIllustration entity) =>
        context.ProductModelIllustrations.Remove(entity);
}