using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface IWorkOrdersRepository
{
    Task<IEnumerable<WorkOrder>> GetAllAsync();
    Task<WorkOrder?> GetByIdAsync(int id);
    Task AddAsync(WorkOrder entity);
    void Update(WorkOrder entity);
    void Delete(WorkOrder entity);
}