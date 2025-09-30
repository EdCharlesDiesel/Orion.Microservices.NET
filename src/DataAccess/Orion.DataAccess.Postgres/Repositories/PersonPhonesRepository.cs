using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;

public class PersonPhonesRepository(OrionDbContext context): IPersonPhonesRepository
{
    public async Task<IEnumerable<PersonPhone>> GetAllAsync() =>
        await context.PersonPhones.ToListAsync();

    public async Task<PersonPhone?> GetByIdAsync(int id) =>
        await context.PersonPhones.FindAsync(id);

    public async Task AddAsync(PersonPhone entity) =>
        await context.PersonPhones.AddAsync(entity);

    public void Update(PersonPhone entity) =>
        context.PersonPhones.Update(entity);

    public void Delete(PersonPhone entity) =>
        context.PersonPhones.Remove(entity);
}