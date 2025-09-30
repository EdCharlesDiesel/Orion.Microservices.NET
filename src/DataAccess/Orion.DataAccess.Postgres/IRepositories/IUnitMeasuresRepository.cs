using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface IUnitMeasuresRepository
{
    Task<IEnumerable<UnitMeasure>> GetAllAsync();
    Task<UnitMeasure?> GetByIdAsync(int id);
    Task AddAsync(UnitMeasure entity);
    void Update(UnitMeasure entity);
    void Delete(UnitMeasure entity);
}