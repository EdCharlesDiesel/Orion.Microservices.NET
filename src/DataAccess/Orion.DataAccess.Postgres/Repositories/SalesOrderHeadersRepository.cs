using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;

public class SalesOrderHeadersRepository(OrionDbContext context) : ISalesOrderHeadersRepository
{
    public async Task<IEnumerable<SalesOrderHeader>> GetAllAsync() =>
        await context.SalesOrderHeaders.ToListAsync();

    public async Task<SalesOrderHeader?> GetByIdAsync(int id) =>
        await context.SalesOrderHeaders.FindAsync(id);

    public async Task AddAsync(SalesOrderHeader entity) =>
        await context.SalesOrderHeaders.AddAsync(entity);

    public void Update(SalesOrderHeader entity) =>
        context.SalesOrderHeaders.Update(entity);

    public void Delete(SalesOrderHeader entity) =>
        context.SalesOrderHeaders.Remove(entity);
}