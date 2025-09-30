using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface ISalesOrderHeadersRepository
{
    Task<IEnumerable<SalesOrderHeader>> GetAllAsync();
    Task<SalesOrderHeader?> GetByIdAsync(int id);
    Task AddAsync(SalesOrderHeader entity);
    void Update(SalesOrderHeader entity);
    void Delete(SalesOrderHeader entity);
}