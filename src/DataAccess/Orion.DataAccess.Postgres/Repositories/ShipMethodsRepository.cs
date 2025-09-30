using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;
using Orion.DataAccess.Postgres.Tools;

namespace Orion.DataAccess.Postgres.Repositories;

public class ShipMethodsRepository(OrionDbContext context) : IShipMethodsRepository
{
    public async Task<IEnumerable<ShipMethod>> GetAllAsync() =>
        await context.ShipMethods.ToListAsync();

    public async Task<ShipMethod?> GetByIdAsync(int id) =>
        await context.ShipMethods.FindAsync(id);

    public async Task AddAsync(ShipMethod entity) =>
        await context.ShipMethods.AddAsync(entity);

    public void Update(ShipMethod entity) =>
        context.ShipMethods.Update(entity);

    public void Delete(ShipMethod entity) =>
        context.ShipMethods.Remove(entity);
}