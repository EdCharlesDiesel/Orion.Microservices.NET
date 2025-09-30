using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;



public class PhoneNumberTypesRepository(OrionDbContext context): IPhoneNumberTypesRepository
{
    public async Task<IEnumerable<PhoneNumberType>> GetAllAsync() =>
        await context.PhoneNumberTypes.ToListAsync();

    public async Task<PhoneNumberType?> GetByIdAsync(int id) =>
        await context.PhoneNumberTypes.FindAsync(id);

    public async Task AddAsync(PhoneNumberType entity) =>
        await context.PhoneNumberTypes.AddAsync(entity);

    public void Update(PhoneNumberType entity) =>
        context.PhoneNumberTypes.Update(entity);

    public void Delete(PhoneNumberType entity) =>
        context.PhoneNumberTypes.Remove(entity);
}