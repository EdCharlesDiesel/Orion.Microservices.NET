using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface IProductCostHistorysRepository
{
    Task<IEnumerable<ProductCostHistory>> GetAllAsync();
    Task<ProductCostHistory?> GetByIdAsync(int id);
    Task AddAsync(ProductCostHistory entity);
    void Update(ProductCostHistory entity);
    void Delete(ProductCostHistory entity);
}