using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;


public class DepartmentsRepository(OrionDbContext context): IDepartmentsRepository
{
    public async Task<IEnumerable<Department>> GetAllAsync() =>
        await context.Departments.ToListAsync();

    public async Task<Department?> GetByIdAsync(int id) =>
        await context.Departments.FindAsync(id);

    public async Task AddAsync(Department entity) =>
        await context.Departments.AddAsync(entity);

    public void Update(Department entity) =>
        context.Departments.Update(entity);

    public void Delete(Department entity) =>
        context.Departments.Remove(entity);
}