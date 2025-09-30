using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.Tools;

namespace Orion.DataAccess.Postgres.Repositories;

public class CurrencyRatesRepository(OrionDbContext context) : ICurrencyRatesRepository
{
    
    public async Task<IEnumerable<CurrencyRate>> GetAllAsync() =>
        await context.CurrencyRates.ToListAsync();

    public async Task<CurrencyRate?> GetByIdAsync(int id) =>
        await context.CurrencyRates.FindAsync(id);

    public async Task AddAsync(CurrencyRate entity) =>
        await context.CurrencyRates.AddAsync(entity);

    public void Update(CurrencyRate entity) =>
        context.CurrencyRates.Update(entity);

    public void Delete(CurrencyRate entity) =>
        context.CurrencyRates.Remove(entity);
}