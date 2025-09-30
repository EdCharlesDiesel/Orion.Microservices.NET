using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;

public class ContactTypesRepository(OrionDbContext context) : IContactTypesRepository
{
    public async Task<IEnumerable<ContactType>> GetAllAsync() =>
        await context.ContactTypes.ToListAsync();

    public async Task<ContactType?> GetByIdAsync(int id) =>
        await context.ContactTypes.FindAsync(id);

    public async Task AddAsync(ContactType entity) =>
        await context.ContactTypes.AddAsync(entity);

    public void Update(ContactType entity) =>
        context.ContactTypes.Update(entity);

    public void Delete(ContactType entity) =>
        context.ContactTypes.Remove(entity);
}