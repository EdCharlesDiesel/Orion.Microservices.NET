using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;


public class WorkOrderRoutingsRepository(OrionDbContext context) : IWorkOrderRoutingsRepository
{
    public async Task<IEnumerable<WorkOrderRouting>> GetAllAsync() =>
        await context.WorkOrderRoutings.ToListAsync();

    public async Task<WorkOrderRouting?> GetByIdAsync(int id) =>
        await context.WorkOrderRoutings.FindAsync(id);

    public async Task AddAsync(WorkOrderRouting entity) =>
        await context.WorkOrderRoutings.AddAsync(entity);

    public void Update(WorkOrderRouting entity) =>
        context.WorkOrderRoutings.Update(entity);

    public void Delete(WorkOrderRouting entity) =>
        context.WorkOrderRoutings.Remove(entity);
}