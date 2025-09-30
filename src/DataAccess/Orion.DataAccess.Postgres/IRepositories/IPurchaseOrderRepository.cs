using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.Tools;

public interface IPurchaseOrderRepository
{
    Task<IEnumerable<ProductModel>> GetAllAsync();
    Task<ProductModel?> GetByIdAsync(int id);
    Task AddAsync(ProductModel entity);
    void Update(ProductModel entity);
    void Delete(ProductModel entity);
}