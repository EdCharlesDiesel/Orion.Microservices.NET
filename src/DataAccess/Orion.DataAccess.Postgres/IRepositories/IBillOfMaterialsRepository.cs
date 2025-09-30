using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface IBillOfMaterialsRepository
{
    Task<IEnumerable<BillOfMaterials>> GetAllAsync();
    Task<BillOfMaterials?> GetByIdAsync(int id);
    Task AddAsync(BillOfMaterials entity);
    void Update(BillOfMaterials entity);
    void Delete(BillOfMaterials entity);
}