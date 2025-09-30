using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface IWorkOrderRoutingsRepository
{
    Task<IEnumerable<WorkOrderRouting>> GetAllAsync();
    Task<WorkOrderRouting?> GetByIdAsync(int id);
    Task AddAsync(WorkOrderRouting entity);
    void Update(WorkOrderRouting entity);
    void Delete(WorkOrderRouting entity);
}