using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;


public class ProductPhotosRepository(OrionDbContext context) : IProductPhotosRepository
{
    
    public async Task<IEnumerable<ProductPhoto>> GetAllAsync() =>
        await context.ProductPhotos.ToListAsync();

    public async Task<ProductPhoto?> GetByIdAsync(int id) =>
        await context.ProductPhotos.FindAsync(id);

    public async Task AddAsync(ProductPhoto entity) =>
        await context.ProductPhotos.AddAsync(entity);

    public void Update(ProductPhoto entity) =>
        context.ProductPhotos.Update(entity);

    public void Delete(ProductPhoto entity) =>
        context.ProductPhotos.Remove(entity);
}