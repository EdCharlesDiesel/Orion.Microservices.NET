using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface IProductDocumentsRepository
{
    Task<IEnumerable<ProductDocument>> GetAllAsync();
    Task<ProductDocument?> GetByIdAsync(int id);
    Task AddAsync(ProductDocument entity);
    void Update(ProductDocument entity);
    void Delete(ProductDocument entity);
}