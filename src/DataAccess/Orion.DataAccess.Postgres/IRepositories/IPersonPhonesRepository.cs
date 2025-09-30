using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface IPersonPhonesRepository
{
    Task<IEnumerable<PersonPhone>> GetAllAsync();
    Task<PersonPhone?> GetByIdAsync(int id);
    Task AddAsync(PersonPhone entity);
    void Update(PersonPhone entity);
    void Delete(PersonPhone entity);
}