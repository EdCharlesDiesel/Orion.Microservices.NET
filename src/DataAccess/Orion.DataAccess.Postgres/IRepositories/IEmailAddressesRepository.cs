using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;


public interface IEmailAddressesRepository
{
    Task<IEnumerable<EmailAddress>> GetAllAsync();
    Task<EmailAddress?> GetByIdAsync(int id);
    Task AddAsync(EmailAddress entity);
    void Update(EmailAddress entity);
    void Delete(EmailAddress entity);
}