namespace Orion.DataAccess.Postgres.IRepositories;

public interface IProductsRepository
{
    Task<IEnumerable<Entities.Product>> GetAllAsync();
    Task<Entities.Product?> GetByIdAsync(int id);
    Task AddAsync(Entities.Product entity);
    void Update(Entities.Product entity);
    void Delete(Entities.Product entity);
}