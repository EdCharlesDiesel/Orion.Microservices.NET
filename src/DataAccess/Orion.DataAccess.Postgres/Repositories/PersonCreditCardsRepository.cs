using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.Tools;

namespace Orion.DataAccess.Postgres.Repositories;

internal class PersonCreditCardsRepository(OrionDbContext context) : IPersonCreditCardsRepository
{
    
    public async Task<IEnumerable<PersonCreditCard>> GetAllAsync() =>
        await context.PersonCreditCards.ToListAsync();

    public async Task<PersonCreditCard?> GetByIdAsync(int id) =>
        await context.PersonCreditCards.FindAsync(id);

    public async Task AddAsync(PersonCreditCard entity) =>
        await context.PersonCreditCards.AddAsync(entity);

    public void Update(PersonCreditCard entity) =>
        context.PersonCreditCards.Update(entity);

    public void Delete(PersonCreditCard entity) =>
        context.PersonCreditCards.Remove(entity);
}