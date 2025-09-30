using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.IRepositories;
using Product = Orion.DataAccess.Postgres.Entities.Product;

namespace Orion.DataAccess.Postgres.Repositories
{

    public class ProductsRepository(OrionDbContext context) : IProductsRepository
    {
        public async Task<IEnumerable<Product>> GetAllAsync() =>
            await context.Products.ToListAsync();

        public async Task<Product?> GetByIdAsync(int id) =>
            await context.Products.FindAsync(id);

        public async Task AddAsync(Product entity) =>
            await context.Products.AddAsync(entity);

        public void Update(Product entity) =>
            context.Products.Update(entity);

        public void Delete(Product entity) =>
            context.Products.Remove(entity);
    }

}
