using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.Tools;

namespace Orion.DataAccess.Postgres.Repositories;

public class CurrencysRepository(OrionDbContext context) : ICurrencysRepository
{
    
    public async Task<IEnumerable<Currency>> GetAllAsync() =>
        await context.Currencies.ToListAsync();

    public async Task<Currency?> GetByIdAsync(int id) =>
        await context.Currencies.FindAsync(id);

    public async Task AddAsync(Currency entity) =>
        await context.Currencies.AddAsync(entity);

    public void Update(Currency entity) =>
        context.Currencies.Update(entity);

    public void Delete(Currency entity) =>
        context.Currencies.Remove(entity);
}