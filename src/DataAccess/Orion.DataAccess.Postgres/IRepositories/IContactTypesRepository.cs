using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface IContactTypesRepository
{
    
    Task<IEnumerable<ContactType>> GetAllAsync();
    Task<ContactType?> GetByIdAsync(int id);
    Task AddAsync(ContactType entity);
    void Update(ContactType entity);
    void Delete(ContactType entity);
    
}

