using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories
{
    public class EmployeeDepartmentHistoriesRepository(OrionDbContext context) : IEmployeeDepartmentHistoriesRepository
    {
        public async Task<IEnumerable<EmployeeDepartmentHistory>> GetAllAsync() =>
            await context.EmployeeDepartmentHistories.ToListAsync();

        public async Task<EmployeeDepartmentHistory?> GetByIdAsync(int id) =>
            await context.EmployeeDepartmentHistories.FindAsync(id);

        public async Task AddAsync(EmployeeDepartmentHistory entity) =>
            await context.EmployeeDepartmentHistories.AddAsync(entity);

        public void Update(EmployeeDepartmentHistory entity) =>
            context.EmployeeDepartmentHistories.Update(entity);

        public void Delete(EmployeeDepartmentHistory entity) =>
            context.EmployeeDepartmentHistories.Remove(entity);
    }
}