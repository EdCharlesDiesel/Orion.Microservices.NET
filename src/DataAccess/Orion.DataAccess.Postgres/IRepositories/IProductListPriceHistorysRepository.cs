using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface IProductListPriceHistorysRepository
{
    Task<IEnumerable<ProductListPriceHistory>> GetAllAsync();
    Task<ProductListPriceHistory?> GetByIdAsync(int id);
    Task AddAsync(ProductListPriceHistory entity);
    void Update(ProductListPriceHistory entity);
    void Delete(ProductListPriceHistory entity);
}