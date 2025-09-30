using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;

public class SalesPersonsRepository(OrionDbContext context) : ISalesPersonsRepository
{
    public async Task<IEnumerable<SalesPerson>> GetAllAsync() =>
        await context.SalesPersons.ToListAsync();

    public async Task<SalesPerson?> GetByIdAsync(int id) =>
        await context.SalesPersons.FindAsync(id);

    public async Task AddAsync(SalesPerson entity) =>
        await context.SalesPersons.AddAsync(entity);

    public void Update(SalesPerson entity) =>
        context.SalesPersons.Update(entity);

    public void Delete(SalesPerson entity) =>
        context.SalesPersons.Remove(entity);
}