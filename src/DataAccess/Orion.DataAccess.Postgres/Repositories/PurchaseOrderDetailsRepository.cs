using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.Tools;

namespace Orion.DataAccess.Postgres.Repositories;

public class PurchaseOrderDetailsRepository(OrionDbContext context) : IPurchaseOrderDetailsRepository
{
    public async Task<IEnumerable<PurchaseOrderDetail>> GetAllAsync() =>
        await context.PurchaseOrderDetails.ToListAsync();

    public async Task<PurchaseOrderDetail?> GetByIdAsync(int id) =>
        await context.PurchaseOrderDetails.FindAsync(id);

    public async Task AddAsync(PurchaseOrderDetail entity) =>
        await context.PurchaseOrderDetails.AddAsync(entity);

    public void Update(PurchaseOrderDetail entity) =>
        context.PurchaseOrderDetails.Update(entity);

    public void Delete(PurchaseOrderDetail entity) =>
        context.PurchaseOrderDetails.Remove(entity);
}