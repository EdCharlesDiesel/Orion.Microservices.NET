using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.Tools;

public interface IPurchaseOrderHeadersRepository
{
    Task<IEnumerable<PurchaseOrderHeader>> GetAllAsync();
    Task<PurchaseOrderHeader?> GetByIdAsync(int id);
    Task AddAsync(PurchaseOrderHeader entity);
    void Update(PurchaseOrderHeader entity);
    void Delete(PurchaseOrderHeader entity);
}