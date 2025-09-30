using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface ISalesPersonsRepository
{
    Task<IEnumerable<SalesPerson>> GetAllAsync();
    Task<SalesPerson?> GetByIdAsync(int id);
    Task AddAsync(SalesPerson entity);
    void Update(SalesPerson entity);
    void Delete(SalesPerson entity);
}