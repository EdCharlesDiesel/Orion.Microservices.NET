using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;

public class SalesTaxRatesRepository(OrionDbContext context) : ISalesTaxRatesRepository
{
    public async Task<IEnumerable<SalesTaxRate>> GetAllAsync() =>
        await context.SalesTaxRates.ToListAsync();

    public async Task<SalesTaxRate?> GetByIdAsync(int id) =>
        await context.SalesTaxRates.FindAsync(id);

    public async Task AddAsync(SalesTaxRate entity) =>
        await context.SalesTaxRates.AddAsync(entity);

    public void Update(SalesTaxRate entity) =>
        context.SalesTaxRates.Update(entity);

    public void Delete(SalesTaxRate entity) =>
        context.SalesTaxRates.Remove(entity);
}