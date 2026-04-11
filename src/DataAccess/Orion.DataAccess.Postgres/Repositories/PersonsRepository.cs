using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;
public class PersonsRepository(OrionDbContext context) : IPersonsRepository
{
    public async Task<IEnumerable<Person>> GetAllAsync() =>
        await context.Persons.ToListAsync();

    public async Task<Person?> GetByIdAsync(int id) =>
        await context.Persons.FindAsync(id);

    public async Task AddAsync(Person entity) =>
        await context.Persons.AddAsync(entity);

    public void Update(Person entity) =>
        context.Persons.Update(entity);

    public void Delete(Person entity) =>
        context.Persons.Remove(entity);
}