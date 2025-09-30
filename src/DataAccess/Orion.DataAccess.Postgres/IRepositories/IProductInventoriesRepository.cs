using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface IProductInventoriesRepository
{
    Task<IEnumerable<ProductInventory>> GetAllAsync();
    Task<ProductInventory?> GetByIdAsync(int id);
    Task AddAsync(ProductInventory entity);
    void Update(ProductInventory entity);
    void Delete(ProductInventory entity);
}