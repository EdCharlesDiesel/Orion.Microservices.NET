using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface IPersonsRepository
{
    Task<IEnumerable<Person>> GetAllAsync();
    Task<Person?> GetByIdAsync(int id);
    Task AddAsync(Person entity);
    void Update(Person entity);
    void Delete(Person entity);
}