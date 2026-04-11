using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;


public class AddressesRepository(OrionDbContext context) : IAddressesRepository
{
    public async Task<IEnumerable<Address>> GetAllAsync() =>
        await context.Addresses.ToListAsync();

    public async Task<Address?> GetByIdAsync(int id) =>
        await context.Addresses.FindAsync(id);

    public async Task AddAsync(Address entity) =>
        await context.Addresses.AddAsync(entity);

    public void Update(Address entity) =>
        context.Addresses.Update(entity);

    public void Delete(Address entity) =>
        context.Addresses.Remove(entity);
}