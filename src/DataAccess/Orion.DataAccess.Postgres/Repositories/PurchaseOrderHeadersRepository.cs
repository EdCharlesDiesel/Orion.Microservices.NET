using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.Tools;

namespace Orion.DataAccess.Postgres.Repositories;


public class PurchaseOrderHeadersRepository(OrionDbContext context) : IPurchaseOrderHeadersRepository
{
    public async Task<IEnumerable<PurchaseOrderHeader>> GetAllAsync() =>
        await context.PurchaseOrderHeaders.ToListAsync();

    public async Task<PurchaseOrderHeader?> GetByIdAsync(int id) =>
        await context.PurchaseOrderHeaders.FindAsync(id);

    public async Task AddAsync(PurchaseOrderHeader entity) =>
        await context.PurchaseOrderHeaders.AddAsync(entity);

    public void Update(PurchaseOrderHeader entity) =>
        context.PurchaseOrderHeaders.Update(entity);

    public void Delete(PurchaseOrderHeader entity) =>
        context.PurchaseOrderHeaders.Remove(entity);
}