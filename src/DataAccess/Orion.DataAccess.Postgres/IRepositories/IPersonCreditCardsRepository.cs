using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.Tools;

public interface IPersonCreditCardsRepository
{
    Task<IEnumerable<PersonCreditCard>> GetAllAsync();
    Task<PersonCreditCard?> GetByIdAsync(int id);
    Task AddAsync(PersonCreditCard entity);
    void Update(PersonCreditCard entity);
    void Delete(PersonCreditCard entity);
}