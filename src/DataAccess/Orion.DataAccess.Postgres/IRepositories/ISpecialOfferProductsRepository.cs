using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface ISpecialOfferProductsRepository
{
    Task<IEnumerable<SpecialOfferProduct>> GetAllAsync();
    Task<SpecialOfferProduct?> GetByIdAsync(int id);
    Task AddAsync(SpecialOfferProduct entity);
    void Update(SpecialOfferProduct entity);
    void Delete(SpecialOfferProduct entity);
}