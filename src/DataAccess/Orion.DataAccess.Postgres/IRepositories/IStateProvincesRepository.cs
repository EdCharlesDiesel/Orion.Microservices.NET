using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface IStateProvincesRepository
{
    Task<IEnumerable<StateProvince>> GetAllAsync();
    Task<StateProvince?> GetByIdAsync(int id);
    Task AddAsync(StateProvince entity);
    void Update(StateProvince entity);
    void Delete(StateProvince entity);
}