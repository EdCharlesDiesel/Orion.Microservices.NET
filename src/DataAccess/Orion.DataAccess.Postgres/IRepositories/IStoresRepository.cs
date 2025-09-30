using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface IStoresRepository
{
    Task<IEnumerable<Store>> GetAllAsync();
    Task<Store?> GetByIdAsync(int id);
    Task AddAsync(Store entity);
    void Update(Store entity);
    void Delete(Store entity);
}