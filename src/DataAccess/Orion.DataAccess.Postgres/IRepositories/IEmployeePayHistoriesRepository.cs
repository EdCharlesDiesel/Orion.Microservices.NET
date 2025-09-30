using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface IEmployeePayHistoriesRepository
{
    Task<IEnumerable<EmployeePayHistory>> GetAllAsync();
    Task<EmployeePayHistory?> GetByIdAsync(int id);
    Task AddAsync(EmployeePayHistory entity);
    void Update(EmployeePayHistory entity);
    void Delete(EmployeePayHistory entity);
}