using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;

public class BillOfMaterialsRepository(OrionDbContext context) : IBillOfMaterialsRepository
{
    public async Task<IEnumerable<BillOfMaterials>> GetAllAsync() =>
        await context.BillOfMaterials.ToListAsync();

    public async Task<BillOfMaterials?> GetByIdAsync(int id) =>
        await context.BillOfMaterials.FindAsync(id);

    public async Task AddAsync(BillOfMaterials entity) =>
        await context.BillOfMaterials.AddAsync(entity);

    public void Update(BillOfMaterials entity) =>
        context.BillOfMaterials.Update(entity);

    public void Delete(BillOfMaterials entity) =>
        context.BillOfMaterials.Remove(entity);
}