using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;

public class WorkOrdersRepository(OrionDbContext context) : IWorkOrdersRepository
{
    public async Task<IEnumerable<WorkOrder>> GetAllAsync() =>
        await context.WorkOrders.ToListAsync();

    public async Task<WorkOrder?> GetByIdAsync(int id) =>
        await context.WorkOrders.FindAsync(id);

    public async Task AddAsync(WorkOrder entity) =>
        await context.WorkOrders.AddAsync(entity);

    public void Update(WorkOrder entity) =>
        context.WorkOrders.Update(entity);

    public void Delete(WorkOrder entity) =>
        context.WorkOrders.Remove(entity);
}