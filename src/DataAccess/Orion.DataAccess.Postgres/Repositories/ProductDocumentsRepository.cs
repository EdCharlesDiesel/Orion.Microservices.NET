using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;

public class ProductDocumentsRepository(OrionDbContext context) : IProductDocumentsRepository
{
    
    public async Task<IEnumerable<ProductDocument>> GetAllAsync() =>
        await context.ProductDocuments.ToListAsync();

    public async Task<ProductDocument?> GetByIdAsync(int id) =>
        await context.ProductDocuments.FindAsync(id);

    public async Task AddAsync(ProductDocument entity) =>
        await context.ProductDocuments.AddAsync(entity);

    public void Update(ProductDocument entity) =>
        context.ProductDocuments.Update(entity);

    public void Delete(ProductDocument entity) =>
        context.ProductDocuments.Remove(entity);
}