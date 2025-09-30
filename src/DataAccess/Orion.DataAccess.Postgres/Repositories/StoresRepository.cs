using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;

public class StoresRepository(OrionDbContext context) : IStoresRepository
{
    public async Task<IEnumerable<Store>> GetAllAsync() =>
        await context.Stores.ToListAsync();

    public async Task<Store?> GetByIdAsync(int id) =>
        await context.Stores.FindAsync(id);

    public async Task AddAsync(Store entity) =>
        await context.Stores.AddAsync(entity);

    public void Update(Store entity) =>
        context.Stores.Update(entity);

    public void Delete(Store entity) =>
        context.Stores.Remove(entity);
}