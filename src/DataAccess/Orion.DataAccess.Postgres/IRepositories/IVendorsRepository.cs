using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface IVendorsRepository
{
    Task<IEnumerable<Vendor>> GetAllAsync();
    Task<Vendor?> GetByIdAsync(int id);
    Task AddAsync(Vendor entity);
    void Update(Vendor entity);
    void Delete(Vendor entity);
}