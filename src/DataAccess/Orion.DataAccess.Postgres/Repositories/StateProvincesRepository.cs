using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;

public class StateProvincesRepository(OrionDbContext context) : IStateProvincesRepository
{
    public async Task<IEnumerable<StateProvince>> GetAllAsync() =>
        await context.StateProvinces.ToListAsync();

    public async Task<StateProvince?> GetByIdAsync(int id) =>
        await context.StateProvinces.FindAsync(id);

    public async Task AddAsync(StateProvince entity) =>
        await context.StateProvinces.AddAsync(entity);

    public void Update(StateProvince entity) =>
        context.StateProvinces.Update(entity);

    public void Delete(StateProvince entity) =>
        context.StateProvinces.Remove(entity);
}