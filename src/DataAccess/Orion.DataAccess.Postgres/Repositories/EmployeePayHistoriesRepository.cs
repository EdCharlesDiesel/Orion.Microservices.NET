using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;


public class EmployeePayHistoriesRepository(OrionDbContext context): IEmployeePayHistoriesRepository
{
    public async Task<IEnumerable<EmployeePayHistory>> GetAllAsync() =>
        await context.EmployeePayHistories.ToListAsync();

    public async Task<EmployeePayHistory?> GetByIdAsync(int id) =>
        await context.EmployeePayHistories.FindAsync(id);

    public async Task AddAsync(EmployeePayHistory entity) =>
        await context.EmployeePayHistories.AddAsync(entity);

    public void Update(EmployeePayHistory entity) =>
        context.EmployeePayHistories.Update(entity);

    public void Delete(EmployeePayHistory entity) =>
        context.EmployeePayHistories.Remove(entity);
}