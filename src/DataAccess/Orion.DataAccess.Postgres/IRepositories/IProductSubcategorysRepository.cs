using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface IProductSubcategorysRepository
{
    Task<IEnumerable<ProductSubcategory>> GetAllAsync();
    Task<ProductSubcategory?> GetByIdAsync(int id);
    Task AddAsync(ProductSubcategory entity);
    void Update(ProductSubcategory entity);
    void Delete(ProductSubcategory entity);
}