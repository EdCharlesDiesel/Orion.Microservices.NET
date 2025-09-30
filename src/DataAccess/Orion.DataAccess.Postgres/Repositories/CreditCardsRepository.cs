using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.Tools;

namespace Orion.DataAccess.Postgres.Repositories;


public class CreditCardsRepository(OrionDbContext context) : ICreditCardsRepository
{
    public async Task<IEnumerable<CreditCard>> GetAllAsync() =>
        await context.CreditCards.ToListAsync();

    public async Task<CreditCard?> GetByIdAsync(int id) =>
        await context.CreditCards.FindAsync(id);

    public async Task AddAsync(CreditCard entity) =>
        await context.CreditCards.AddAsync(entity);

    public void Update(CreditCard entity) =>
        context.CreditCards.Update(entity);

    public void Delete(CreditCard entity) =>
        context.CreditCards.Remove(entity);
}