using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;

public class VendorsRepository(OrionDbContext context) : IVendorsRepository
{
    public async Task<IEnumerable<Vendor>> GetAllAsync() =>
        await context.Vendors.ToListAsync();

    public async Task<Vendor?> GetByIdAsync(int id) =>
        await context.Vendors.FindAsync(id);

    public async Task AddAsync(Vendor entity) =>
        await context.Vendors.AddAsync(entity);

    public void Update(Vendor entity) =>
        context.Vendors.Update(entity);

    public void Delete(Vendor entity) =>
        context.Vendors.Remove(entity);
}