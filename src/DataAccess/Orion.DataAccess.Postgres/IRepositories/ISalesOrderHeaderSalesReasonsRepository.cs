using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface ISalesOrderHeaderSalesReasonsRepository
{
    Task<IEnumerable<SalesOrderHeaderSalesReason>> GetAllAsync();
    Task<SalesOrderHeaderSalesReason?> GetByIdAsync(int id);
    Task AddAsync(SalesOrderHeaderSalesReason entity);
    void Update(SalesOrderHeaderSalesReason entity);
    void Delete(SalesOrderHeaderSalesReason entity);
}