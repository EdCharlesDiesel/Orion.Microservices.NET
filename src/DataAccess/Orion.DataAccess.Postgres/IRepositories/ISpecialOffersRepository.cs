using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface ISpecialOffersRepository
{
    Task<IEnumerable<SpecialOffer>> GetAllAsync();
    Task<SpecialOffer?> GetByIdAsync(int id);
    Task AddAsync(SpecialOffer entity);
    void Update(SpecialOffer entity);
    void Delete(SpecialOffer entity);
}