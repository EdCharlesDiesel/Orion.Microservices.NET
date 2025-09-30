using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.Tools;

public interface IProductVendorsRepository
{
    Task<IEnumerable<ProductVendor>> GetAllAsync();
    Task<ProductVendor?> GetByIdAsync(int id);
    Task AddAsync(ProductVendor entity);
    void Update(ProductVendor entity);
    void Delete(ProductVendor entity);
}