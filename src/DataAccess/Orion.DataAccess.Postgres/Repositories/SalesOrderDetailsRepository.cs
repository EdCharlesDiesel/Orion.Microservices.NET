using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;

public class SalesOrderDetailsRepository(OrionDbContext context) : ISalesOrderDetailsRepository
{
    public async Task<IEnumerable<SalesOrderDetail>> GetAllAsync() =>
        await context.SalesOrderDetails.ToListAsync();

    public async Task<SalesOrderDetail?> GetByIdAsync(int id) =>
        await context.SalesOrderDetails.FindAsync(id);

    public async Task AddAsync(SalesOrderDetail entity) =>
        await context.SalesOrderDetails.AddAsync(entity);

    public void Update(SalesOrderDetail entity) =>
        context.SalesOrderDetails.Update(entity);

    public void Delete(SalesOrderDetail entity) =>
        context.SalesOrderDetails.Remove(entity);
}