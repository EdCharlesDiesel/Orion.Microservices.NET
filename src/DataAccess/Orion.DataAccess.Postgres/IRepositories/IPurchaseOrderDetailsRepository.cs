using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.Tools;

public interface IPurchaseOrderDetailsRepository
{
    Task<IEnumerable<PurchaseOrderDetail>> GetAllAsync();
    Task<PurchaseOrderDetail?> GetByIdAsync(int id);
    Task AddAsync(PurchaseOrderDetail entity);
    void Update(PurchaseOrderDetail entity);
    void Delete(PurchaseOrderDetail entity);
}