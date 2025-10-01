using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;

public class InternalEmployeesRepository(OrionDbContext context) : IInternalEmployeesRepository
{
    public async Task<IEnumerable<Employee>> GetAllAsync() =>
        await context.Employees.ToListAsync();

    public async Task<Employee?> GetByIdAsync(int id) =>
        await context.Employees.FindAsync(id);

    public async Task AddAsync(Employee entity) =>
        await context.Employees.AddAsync(entity);

    public void Update(Employee entity) =>
        context.Employees.Update(entity);

    public void Delete(Employee entity) =>
        context.Employees.Remove(entity);
}