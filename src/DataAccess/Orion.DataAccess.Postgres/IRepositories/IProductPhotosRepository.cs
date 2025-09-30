using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface IProductPhotosRepository
{
    Task<IEnumerable<ProductPhoto>> GetAllAsync();
    Task<ProductPhoto?> GetByIdAsync(int id);
    Task AddAsync(ProductPhoto entity);
    void Update(ProductPhoto entity);
    void Delete(ProductPhoto entity);
}