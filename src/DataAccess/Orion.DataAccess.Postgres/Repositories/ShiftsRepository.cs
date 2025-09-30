using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;

public class ShiftsRepository(OrionDbContext context) : IShiftsRepository
{
    public async Task<IEnumerable<Shift>> GetAllAsync() =>
        await context.Shifts.ToListAsync();

    public async Task<Shift?> GetByIdAsync(int id) =>
        await context.Shifts.FindAsync(id);

    public async Task AddAsync(Shift entity) =>
        await context.Shifts.AddAsync(entity);

    public void Update(Shift entity) =>
        context.Shifts.Update(entity);

    public void Delete(Shift entity) =>
        context.Shifts.Remove(entity);
}