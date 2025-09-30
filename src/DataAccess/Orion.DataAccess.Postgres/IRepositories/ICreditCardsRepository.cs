using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.Tools;

public interface ICreditCardsRepository
{
    Task<IEnumerable<CreditCard>> GetAllAsync();
    Task<CreditCard?> GetByIdAsync(int id);
    Task AddAsync(CreditCard entity);
    void Update(CreditCard entity);
    void Delete(CreditCard entity);
}