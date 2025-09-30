using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;

public class EmailAddressesRepository(OrionDbContext context) : IEmailAddressesRepository
{
    public async Task<IEnumerable<EmailAddress>> GetAllAsync() =>
        await context.EmailAddresses.ToListAsync();

    public async Task<EmailAddress?> GetByIdAsync(int id) =>
        await context.EmailAddresses.FindAsync(id);

    public async Task AddAsync(EmailAddress entity) =>
        await context.EmailAddresses.AddAsync(entity);

    public void Update(EmailAddress entity) =>
        context.EmailAddresses.Update(entity);

    public void Delete(EmailAddress entity) =>
        context.EmailAddresses.Remove(entity);
}