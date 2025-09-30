using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;


public class AddressTypesRepository(OrionDbContext context) : IAddressTypesRepository
{
    public async Task<IEnumerable<AddressType>> GetAllAsync() =>
        await context.AddressTypes.ToListAsync();

    public async Task<AddressType?> GetByIdAsync(int id) =>
        await context.AddressTypes.FindAsync(id);

    public async Task AddAsync(AddressType entity) =>
        await context.AddressTypes.AddAsync(entity);

    public void Update(AddressType entity) =>
        context.AddressTypes.Update(entity);

    public void Delete(AddressType entity) =>
        context.AddressTypes.Remove(entity);
}