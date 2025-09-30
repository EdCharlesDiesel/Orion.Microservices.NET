using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;

public class IllustrationsRepository(OrionDbContext context) : IIllustrationsRepository
{
    
    public async Task<IEnumerable<Illustration>> GetAllAsync() =>
        await context.Illustrations.ToListAsync();

    public async Task<Illustration?> GetByIdAsync(int id) =>
        await context.Illustrations.FindAsync(id);

    public async Task AddAsync(Illustration entity) =>
        await context.Illustrations.AddAsync(entity);

    public void Update(Illustration entity) =>
        context.Illustrations.Update(entity);

    public void Delete(Illustration entity) =>
        context.Illustrations.Remove(entity);
}