using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface ILocationsRepository
{
    Task<IEnumerable<Location>> GetAllAsync();
    Task<Location?> GetByIdAsync(int id);
    Task AddAsync(Location entity);
    void Update(Location entity);
    void Delete(Location entity);
}