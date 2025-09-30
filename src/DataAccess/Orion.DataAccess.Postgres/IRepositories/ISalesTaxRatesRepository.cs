using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface ISalesTaxRatesRepository
{
    Task<IEnumerable<SalesTaxRate>> GetAllAsync();
    Task<SalesTaxRate?> GetByIdAsync(int id);
    Task AddAsync(SalesTaxRate entity);
    void Update(SalesTaxRate entity);
    void Delete(SalesTaxRate entity);
}