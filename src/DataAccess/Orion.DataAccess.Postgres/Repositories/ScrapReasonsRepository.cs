using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;


public class ScrapReasonsRepository(OrionDbContext context) : IScrapReasonsRepository
{
    public async Task<IEnumerable<ScrapReason>> GetAllAsync() =>
        await context.ScrapReasons.ToListAsync();

    public async Task<ScrapReason?> GetByIdAsync(int id) =>
        await context.ScrapReasons.FindAsync(id);

    public async Task AddAsync(ScrapReason entity) =>
        await context.ScrapReasons.AddAsync(entity);

    public void Update(ScrapReason entity) =>
        context.ScrapReasons.Update(entity);

    public void Delete(ScrapReason entity) =>
        context.ScrapReasons.Remove(entity);
}