using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface IProductCategoriesRepository
{
    Task<IEnumerable<ProductCategory>> GetAllAsync();
    Task<ProductCategory?> GetByIdAsync(int id);
    Task AddAsync(ProductCategory entity);
    void Update(ProductCategory entity);
    void Delete(ProductCategory entity);
}