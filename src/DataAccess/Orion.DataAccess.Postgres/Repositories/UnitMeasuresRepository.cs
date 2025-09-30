using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;

public class UnitMeasuresRepository(OrionDbContext context) : IUnitMeasuresRepository
{
    public async Task<IEnumerable<UnitMeasure>> GetAllAsync() =>
        await context.UnitMeasures.ToListAsync();

    public async Task<UnitMeasure?> GetByIdAsync(int id) =>
        await context.UnitMeasures.FindAsync(id);

    public async Task AddAsync(UnitMeasure entity) =>
        await context.UnitMeasures.AddAsync(entity);

    public void Update(UnitMeasure entity) =>
        context.UnitMeasures.Update(entity);

    public void Delete(UnitMeasure entity) =>
        context.UnitMeasures.Remove(entity);
}