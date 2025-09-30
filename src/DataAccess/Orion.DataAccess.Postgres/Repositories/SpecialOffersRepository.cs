using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;

public class SpecialOffersRepository(OrionDbContext context) : ISpecialOffersRepository
{
    public async Task<IEnumerable<SpecialOffer>> GetAllAsync() =>
        await context.SpecialOffers.ToListAsync();

    public async Task<SpecialOffer?> GetByIdAsync(int id) =>
        await context.SpecialOffers.FindAsync(id);

    public async Task AddAsync(SpecialOffer entity) =>
        await context.SpecialOffers.AddAsync(entity);

    public void Update(SpecialOffer entity) =>
        context.SpecialOffers.Update(entity);

    public void Delete(SpecialOffer entity) =>
        context.SpecialOffers.Remove(entity);
}