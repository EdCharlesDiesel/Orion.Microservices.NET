using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface IProductModelProductDescriptionCulturesRepository
{
    Task<IEnumerable<ProductModelProductDescriptionCulture>> GetAllAsync();
    Task<ProductModelProductDescriptionCulture?> GetByIdAsync(int id);
    Task AddAsync(ProductModelProductDescriptionCulture entity);
    void Update(ProductModelProductDescriptionCulture entity);
    void Delete(ProductModelProductDescriptionCulture entity);
}