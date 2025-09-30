using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface IDepartmentsRepository
{
    Task<IEnumerable<Department>> GetAllAsync();
    Task<Department?> GetByIdAsync(int id);
    Task AddAsync(Department entity);
    void Update(Department entity);
    void Delete(Department entity);
}