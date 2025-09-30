using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface IAddressTypesRepository
{
    Task<IEnumerable<AddressType>> GetAllAsync();
    Task<AddressType?> GetByIdAsync(int id);
    Task AddAsync(AddressType entity);
    void Update(AddressType entity);
    void Delete(AddressType entity);
}