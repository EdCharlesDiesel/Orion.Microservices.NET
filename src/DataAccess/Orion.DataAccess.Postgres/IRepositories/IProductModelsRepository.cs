using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface IProductModelsRepository
{
    Task<IEnumerable<ProductModel>> GetAllAsync();
    Task<ProductModel?> GetByIdAsync(int id);
    Task AddAsync(ProductModel entity);
    void Update(ProductModel entity);
    void Delete(ProductModel entity);
}