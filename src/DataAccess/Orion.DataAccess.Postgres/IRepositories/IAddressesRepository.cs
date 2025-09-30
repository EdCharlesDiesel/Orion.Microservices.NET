using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface IAddressesRepository
{
    Task<IEnumerable<Address>> GetAllAsync();
    Task<Address?> GetByIdAsync(int id);
    Task AddAsync(Address entity);
    void Update(Address entity);
    void Delete(Address entity);
}