using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface IProductDescriptionsRepository
{
    Task<IEnumerable<ProductDescription>> GetAllAsync();
    Task<ProductDescription?> GetByIdAsync(int id);
    Task AddAsync(ProductDescription entity);
    void Update(ProductDescription entity);
    void Delete(ProductDescription entity);
}