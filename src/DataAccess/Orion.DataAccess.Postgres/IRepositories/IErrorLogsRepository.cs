using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface IErrorLogsRepository
{
    Task<IEnumerable<ErrorLog>> GetAllAsync();
    Task<ErrorLog?> GetByIdAsync(int id);
    Task AddAsync(ErrorLog entity);
    void Update(ErrorLog entity);
    void Delete(ErrorLog entity);
}