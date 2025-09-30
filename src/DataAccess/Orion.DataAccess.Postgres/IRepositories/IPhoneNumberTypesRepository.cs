using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface IPhoneNumberTypesRepository
{
    Task<IEnumerable<PhoneNumberType>> GetAllAsync();
    Task<PhoneNumberType?> GetByIdAsync(int id);
    Task AddAsync(PhoneNumberType entity);
    void Update(PhoneNumberType entity);
    void Delete(PhoneNumberType entity);
}