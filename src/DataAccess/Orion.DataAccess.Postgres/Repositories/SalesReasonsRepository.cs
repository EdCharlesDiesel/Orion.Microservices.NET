using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;

public class SalesReasonsRepository(OrionDbContext context) : ISalesReasonsRepository
{
    public async Task<IEnumerable<SalesReason>> GetAllAsync() =>
        await context.SalesReasons.ToListAsync();

    public async Task<SalesReason?> GetByIdAsync(int id) =>
        await context.SalesReasons.FindAsync(id);

    public async Task AddAsync(SalesReason entity) =>
        await context.SalesReasons.AddAsync(entity);

    public void Update(SalesReason entity) =>
        context.SalesReasons.Update(entity);

    public void Delete(SalesReason entity) =>
        context.SalesReasons.Remove(entity);
}