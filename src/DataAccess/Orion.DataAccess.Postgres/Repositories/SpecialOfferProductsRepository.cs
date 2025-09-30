using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;

public class SpecialOfferProductsRepository(OrionDbContext context) : ISpecialOfferProductsRepository
{
    public async Task<IEnumerable<SpecialOfferProduct>> GetAllAsync() =>
        await context.SpecialOfferProducts.ToListAsync();

    public async Task<SpecialOfferProduct?> GetByIdAsync(int id) =>
        await context.SpecialOfferProducts.FindAsync(id);

    public async Task AddAsync(SpecialOfferProduct entity) =>
        await context.SpecialOfferProducts.AddAsync(entity);

    public void Update(SpecialOfferProduct entity) =>
        context.SpecialOfferProducts.Update(entity);

    public void Delete(SpecialOfferProduct entity) =>
        context.SpecialOfferProducts.Remove(entity);
}