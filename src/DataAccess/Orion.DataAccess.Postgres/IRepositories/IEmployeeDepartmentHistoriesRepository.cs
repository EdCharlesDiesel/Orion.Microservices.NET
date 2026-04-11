using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface IEmployeeDepartmentHistoriesRepository
{
    Task<IEnumerable<EmployeeDepartmentHistory>> GetAllAsync();
    Task<EmployeeDepartmentHistory?> GetByIdAsync(int id);
    Task AddAsync(EmployeeDepartmentHistory entity);
    void Update(EmployeeDepartmentHistory entity);
    void Delete(EmployeeDepartmentHistory entity);
}