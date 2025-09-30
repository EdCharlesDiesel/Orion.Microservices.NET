using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface ISalesOrderDetailsRepository
{
    Task<IEnumerable<SalesOrderDetail>> GetAllAsync();
    Task<SalesOrderDetail?> GetByIdAsync(int id);
    Task AddAsync(SalesOrderDetail entity);
    void Update(SalesOrderDetail entity);
    void Delete(SalesOrderDetail entity);
}