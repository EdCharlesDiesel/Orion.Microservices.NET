using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;

public class ErrorLogsRepository(OrionDbContext context) : IErrorLogsRepository
{
    public async Task<IEnumerable<ErrorLog>> GetAllAsync() =>
        await context.ErrorLogs.ToListAsync();

    public async Task<ErrorLog?> GetByIdAsync(int id) =>
        await context.ErrorLogs.FindAsync(id);

    public async Task AddAsync(ErrorLog entity) =>
        await context.ErrorLogs.AddAsync(entity);

    public void Update(ErrorLog entity) =>
        context.ErrorLogs.Update(entity);

    public void Delete(ErrorLog entity) =>
        context.ErrorLogs.Remove(entity);
}