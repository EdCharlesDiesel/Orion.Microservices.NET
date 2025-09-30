using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;


public class LocationsRepository(OrionDbContext context) : ILocationsRepository
{
    
    public async Task<IEnumerable<Location>> GetAllAsync() =>
        await context.Locations.ToListAsync();

    public async Task<Location?> GetByIdAsync(int id) =>
        await context.Locations.FindAsync(id);

    public async Task AddAsync(Location entity) =>
        await context.Locations.AddAsync(entity);

    public void Update(Location entity) =>
        context.Locations.Update(entity);

    public void Delete(Location entity) =>
        context.Locations.Remove(entity);
}