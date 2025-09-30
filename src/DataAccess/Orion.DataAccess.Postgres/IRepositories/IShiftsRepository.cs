using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface IShiftsRepository
{
    Task<IEnumerable<Shift>> GetAllAsync();
    Task<Shift?> GetByIdAsync(int id);
    Task AddAsync(Shift entity);
    void Update(Shift entity);
    void Delete(Shift entity);
}