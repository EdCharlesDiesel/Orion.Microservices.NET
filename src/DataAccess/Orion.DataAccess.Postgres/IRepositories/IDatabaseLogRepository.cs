using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface IDatabaseLogRepository
{
    Task<IEnumerable<DatabaseLog>> GetAllAsync();
    Task<DatabaseLog?> GetByIdAsync(int id);
    Task AddAsync(DatabaseLog entity);
    void Update(DatabaseLog entity);
    void Delete(DatabaseLog entity);
}



