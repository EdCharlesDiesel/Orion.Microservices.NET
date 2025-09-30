using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;

public class SalesPersonQuotaHistorysRepository(OrionDbContext context) : ISalesPersonQuotaHistorysRepository
{
    public async Task<IEnumerable<SalesPersonQuotaHistory>> GetAllAsync() =>
        await context.SalesPersonQuotaHistories.ToListAsync();

    public async Task<SalesPersonQuotaHistory?> GetByIdAsync(int id) =>
        await context.SalesPersonQuotaHistories.FindAsync(id);

    public async Task AddAsync(SalesPersonQuotaHistory entity) =>
        await context.SalesPersonQuotaHistories.AddAsync(entity);

    public void Update(SalesPersonQuotaHistory entity) =>
        context.SalesPersonQuotaHistories.Update(entity);

    public void Delete(SalesPersonQuotaHistory entity) =>
        context.SalesPersonQuotaHistories.Remove(entity);
}