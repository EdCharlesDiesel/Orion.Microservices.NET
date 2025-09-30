using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;

public class SalesOrderHeaderSalesReasonsRepository(OrionDbContext context) : ISalesOrderHeaderSalesReasonsRepository
{
    public async Task<IEnumerable<SalesOrderHeaderSalesReason>> GetAllAsync() =>
        await context.SalesOrderHeaderSalesReasons.ToListAsync();

    public async Task<SalesOrderHeaderSalesReason?> GetByIdAsync(int id) =>
        await context.SalesOrderHeaderSalesReasons.FindAsync(id);

    public async Task AddAsync(SalesOrderHeaderSalesReason entity) =>
        await context.SalesOrderHeaderSalesReasons.AddAsync(entity);

    public void Update(SalesOrderHeaderSalesReason entity) =>
        context.SalesOrderHeaderSalesReasons.Update(entity);

    public void Delete(SalesOrderHeaderSalesReason entity) =>
        context.SalesOrderHeaderSalesReasons.Remove(entity);
}