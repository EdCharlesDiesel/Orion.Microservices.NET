using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.Tools;

namespace Orion.DataAccess.Postgres.Repositories;

public class ProductVendorsRepository(OrionDbContext context) : IProductVendorsRepository
{
    public async Task<IEnumerable<ProductVendor>> GetAllAsync() =>
        await context.ProductVendors.ToListAsync();

    public async Task<ProductVendor?> GetByIdAsync(int id) =>
        await context.ProductVendors.FindAsync(id);

    public async Task AddAsync(ProductVendor entity) =>
        await context.ProductVendors.AddAsync(entity);

    public void Update(ProductVendor entity) =>
        context.ProductVendors.Update(entity);

    public void Delete(ProductVendor entity) =>
        context.ProductVendors.Remove(entity);
}