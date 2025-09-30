using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface IProductModelIllustrationsRepository
{
    Task<IEnumerable<ProductModelIllustration>> GetAllAsync();
    Task<ProductModelIllustration?> GetByIdAsync(int id);
    Task AddAsync(ProductModelIllustration entity);
    void Update(ProductModelIllustration entity);
    void Delete(ProductModelIllustration entity);
}