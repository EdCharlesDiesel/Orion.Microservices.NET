using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface IIllustrationsRepository
{
    Task<IEnumerable<Illustration>> GetAllAsync();
    Task<Illustration?> GetByIdAsync(int id);
    Task AddAsync(Illustration entity);
    void Update(Illustration entity);
    void Delete(Illustration entity);
}